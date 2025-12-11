
using ElasticSentinel.Application;
using ElasticSentinel.Infrastructure;
using ElasticSentinel.Infrastructure.BackgroundJobs;
using ElasticSentinel.Infrastructure.Auth;
using ElasticSentinel.Infrastructure.Persistence;
using ElasticSentinel.Presentation.API;
using ElasticSentinel.Presentation.API.Middleware;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Serilog;
using System.Text.Json;
using System.Text.Json.Serialization;

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
builder.Services.AddAuthentication("ApiKey")
    .AddScheme<AuthenticationSchemeOptions, ApiKeyAuthenticationHandler>("ApiKey", _ => { });
builder.Services.AddAuthorization();
builder.Services.AddHealthChecks()
    .AddDbContextCheck<SentinelDbContext>("database");

// Prevent JSON serialization cycles when returning EF entities with navigation properties
builder.Services.Configure<JsonOptions>(options =>
{
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

// Add Razor Pages (for existing UI)
builder.Services.AddRazorPages();

// Add API services
// Note: Swagger has compatibility issues with .NET 10.0 preview
// Uncomment when stable version is available or use .NET 9.0
// builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // Swagger disabled due to .NET 10.0 compatibility issues
    // app.UseSwagger();
    // app.UseSwaggerUI(options =>
    // {
    //     options.SwaggerEndpoint("/swagger/v1/swagger.json", "Elastic Sentinel API v1");
    //     options.RoutePrefix = "api/docs";
    // });
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

// Global exception handler for API
app.UseMiddleware<GlobalExceptionHandler>();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Enable CORS
app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

// Map API endpoints
app.MapApiEndpoints();

// Health checks (returns JSON payload)
app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = async (context, report) =>
    {
        context.Response.ContentType = "application/json";

        var payload = new
        {
            status = report.Status.ToString(),
            results = report.Entries.ToDictionary(
                entry => entry.Key,
                entry => new
                {
                    status = entry.Value.Status.ToString(),
                    description = entry.Value.Description,
                    data = entry.Value.Data
                })
        };

        var json = JsonSerializer.Serialize(payload, new JsonSerializerOptions
        {
            WriteIndented = true
        });

        await context.Response.WriteAsync(json);
    }
});

// Map Razor Pages and SignalR Hub
app.MapHub<JobsHub>("/jobshub");
app.MapRazorPages();
app.MapFallbackToPage("/Home");

app.Lifetime.ApplicationStarted.Register(() => _logger.Information("Application Started"));
app.Lifetime.ApplicationStopped.Register(() => _logger.Information("Application Stopped"));

app.Run();
