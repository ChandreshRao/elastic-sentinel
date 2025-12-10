namespace ElasticSentinel.Application.Common.Abstractions;

/// <summary>
/// Marker interface for all domain events in the application.
/// Events represent something that has already happened.
/// </summary>
public interface IEvent
{
    /// <summary>
    /// When the event occurred
    /// </summary>
    DateTime OccurredOn { get; }
}

/// <summary>
/// Base record for domain events with automatic timestamp
/// </summary>
public abstract record DomainEvent : IEvent
{
    public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
}
