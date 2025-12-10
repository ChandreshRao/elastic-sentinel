using Quartz;

using ElasticSentinel.Application.Common.Interfaces;
using ElasticSentinel.Domain.Common;
using ElasticSentinel.Infrastructure.Persistence;

namespace ElasticSentinel.Infrastructure.BackgroundJobs
{
    public class NotifyManagerJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            throw new NotImplementedException();
        }
    }
}
