using ElasticSentinel.Infrastructure.Persistence;
using Quartz;

namespace ElasticSentinel.Application.Common.Interfaces
{
    public interface IJobManagerService
    {
        IScheduler? GetCurrentScheduler();

        Task<IReadOnlyCollection<IJobExecutionContext>?> GetRunningJobs();

        Task HandleJobs(SentinelDbContext dbContext, IScheduler scheduler, ILogger logger, CancellationToken ct);
    }
}
