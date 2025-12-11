using System.Net;
using System.Net.Http.Json;
using ElasticSentinel.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace ElasticSentinel.Tests.API.Endpoints;

[Collection("API Tests")]
public class SchedulerEndpointsTests
{
    private readonly HttpClient _client;
    private readonly TestWebApplicationFactory _factory;

    public SchedulerEndpointsTests(TestWebApplicationFactory factory)
    {
        _factory = factory;
        _factory.SeedDatabase(); // Seed data before each test class
        _client = factory.CreateAuthenticatedClient();
    }

    [Fact]
    public async Task GetAllSchedulerConfigs_ReturnsOk()
    {
        // Act
        var response = await _client.GetAsync("/api/scheduler/configs");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var configs = await response.Content.ReadFromJsonAsync<List<AlertSchedulerConfig>>();
        configs.Should().NotBeNull();
    }

    [Fact]
    public async Task CreateSchedulerConfig_WithValidData_ReturnsCreated()
    {
        // Arrange
        var newConfig = new AlertSchedulerConfig
        {
            SchedulerGroup = "TestGroup",
            SchedulerName = "Test Scheduler",
            CronExp = "0 */5 * * * ?",
            ElasticConfigId = 1,
            ElasticQueryId = 1,
            EmailConnectorId = 1,
            MSTeamsConnectorId = 1,
            EmailAlertDetailId = 1,
            NotificationTemplateId = 1,
            IsEnabled = true
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/scheduler/configs", newConfig);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var created = await response.Content.ReadFromJsonAsync<AlertSchedulerConfig>();
        created.Should().NotBeNull();
        created!.SchedulerName.Should().Be("Test Scheduler");
        created.AlertSchedulerConfigId.Should().BeGreaterThan((short)0);
    }

    [Fact]
    public async Task GetSchedulerConfigById_WithValidId_ReturnsOk()
    {
        // Arrange
        var newConfig = new AlertSchedulerConfig
        {
            SchedulerGroup = "GetTestGroup",
            SchedulerName = "Config for Get Test",
            CronExp = "0 0 * * * ?",
            ElasticConfigId = 1,
            ElasticQueryId = 1,
            EmailConnectorId = 1,
            MSTeamsConnectorId = 1,
            EmailAlertDetailId = 1,
            NotificationTemplateId = 1,
            IsEnabled = true
        };
        var createResponse = await _client.PostAsJsonAsync("/api/scheduler/configs", newConfig);
        var created = await createResponse.Content.ReadFromJsonAsync<AlertSchedulerConfig>();

        // Act
        var response = await _client.GetAsync($"/api/scheduler/configs/{created!.AlertSchedulerConfigId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var config = await response.Content.ReadFromJsonAsync<AlertSchedulerConfig>();
        config.Should().NotBeNull();
        config!.SchedulerName.Should().Be("Config for Get Test");
    }

    [Fact]
    public async Task UpdateSchedulerConfig_WithValidData_ReturnsOk()
    {
        // Arrange
        var newConfig = new AlertSchedulerConfig
        {
            SchedulerGroup = "UpdateGroup",
            SchedulerName = "Original Name",
            CronExp = "0 0 * * * ?",
            ElasticConfigId = 1,
            ElasticQueryId = 1,
            EmailConnectorId = 1,
            MSTeamsConnectorId = 1,
            EmailAlertDetailId = 1,
            NotificationTemplateId = 1,
            IsEnabled = true
        };
        var createResponse = await _client.PostAsJsonAsync("/api/scheduler/configs", newConfig);
        var created = await createResponse.Content.ReadFromJsonAsync<AlertSchedulerConfig>();
        
        created!.SchedulerName = "Updated Name";
        created.CronExp = "0 */10 * * * ?";

        // Act
        var response = await _client.PutAsJsonAsync($"/api/scheduler/configs/{created.AlertSchedulerConfigId}", created);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var updated = await response.Content.ReadFromJsonAsync<AlertSchedulerConfig>();
        updated!.SchedulerName.Should().Be("Updated Name");
        updated.CronExp.Should().Be("0 */10 * * * ?");
    }

    [Fact]
    public async Task DeleteSchedulerConfig_WithValidId_ReturnsNoContent()
    {
        // Arrange
        var newConfig = new AlertSchedulerConfig
        {
            SchedulerGroup = "DeleteGroup",
            SchedulerName = "Config to Delete",
            CronExp = "0 0 * * * ?",
            ElasticConfigId = 1,
            ElasticQueryId = 1,
            EmailConnectorId = 1,
            MSTeamsConnectorId = 1,
            EmailAlertDetailId = 1,
            NotificationTemplateId = 1,
            IsEnabled = true
        };
        var createResponse = await _client.PostAsJsonAsync("/api/scheduler/configs", newConfig);
        var created = await createResponse.Content.ReadFromJsonAsync<AlertSchedulerConfig>();

        // Act
        var response = await _client.DeleteAsync($"/api/scheduler/configs/{created!.AlertSchedulerConfigId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        
        // Verify deletion
        var getResponse = await _client.GetAsync($"/api/scheduler/configs/{created.AlertSchedulerConfigId}");
        getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetSchedulerConfigById_WithInvalidId_ReturnsNotFound()
    {
        // Act
        var response = await _client.GetAsync("/api/scheduler/configs/99999");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
