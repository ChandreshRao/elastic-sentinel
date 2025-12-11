using System.Net;
using System.Net.Http.Json;
using ElasticSentinel.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace ElasticSentinel.Tests.API.Endpoints;

[Collection("API Tests")]
public class ElasticConfigurationsEndpointsTests
{
    private readonly HttpClient _client;
    private readonly TestWebApplicationFactory _factory;

    public ElasticConfigurationsEndpointsTests(TestWebApplicationFactory factory)
    {
        _factory = factory;
        _factory.SeedDatabase(); // Seed data before each test class
        _client = factory.CreateAuthenticatedClient();
    }

    [Fact]
    public async Task GetAllConfigurations_ReturnsOk()
    {
        // Act
        var response = await _client.GetAsync("/api/elastic-configurations");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var configurations = await response.Content.ReadFromJsonAsync<List<ElasticConfiguration>>();
        configurations.Should().NotBeNull();
    }

    [Fact]
    public async Task CreateConfiguration_WithValidData_ReturnsCreated()
    {
        // Arrange
        var newConfig = new ElasticConfiguration
        {
            ClusterName = "Test Cluster",
            ElasticHost = "https://localhost:9200",
            UserName = "elastic",
            Password = "password123",
            IsEnabled = true
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/elastic-configurations", newConfig);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var created = await response.Content.ReadFromJsonAsync<ElasticConfiguration>();
        created.Should().NotBeNull();
        created!.ClusterName.Should().Be("Test Cluster");
        created.ElasticConfigId.Should().BeGreaterThan((short)0);
    }

    [Fact]
    public async Task GetConfigurationById_WithValidId_ReturnsOk()
    {
        // Arrange
        var newConfig = new ElasticConfiguration
        {
            ClusterName = "Config for Get Test",
            ElasticHost = "https://localhost:9200",
            UserName = "elastic",
            Password = "password",
            IsEnabled = true
        };
        var createResponse = await _client.PostAsJsonAsync("/api/elastic-configurations", newConfig);
        var created = await createResponse.Content.ReadFromJsonAsync<ElasticConfiguration>();

        // Act
        var response = await _client.GetAsync($"/api/elastic-configurations/{created!.ElasticConfigId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var config = await response.Content.ReadFromJsonAsync<ElasticConfiguration>();
        config.Should().NotBeNull();
        config!.ClusterName.Should().Be("Config for Get Test");
    }

    [Fact]
    public async Task GetConfigurationById_WithInvalidId_ReturnsNotFound()
    {
        // Act
        var response = await _client.GetAsync("/api/elastic-configurations/99999");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task UpdateConfiguration_WithValidData_ReturnsOk()
    {
        // Arrange
        var newConfig = new ElasticConfiguration
        {
            ClusterName = "Original Cluster",
            ElasticHost = "https://localhost:9200",
            UserName = "elastic",
            Password = "password",
            IsEnabled = true
        };
        var createResponse = await _client.PostAsJsonAsync("/api/elastic-configurations", newConfig);
        var created = await createResponse.Content.ReadFromJsonAsync<ElasticConfiguration>();
        
        created!.ClusterName = "Updated Cluster";
        created.ElasticHost = "https://updated-host:9200";

        // Act
        var response = await _client.PutAsJsonAsync($"/api/elastic-configurations/{created.ElasticConfigId}", created);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var updated = await response.Content.ReadFromJsonAsync<ElasticConfiguration>();
        updated!.ClusterName.Should().Be("Updated Cluster");
        updated.ElasticHost.Should().Be("https://updated-host:9200");
    }

    [Fact]
    public async Task DeleteConfiguration_WithValidId_ReturnsNoContent()
    {
        // Arrange
        var newConfig = new ElasticConfiguration
        {
            ClusterName = "Config to Delete",
            ElasticHost = "https://localhost:9200",
            UserName = "elastic",
            Password = "password",
            IsEnabled = true
        };
        var createResponse = await _client.PostAsJsonAsync("/api/elastic-configurations", newConfig);
        var created = await createResponse.Content.ReadFromJsonAsync<ElasticConfiguration>();

        // Act
        var response = await _client.DeleteAsync($"/api/elastic-configurations/{created!.ElasticConfigId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        
        // Verify deletion
        var getResponse = await _client.GetAsync($"/api/elastic-configurations/{created.ElasticConfigId}");
        getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task UpdateConfiguration_WithInvalidId_ReturnsNotFound()
    {
        // Arrange
        var config = new ElasticConfiguration
        {
            ElasticConfigId = 999,
            ClusterName = "Non-existent",
            ElasticHost = "https://localhost:9200",
            UserName = "elastic",
            Password = "password",
            IsEnabled = true
        };

        // Act
        var response = await _client.PutAsJsonAsync("/api/elastic-configurations/999", config);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task CreateConfiguration_WithCertificateThumbprint_ReturnsCreated()
    {
        // Arrange
        var newConfig = new ElasticConfiguration
        {
            ClusterName = "Secure Cluster",
            ElasticHost = "https://secure-host:9200",
            UserName = "elastic",
            Password = "password",
            CertificateThumbprint = "ABCDEF1234567890",
            IsEnabled = true
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/elastic-configurations", newConfig);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var created = await response.Content.ReadFromJsonAsync<ElasticConfiguration>();
        created.Should().NotBeNull();
        created!.CertificateThumbprint.Should().Be("ABCDEF1234567890");
    }
}
