using ElasticSentinel.Application.Features.Notifications;
using FluentAssertions;
using Moq;
using Xunit;

namespace ElasticSentinel.Tests.Application.Features.Notifications;

public class RenderNotificationHandlerTests
{
    [Fact]
    public async Task HandleAsync_WithValidDataRequest_ShouldRenderTemplate()
    {
        // Arrange
        var handler = new RenderNotificationHandler();
        var data = new Dictionary<string, string>
        {
            { "Name", "John Doe" },
            { "Status", "Active" }
        };
        var template = "Hello {{ Name }}, your status is {{ Status }}";
        var request = new RenderNotificationWithDataRequest(template, data);

        // Act
        var result = await handler.HandleAsync(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNullOrWhiteSpace();
        result.Should().Contain("John Doe");
        result.Should().Contain("Active");
    }

    [Fact]
    public async Task HandleAsync_WithListRequest_ShouldRenderTemplateForAllItems()
    {
        // Arrange
        var handler = new RenderNotificationHandler();
        var dataList = new List<Dictionary<string, string>>
        {
            new() { { "Name", "Item1" }, { "Value", "100" } },
            new() { { "Name", "Item2" }, { "Value", "200" } }
        };
        var template = "{{ for item in items }}{{ item.Name }}: {{ item.Value }}\n{{ end }}";
        var request = new RenderNotificationWithListRequest(template, dataList);

        // Act
        var result = await handler.HandleAsync(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNullOrWhiteSpace();
        result.Should().Contain("Item1");
        result.Should().Contain("Item2");
        result.Should().Contain("100");
        result.Should().Contain("200");
    }

    [Fact]
    public async Task HandleAsync_WithEmptyTemplate_ShouldReturnEmptyString()
    {
        // Arrange
        var handler = new RenderNotificationHandler();
        var data = new Dictionary<string, string> { { "Key", "Value" } };
        var request = new RenderNotificationWithDataRequest(string.Empty, data);

        // Act
        var result = await handler.HandleAsync(request, CancellationToken.None);

        // Assert
        result.Should().BeEmpty();
    }
}
