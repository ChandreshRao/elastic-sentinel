
//using Elastic.Transport;
using ElasticSentinel.Application;
using ElasticSentinel.Infrastructure;
using ElasticSentinel.Infrastructure.BackgroundJobs;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
// Serilog configuration        
var _logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext().CreateLogger();

builder.Logging.AddSerilog(_logger);

// Add services using Clean Architecture layers
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

// Add services to the container.
builder.Services.AddRazorPages();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Modern routing for SignalR hub
app.MapHub<JobsHub>("/jobshub");

app.Lifetime.ApplicationStarted.Register(() => _logger.Information("Application Started"));
app.Lifetime.ApplicationStopped.Register(() => _logger.Information("Application Stopped"));

app.MapRazorPages();
app.MapFallbackToPage("/Home");

app.Run();
