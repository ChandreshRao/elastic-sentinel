using ElasticSentinel.Infrastructure.BackgroundJobs;
using ElasticSentinel.Application.Common.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using Quartz;

using ElasticSentinel.Application.Common.Interfaces;
using ElasticSentinel.Domain.Common;
using ElasticSentinel.Infrastructure.Persistence;

namespace ElasticSentinel.Infrastructure.BackgroundJobs
{
    public class JobManagerService : IJobManagerService
    {
        private IScheduler? _scheduler;
        private CancellationToken? _ct;


        public async Task HandleJobs(SentinelDbContext dbContext, IScheduler scheduler, ILogger logger, CancellationToken ct)
        {
            string strTracker = "Running handle job";
            try
            {
                _scheduler = scheduler;
                _ct = ct;

                strTracker = "Fetching alert config details";
                var lstJobs = await dbContext.AlertSchedulerConfigs
                .Include(r => r.ElasticConfig)
                .Include(k => k.MailConnector)
                .Include(k => k.MailConnectorDetail)
                .Include(k => k.Template)
                .Include(k => k.TeamsConnector)
                .Where(p => p.IsEnabled).ToListAsync(cancellationToken: _ct.Value);

                strTracker = "Getting current executing jobs";
                var lstScheduledJobs = await _scheduler.GetCurrentlyExecutingJobs(_ct.Value);

                foreach (var job in lstJobs)
                {
                    strTracker = "Skipping disabled elastic host related jobs";
                    var elastic = job.ElasticConfig;
                    if (elastic == null || (!elastic?.IsEnabled ?? false))
                    {
                        continue;
                    }

                    strTracker = "Checking if the job exists";
                    var jobKey = new JobKey(job.SchedulerName, job.SchedulerGroup);

                    if (await _scheduler.CheckExists(jobKey, _ct.Value))
                    {
                        continue;
                    }

                    strTracker = "Checking if the job exists";
                    var query = await dbContext.ElasticQueries
                                    .Where(r => r.ElasticQueryId.Equals(job.ElasticQueryId) && r.IsDynamic)
                                    .Include(r => r.DynamicRequestDetail).ThenInclude(k => k.QuerySource)
                                    .Include(r => r.DynamicResponseDetail).ThenInclude(k => k.QueryResponseStructures)
                                    .FirstOrDefaultAsync(cancellationToken: _ct.Value);

                    if (query != null && elastic != null)
                    {
                        strTracker = "Constructing job request paramaters";
                        var requestDetail = query.DynamicRequestDetail;
                        var responseDetail = query.DynamicResponseDetail;
                        var resStructures = responseDetail?.QueryResponseStructures;

                        if (requestDetail != null
                            && responseDetail != null
                            && resStructures != null)
                        {
                            ElasticQueryAPIRequest apiRequest = new()
                            {
                                AuthType = "Basic",
                                ElasticHost = elastic.ElasticHost,
                                UserName = elastic.UserName,
                                Password = elastic.Password,
                                QueryName = query.QueryName,
                                QuerySuffixes = new List<string> { requestDetail.IndexName!, requestDetail.QueryType! },
                                QueryParams = new Dictionary<string, string?>()
                                {
                                    { "source", requestDetail.QuerySource!.SourceQuery ?? "" },
                                    { "source_content_type", requestDetail.QuerySource.SourceType ?? ""}
                                }
                            };

                            Dictionary<string, Dictionary<string, string>> dictResMapper = new();
                            foreach (var resData in resStructures)
                            {
                                if (resData.IndexRootFieldName != null)
                                {
                                    if (dictResMapper.ContainsKey(resData.IndexRootFieldName))
                                    {
                                        if (dictResMapper[resData.IndexRootFieldName] != null)
                                        {
                                            dictResMapper[resData.IndexRootFieldName].Add(resData.IndexFieldName, resData.AliasFieldName);
                                        }
                                        continue;
                                    }
                                    dictResMapper.Add(resData.IndexRootFieldName, new Dictionary<string, string>()
                                    {
                                        { resData.IndexFieldName, resData.AliasFieldName }
                                    });
                                }
                            }

                            TeamsConnectorDetails? teamsConnectorDetails = null;
                            if (job.TeamsConnector != null)
                            {
                                teamsConnectorDetails = new()
                                {
                                    WebhookUrl = job.TeamsConnector.WebhookUrl
                                };
                            }

                            EmailConnectorDetails? emailConnectorDetails = null;
                            if (job.MailConnectorDetail != null && job.MailConnector != null)
                            {
                                emailConnectorDetails = new()
                                {
                                    EmailSubject = job.MailConnectorDetail.EmailSubject ?? "Application Alert",
                                    CcEmails = job.MailConnectorDetail.CcEmails,
                                    FromEmail = job.MailConnector.FromEmail,
                                    ToEmails = job.MailConnectorDetail.ToEmails,
                                    SMTPServer = job.MailConnector.PrimarySMTPServer,
                                    SMTPAltServer = job.MailConnector.AlternateSMTPServer,
                                    SMTPPort = job.MailConnector.SMTPPort
                                };
                            }
                            strTracker = "Creating ElasticQueryManagerJob";
                            var newJob = JobBuilder.Create<ElasticQueryManagerJob>()
                                .WithIdentity(job.SchedulerName, job.SchedulerGroup)
                                .Build();

                            var newTrigger = TriggerBuilder.Create()
                                .WithIdentity($"{job.SchedulerName}-trigger", job.SchedulerGroup)
                                .ForJob(job.SchedulerName, job.SchedulerGroup)
                                .WithCronSchedule(job.CronExp.Trim())
                                //.WithSimpleSchedule(r=>r.WithIntervalInMinutes(1).WithRepeatCount(0))
                                .Build();
                            newJob.JobDataMap.Put("schedulerName", job.SchedulerName);
                            newJob.JobDataMap.Put("elasticApiRequest", JsonConvert.SerializeObject(apiRequest));
                            newJob.JobDataMap.Put("responseStr", JsonConvert.SerializeObject(dictResMapper));
                            if (job.Template != null)
                            {
                                newJob.JobDataMap.Put("template", job.Template.TemplateContent);
                            }
                            if (emailConnectorDetails != null)
                            {
                                newJob.JobDataMap.Put("mailConnectorDetails", JsonConvert.SerializeObject(emailConnectorDetails));
                            }
                            if (teamsConnectorDetails != null)
                            {
                                newJob.JobDataMap.Put("teamsConnectorDetails", JsonConvert.SerializeObject(teamsConnectorDetails));
                            }

                            strTracker = "Scheduling ElasticQueryManagerJob";
                            await _scheduler.ScheduleJob(newJob, newTrigger, _ct.Value);

                            strTracker = "Starting ElasticQueryManagerJob";
                            await _scheduler.Start(_ct.Value);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Error while {strTracker} : Error {ex.Message}", strTracker, ex.Message);
            }

        }

        public IScheduler? GetCurrentScheduler()
        {
            return _scheduler;
        }

        public async Task<IReadOnlyCollection<IJobExecutionContext>?> GetRunningJobs()
        {
            if (_scheduler != null && _ct != null)
            {
                return await _scheduler.GetCurrentlyExecutingJobs();
            }
            return null;
        }
    }
}
