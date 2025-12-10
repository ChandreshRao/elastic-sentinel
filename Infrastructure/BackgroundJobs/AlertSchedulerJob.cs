using ElasticSentinel.Domain.Common;
using ElasticSentinel.Application.Common.Interfaces;
using ElasticSentinel.Infrastructure.Persistence;
using Microsoft.AspNetCore.SignalR;
using Quartz;

namespace ElasticSentinel.Infrastructure.BackgroundJobs
{
    [DisallowConcurrentExecution]
    public class AlertSchedulerJob : IJob
    {
        private readonly ILogger<AlertSchedulerJob> _logger;
        private readonly IHubContext<JobsHub> _hubContext;
        private readonly SentinelDbContext _sentinelDbContext;
        private readonly IJobManagerService _jobManagerService;

        public AlertSchedulerJob(ILogger<AlertSchedulerJob> logger,
           IHubContext<JobsHub> hubContext, 
           IJobManagerService jobManagerService, 
           SentinelDbContext sentinelDbContext)
        {
            _logger = logger;
            _hubContext = hubContext;
            _jobManagerService = jobManagerService;
            _sentinelDbContext = sentinelDbContext;
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
                strTracker = $"{SentinelConstants.SCHEDULER_JOB_NAME} : Started executing";
                await SendLogMessage(strTracker);
                await _jobManagerService.HandleJobs(_sentinelDbContext, context.Scheduler, _logger, context.CancellationToken);
                strTracker = $"{SentinelConstants.SCHEDULER_JOB_NAME} : Completed executing";
                await SendLogMessage(strTracker);
            }
            catch (Exception ex)
            {
                string? message = $"Error in {SentinelConstants.SCHEDULER_JOB_NAME} at {strTracker} : {ex.Message}";
                _logger.LogError(message, ex);
            }
        }
    }
}
