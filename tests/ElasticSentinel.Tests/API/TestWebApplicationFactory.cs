using ElasticSentinel.Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ElasticSentinel.Tests.API;

public class TestWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly SqliteConnection _connection;
    private readonly object _seedLock = new object();

    public TestWebApplicationFactory()
    {
        // Create and open the connection immediately during construction
        // This ensures it's done before any test classes are instantiated
        _connection = new SqliteConnection("Data Source=InMemoryTestDb;Mode=Memory;Cache=Shared");
        _connection.Open();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Remove existing DbContext registration
            var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<SentinelDbContext>));
            if (descriptor != null)
                services.Remove(descriptor);

            // Add SQLite test database using the pre-opened connection
            services.AddDbContext<SentinelDbContext>(options =>
            {
                options.UseSqlite(_connection);
            });
        });

        builder.UseEnvironment("Testing");
    }

    public void SeedDatabase()
    {
        lock (_seedLock)
        {
            using var scope = Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<SentinelDbContext>();
            context.Database.EnsureCreated();
            TestDataSeeder.CleanupTestData(context);
            TestDataSeeder.SeedTestData(context);
        }
    }

    public HttpClient CreateAuthenticatedClient()
    {
        var client = CreateClient();
        client.DefaultRequestHeaders.Add("X-API-KEY", "test-api-key");
        return client;
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _connection.Close();
            _connection.Dispose();
        }
        base.Dispose(disposing);
    }
}
