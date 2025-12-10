using ElasticSentinel.Domain.Common;
using ElasticSentinel.Application.Common.Models;
using ElasticSentinel.Application.Features.ElasticQueries;
using ElasticSentinel.Application.Features.Notifications;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using Quartz;

namespace ElasticSentinel.Infrastructure.BackgroundJobs
{
    [DisallowConcurrentExecution]
    public class ElasticQueryManagerJob : IJob
    {
        private readonly IExecuteElasticQueryHandler _executeQueryHandler;
        private readonly IRenderNotificationHandler _renderNotificationHandler;
        private readonly ISendEmailNotificationHandler _sendEmailHandler;
        private readonly ISendTeamsNotificationHandler _sendTeamsHandler;
        private readonly ILogger<ElasticQueryManagerJob> _logger;
        private readonly IHubContext<JobsHub> _hubContext;

        public ElasticQueryManagerJob(
            IExecuteElasticQueryHandler executeQueryHandler,
            IRenderNotificationHandler renderNotificationHandler,
            ISendEmailNotificationHandler sendEmailHandler,
            ISendTeamsNotificationHandler sendTeamsHandler,
            IHubContext<JobsHub> hubContext,
            ILogger<ElasticQueryManagerJob> logger)
        {
            _executeQueryHandler = executeQueryHandler;
            _renderNotificationHandler = renderNotificationHandler;
            _sendEmailHandler = sendEmailHandler;
            _sendTeamsHandler = sendTeamsHandler;
            _logger = logger;
            _hubContext = hubContext;
        }

        private async Task SendLogMessage(string message)
        {
            const string jobName = SentinelConstants.JOB_HUB_NAME;
            string currentDtTm = DateTime.UtcNow.ToString("MM/dd/yyyyTHH:mm:ssZ");
            await _hubContext.Clients.All.SendAsync(jobName, currentDtTm, message);
        }

        public async Task Execute(IJobExecutionContext context)
        {
            string? strTracker = default;
            try
            {
                var dataMap = context.MergedJobDataMap;
                string strSchedulerName = (string)dataMap["schedulerName"];
                string strApiRequest = (string)dataMap["elasticApiRequest"];
                string strResponse = (string)dataMap["responseStr"];
                string strTemplate = (string)dataMap["template"];
                string strEmailConnector = (string)dataMap["mailConnectorDetails"];
                string strTeamsConnectorDetails = (string)dataMap["teamsConnectorDetails"];

                strTracker = $"{strSchedulerName} : Started executing";
                await SendLogMessage(strTracker);

                if (strApiRequest != null)
                {
                    var apiRequest = JsonConvert.DeserializeObject<ElasticQueryAPIRequest>(strApiRequest);
                    var responseStr = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(strResponse);
                    if (apiRequest != null && responseStr != null)
                    {
                        // Execute Elasticsearch query using handler
                        var executeQueryRequest = new ExecuteElasticQueryRequest(apiRequest, responseStr, _logger);
                        var docs = await _executeQueryHandler.HandleAsync(executeQueryRequest, context.CancellationToken);

                        bool isNotify = true;
                        if (docs != null && docs.Count > 0)
                        {
                            if (docs.Count == 1
                                && docs[0].Count == 1
                                && docs[0].FirstOrDefault().Key.ToLower().Contains("count"))
                            {
                                _ = int.TryParse(docs[0].FirstOrDefault().Value, out int count);
                                if (count <= 0)
                                {
                                    isNotify = false;
                                }
                                else
                                {
                                    strTracker = $"{strSchedulerName} : There have been {count} non-approved transactions";
                                    await SendLogMessage(strTracker);
                                }
                            }
                            if (isNotify)
                            {
                                strTracker = $"{strSchedulerName} : There have been {docs.Count} notifications";
                                await SendLogMessage(strTracker);
                                var emailConnectorDetails = JsonConvert.DeserializeObject<EmailConnectorDetails>(strEmailConnector);
                                var teamsConnectorDetails = JsonConvert.DeserializeObject<TeamsConnectorDetails>(strTeamsConnectorDetails);

                                if (emailConnectorDetails != null || teamsConnectorDetails != null)
                                {
                                    var str = JsonConvert.SerializeObject(docs);

                                    // Render notification template using handler
                                    var renderRequest = new RenderNotificationWithListRequest(strTemplate, docs);
                                    string strMessage = await _renderNotificationHandler.HandleAsync(renderRequest, context.CancellationToken);

                                    // Send notifications using handlers
                                    if (emailConnectorDetails != null)
                                    {
                                        var emailRequest = new SendEmailNotificationRequest(strMessage, emailConnectorDetails);
                                        await _sendEmailHandler.HandleAsync(emailRequest, context.CancellationToken);
                                    }

                                    if (teamsConnectorDetails != null)
                                    {
                                        var teamsRequest = new SendTeamsNotificationRequest(strMessage, teamsConnectorDetails);
                                        await _sendTeamsHandler.HandleAsync(teamsRequest, context.CancellationToken);
                                    }
                                }
                            }
                        }
                    }
                }

                strTracker = $"{strSchedulerName} : Completed executing";
                await SendLogMessage(strTracker);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error at ElasticQueryManagerJob");
            }
        }
    }
}
