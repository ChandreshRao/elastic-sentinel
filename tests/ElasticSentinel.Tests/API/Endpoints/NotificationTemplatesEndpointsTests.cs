using System.Net;
using System.Net.Http.Json;
using ElasticSentinel.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace ElasticSentinel.Tests.API.Endpoints;

[Collection("API Tests")]
public class NotificationTemplatesEndpointsTests
{
    private readonly HttpClient _client;
    private readonly TestWebApplicationFactory _factory;

    public NotificationTemplatesEndpointsTests(TestWebApplicationFactory factory)
    {
        _factory = factory;
        _factory.SeedDatabase(); // Seed data before each test class
        _client = factory.CreateAuthenticatedClient();
    }

    [Fact]
    public async Task GetAllTemplates_ReturnsOk()
    {
        // Act
        var response = await _client.GetAsync("/api/templates");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var templates = await response.Content.ReadFromJsonAsync<List<NotificationTemplate>>();
        templates.Should().NotBeNull();
    }

    [Fact]
    public async Task CreateTemplate_WithValidData_ReturnsCreated()
    {
        // Arrange
        var newTemplate = new NotificationTemplate
        {
            TemplateName = "Test Template",
            TemplateContent = "Hello {{ name }}",
            IsEnabled = true
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/templates", newTemplate);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var created = await response.Content.ReadFromJsonAsync<NotificationTemplate>();
        created.Should().NotBeNull();
        created!.TemplateName.Should().Be("Test Template");
        created.NotificationTemplateId.Should().BeGreaterThan((short)0);
    }

    [Fact]
    public async Task GetTemplateById_WithValidId_ReturnsOk()
    {
        // Arrange
        var newTemplate = new NotificationTemplate
        {
            TemplateName = "Template for Get Test",
            TemplateContent = "Body {{ data }}",
            IsEnabled = true
        };
        var createResponse = await _client.PostAsJsonAsync("/api/templates", newTemplate);
        var created = await createResponse.Content.ReadFromJsonAsync<NotificationTemplate>();

        // Act
        var response = await _client.GetAsync($"/api/templates/{created!.NotificationTemplateId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var template = await response.Content.ReadFromJsonAsync<NotificationTemplate>();
        template.Should().NotBeNull();
        template!.TemplateName.Should().Be("Template for Get Test");
    }

    [Fact]
    public async Task GetTemplateById_WithInvalidId_ReturnsNotFound()
    {
        // Act
        var response = await _client.GetAsync("/api/templates/99999");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task UpdateTemplate_WithValidData_ReturnsOk()
    {
        // Arrange
        var newTemplate = new NotificationTemplate
        {
            TemplateName = "Original Template Name",
            TemplateContent = "Original body",
            IsEnabled = true
        };
        var createResponse = await _client.PostAsJsonAsync("/api/templates", newTemplate);
        var created = await createResponse.Content.ReadFromJsonAsync<NotificationTemplate>();
        
        created!.TemplateName = "Updated Template Name";
        created.TemplateContent = "Updated body {{ data }}";

        // Act
        var response = await _client.PutAsJsonAsync($"/api/templates/{created.NotificationTemplateId}", created);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var updated = await response.Content.ReadFromJsonAsync<NotificationTemplate>();
        updated!.TemplateName.Should().Be("Updated Template Name");
        updated.TemplateContent.Should().Be("Updated body {{ data }}");
    }

    [Fact]
    public async Task DeleteTemplate_WithValidId_ReturnsNoContent()
    {
        // Arrange
        var newTemplate = new NotificationTemplate
        {
            TemplateName = "Template to Delete",
            TemplateContent = "Body",
            IsEnabled = true
        };
        var createResponse = await _client.PostAsJsonAsync("/api/templates", newTemplate);
        var created = await createResponse.Content.ReadFromJsonAsync<NotificationTemplate>();

        // Act
        var response = await _client.DeleteAsync($"/api/templates/{created!.NotificationTemplateId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        
        // Verify deletion
        var getResponse = await _client.GetAsync($"/api/templates/{created.NotificationTemplateId}");
        getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task UpdateTemplate_WithInvalidId_ReturnsNotFound()
    {
        // Arrange
        var template = new NotificationTemplate
        {
            NotificationTemplateId = 999,
            TemplateName = "Non-existent",
            TemplateContent = "Body",
            IsEnabled = true
        };

        // Act
        var response = await _client.PutAsJsonAsync("/api/templates/999", template);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
