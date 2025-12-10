using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ElasticSentinel.Application.Common.Abstractions;

/// <summary>
/// Implementation of IEventPublisher that resolves all handlers for an event
/// and executes them in parallel using Task.WhenAll.
/// </summary>
public sealed class EventPublisher : IEventPublisher
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<EventPublisher> _logger;

    public EventPublisher(IServiceProvider serviceProvider, ILogger<EventPublisher> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default) where TEvent : IEvent
    {
        var eventType = @event.GetType();
        _logger.LogDebug("Publishing event {EventType} that occurred at {OccurredOn}", eventType.Name, @event.OccurredOn);

        try
        {
            // Resolve all handlers for this event type
            var handlers = _serviceProvider.GetServices<IEventHandler<TEvent>>().ToArray();
            
            if (handlers.Length == 0)
            {
                _logger.LogDebug("No handlers registered for event {EventType}", eventType.Name);
                return;
            }

            _logger.LogInformation("Found {HandlerCount} handler(s) for event {EventType}", handlers.Length, eventType.Name);

            // Execute all handlers in parallel
            var handlerTasks = handlers.Select(handler =>
                ExecuteHandlerAsync(handler, @event, cancellationToken)
            ).ToList();

            await Task.WhenAll(handlerTasks);

            _logger.LogInformation("Successfully published event {EventType} to {HandlerCount} handler(s)", eventType.Name, handlers.Length);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error publishing event {EventType}", eventType.Name);
            throw;
        }
    }

    private async Task ExecuteHandlerAsync<TEvent>(
        IEventHandler<TEvent> handler,
        TEvent @event,
        CancellationToken cancellationToken) where TEvent : IEvent
    {
        var handlerType = handler.GetType();
        
        try
        {
            _logger.LogDebug("Executing handler {HandlerType} for event {EventType}", handlerType.Name, typeof(TEvent).Name);
            await handler.HandleAsync(@event, cancellationToken);
            _logger.LogDebug("Handler {HandlerType} completed successfully", handlerType.Name);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error executing handler {HandlerType} for event {EventType}", handlerType.Name, typeof(TEvent).Name);
            throw;
        }
    }
}
