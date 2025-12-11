using ElasticSentinel.Application.Common.Interfaces;
using ElasticSentinel.Infrastructure.BackgroundJobs;
using ElasticSentinel.Infrastructure.Persistence;
using ElasticSentinel.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Quartz;

namespace ElasticSentinel.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Database
        services.AddDbContext<SentinelDbContext>(options =>
        {
            options.UseSqlite(configuration.GetConnectionString("SentinelDb"));
        });

        // HTTP Client for internal API calls from Razor Pages
        services.AddHttpClient<ApiClientService>(client =>
        {
            client.BaseAddress = new Uri("http://localhost:5000");
            client.DefaultRequestHeaders.Add("Accept", "application/json");
        });

        // Background Jobs Management (Infrastructure concern)
        services.AddSingleton<IJobManagerService, JobManagerService>();

        // SignalR
        services.AddSignalR();

        // Quartz Background Jobs
        services.AddQuartz(q =>
        {
            var nonConconcurrentJobKey = new JobKey("Alert-Scheduler-Job", "Alert-Main-Group");
            q.AddJob<AlertSchedulerJob>(opts => opts.WithIdentity(nonConconcurrentJobKey).StoreDurably(true));
            q.AddTrigger(opts => opts
                .ForJob(nonConconcurrentJobKey)
                .WithIdentity("Alert-Scheduler-Job-trigger", "Alert-Main-Group-Trigger")
                .WithSimpleSchedule(o => o.WithRepeatCount(0).WithInterval(TimeSpan.FromSeconds(10))));
        });
        
        services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

        return services;
    }
}
