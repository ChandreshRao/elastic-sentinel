using System.Net;
using System.Net.Http.Json;
using ElasticSentinel.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace ElasticSentinel.Tests.API.Endpoints;

[Collection("API Tests")]
public class ConnectorsEndpointsTests
{
    private readonly HttpClient _client;
    private readonly TestWebApplicationFactory _factory;

    public ConnectorsEndpointsTests(TestWebApplicationFactory factory)
    {
        _factory = factory;
        _factory.SeedDatabase(); // Seed data before each test class
        _client = factory.CreateAuthenticatedClient();
    }

    #region Email Connector Tests

    [Fact]
    public async Task GetAllEmailConnectors_ReturnsOk()
    {
        // Act
        var response = await _client.GetAsync("/api/connectors/email");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var connectors = await response.Content.ReadFromJsonAsync<List<EmailConnector>>();
        connectors.Should().NotBeNull();
    }

    [Fact]
    public async Task CreateEmailConnector_WithValidData_ReturnsCreated()
    {
        // Arrange
        var newConnector = new EmailConnector
        {
            Name = "Test Email Connector",
            FromEmail = "test@test.com",
            PrimarySMTPServer = "smtp.test.com",
            SMTPPort = 587,
            Username = "test@test.com",
            Password = "password123",
            IsEnabled = true
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/connectors/email", newConnector);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var created = await response.Content.ReadFromJsonAsync<EmailConnector>();
        created.Should().NotBeNull();
        created!.Name.Should().Be("Test Email Connector");
    }

    [Fact]
    public async Task UpdateEmailConnector_WithValidData_ReturnsOk()
    {
        // Arrange
        var newConnector = new EmailConnector
        {
            Name = "Original Email",
            FromEmail = "test@test.com",
            PrimarySMTPServer = "smtp.test.com",
            SMTPPort = 587,
            Username = "test@test.com",
            Password = "password",
            IsEnabled = true
        };
        var createResponse = await _client.PostAsJsonAsync("/api/connectors/email", newConnector);
        var created = await createResponse.Content.ReadFromJsonAsync<EmailConnector>();
        
        created!.Name = "Updated Email";

        // Act
        var response = await _client.PutAsJsonAsync($"/api/connectors/email/{created.EmailConnectorId}", created);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var updated = await response.Content.ReadFromJsonAsync<EmailConnector>();
        updated!.Name.Should().Be("Updated Email");
    }

    [Fact]
    public async Task DeleteEmailConnector_WithValidId_ReturnsNoContent()
    {
        // Arrange
        var newConnector = new EmailConnector
        {
            Name = "Connector for Get Test",
            FromEmail = "test@test.com",
            PrimarySMTPServer = "smtp.test.com",
            SMTPPort = 587,
            Username = "test@test.com",
            Password = "password123",
            IsEnabled = true
        };
        var createResponse = await _client.PostAsJsonAsync("/api/connectors/email", newConnector);
        var created = await createResponse.Content.ReadFromJsonAsync<EmailConnector>();

        // Act
        var response = await _client.DeleteAsync($"/api/connectors/email/{created!.EmailConnectorId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    #endregion

    #region Teams Connector Tests

    [Fact]
    public async Task GetAllTeamsConnectors_ReturnsOk()
    {
        // Act
        var response = await _client.GetAsync("/api/connectors/teams");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var connectors = await response.Content.ReadFromJsonAsync<List<MSTeamsConnector>>();
        connectors.Should().NotBeNull();
    }

    [Fact]
    public async Task CreateTeamsConnector_WithValidData_ReturnsCreated()
    {
        // Arrange
        var newConnector = new MSTeamsConnector
        {
            Name = "Test Teams Connector",
            WebhookUrl = "https://outlook.office.com/webhook/test",
            IsEnabled = true
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/connectors/teams", newConnector);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var created = await response.Content.ReadFromJsonAsync<MSTeamsConnector>();
        created.Should().NotBeNull();
        created!.Name.Should().Be("Test Teams Connector");
    }

    #endregion

    #region Email Connector Details Tests

    [Fact]
    public async Task GetAllEmailConnectorDetails_ReturnsOk()
    {
        // Act
        var response = await _client.GetAsync("/api/connectors/email-details");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var details = await response.Content.ReadFromJsonAsync<List<EmailConnectorDetail>>();
        details.Should().NotBeNull();
    }

    [Fact]
    public async Task CreateEmailConnectorDetail_WithValidData_ReturnsCreated()
    {
        // Arrange
        var newDetail = new EmailConnectorDetail
        {
            Name = "Test Email Detail",
            ToEmails = "to@test.com",
            EmailSubject = "Test Subject",
            CcEmails = "cc@test.com"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/connectors/email-details", newDetail);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var created = await response.Content.ReadFromJsonAsync<EmailConnectorDetail>();
        created.Should().NotBeNull();
        created!.Name.Should().Be("Test Email Detail");
    }

    #endregion
}
