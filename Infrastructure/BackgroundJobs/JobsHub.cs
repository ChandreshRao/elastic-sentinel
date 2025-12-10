using Microsoft.AspNetCore.SignalR;

using ElasticSentinel.Application.Common.Interfaces;
using ElasticSentinel.Domain.Common;
using ElasticSentinel.Infrastructure.Persistence;

namespace ElasticSentinel.Infrastructure.BackgroundJobs
{
    public class JobsHub : Hub
    {
        public async Task SendMessage(string message)
        {
            await Clients.All.SendAsync(message);
        }
        //public Task SendConcurrentJobsMessage(string message)
        //{
        //    return Clients.All.SendAsync("ConcurrentJobs", message);
        //}

        //public Task SendNonConcurrentJobsMessage(string message)
        //{
        //    return Clients.All.SendAsync("NonConcurrentJobs", message);
        //}

    }
}
