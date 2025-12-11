using System.Net;
using System.Net.Http.Json;
using ElasticSentinel.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace ElasticSentinel.Tests.API.Endpoints;

[Collection("API Tests")]
public class ElasticQueriesEndpointsTests
{
    private readonly HttpClient _client;
    private readonly TestWebApplicationFactory _factory;

    public ElasticQueriesEndpointsTests(TestWebApplicationFactory factory)
    {
        _factory = factory;
        _factory.SeedDatabase(); // Seed data before each test class
        _client = factory.CreateAuthenticatedClient();
    }

    [Fact]
    public async Task GetAllQueries_WithoutApiKey_ReturnsUnauthorized()
    {
        // Arrange
        var unauthClient = _factory.CreateClient();

        // Act
        var response = await unauthClient.GetAsync("/api/queries");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task GetAllQueries_ReturnsOk()
    {
        // Act
        var response = await _client.GetAsync("/api/queries");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var queries = await response.Content.ReadFromJsonAsync<List<ElasticQuery>>();
        queries.Should().NotBeNull();
    }

    [Fact]
    public async Task CreateQuery_WithValidData_ReturnsCreated()
    {
        // Arrange
        var newQuery = new ElasticQuery
        {
            QueryName = "Test Query",
            QueryDescription = "Test Description",
            IsDynamic = false,
            ElasticDynamicQueryDetailId = 1,
            ElasticDynamicQueryResponseDetailId = 1
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/queries", newQuery);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var createdQuery = await response.Content.ReadFromJsonAsync<ElasticQuery>();
        createdQuery.Should().NotBeNull();
        createdQuery!.QueryName.Should().Be("Test Query");
        createdQuery.ElasticQueryId.Should().BeGreaterThan((short)0);
    }

    [Fact]
    public async Task GetQueryById_WithValidId_ReturnsOk()
    {
        // Arrange
        var newQuery = new ElasticQuery
        {
            QueryName = "Query for Get Test",
            QueryDescription = "Test Description",
            IsDynamic = false,
            ElasticDynamicQueryDetailId = 1,
            ElasticDynamicQueryResponseDetailId = 1
        };
        var createResponse = await _client.PostAsJsonAsync("/api/queries", newQuery);
        var createdQuery = await createResponse.Content.ReadFromJsonAsync<ElasticQuery>();

        // Act
        var response = await _client.GetAsync($"/api/queries/{createdQuery!.ElasticQueryId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var query = await response.Content.ReadFromJsonAsync<ElasticQuery>();
        query.Should().NotBeNull();
        query!.QueryName.Should().Be("Query for Get Test");
    }

    [Fact]
    public async Task GetQueryById_WithInvalidId_ReturnsNotFound()
    {
        // Act
        var response = await _client.GetAsync("/api/queries/99999");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task UpdateQuery_WithValidData_ReturnsOk()
    {
        // Arrange
        var newQuery = new ElasticQuery
        {
            QueryName = "Original Name",
            QueryDescription = "Description",
            IsDynamic = false,
            ElasticDynamicQueryDetailId = 1,
            ElasticDynamicQueryResponseDetailId = 1
        };
        var createResponse = await _client.PostAsJsonAsync("/api/queries", newQuery);
        var createdQuery = await createResponse.Content.ReadFromJsonAsync<ElasticQuery>();
        
        createdQuery!.QueryName = "Updated Name";

        // Act
        var response = await _client.PutAsJsonAsync($"/api/queries/{createdQuery.ElasticQueryId}", createdQuery);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var updatedQuery = await response.Content.ReadFromJsonAsync<ElasticQuery>();
        updatedQuery!.QueryName.Should().Be("Updated Name");
    }

    [Fact]
    public async Task DeleteQuery_WithValidId_ReturnsNoContent()
    {
        // Arrange
        var newQuery = new ElasticQuery
        {
            QueryName = "Query to Delete",
            QueryDescription = "Description",
            IsDynamic = false,
            ElasticDynamicQueryDetailId = 1,
            ElasticDynamicQueryResponseDetailId = 1
        };
        var createResponse = await _client.PostAsJsonAsync("/api/queries", newQuery);
        var createdQuery = await createResponse.Content.ReadFromJsonAsync<ElasticQuery>();

        // Act
        var response = await _client.DeleteAsync($"/api/queries/{createdQuery!.ElasticQueryId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        
        // Verify deletion
        var getResponse = await _client.GetAsync($"/api/queries/{createdQuery.ElasticQueryId}");
        getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
