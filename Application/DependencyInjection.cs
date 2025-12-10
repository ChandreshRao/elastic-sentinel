using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using ElasticSentinel.Application.Common.Abstractions;
using FluentValidation;

namespace ElasticSentinel.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Register EventPublisher
        services.AddSingleton<IEventPublisher, EventPublisher>();
        
        // Auto-register all handlers and event handlers from Application assembly
        services.RegisterHandlersFromAssemblyContaining<IHandler>();
        
        // Register FluentValidation validators
        services.AddValidatorsFromAssemblyContaining<IHandler>();
        
        return services;
    }
    
    /// <summary>
    /// Registers all handlers from the assembly containing the specified type.
    /// Scans for implementations of IHandler and IEventHandler and registers them with their interfaces.
    /// </summary>
    /// <param name="services">The service collection</param>
    /// <param name="marker">A type from the assembly where handlers are located</param>
    /// <returns>The service collection for chaining</returns>
    private static IServiceCollection RegisterHandlersFromAssemblyContaining<TMarker>(
        this IServiceCollection services)
    {
        var assembly = typeof(TMarker).Assembly;
        
        // Register command and query handlers
        RegisterCommandAndQueryHandlers(services, assembly);
        
        // Register event handlers
        RegisterEventHandlers(services, assembly);
        
        return services;
    }
    
    private static void RegisterCommandAndQueryHandlers(IServiceCollection services, Assembly assembly)
    {
        var handlerTypes = assembly.GetTypes()
            .Where(t => t is { IsClass: true, IsAbstract: false } 
                        && t.IsAssignableTo(typeof(IHandler))
                        && !t.IsAssignableTo(typeof(IEventHandler))) // Exclude event handlers
            .ToList();

        foreach (var implementationType in handlerTypes)
        {
            var interfaceType = implementationType.GetInterfaces()
                .FirstOrDefault(i => i != typeof(IHandler) 
                                     && i.IsAssignableTo(typeof(IHandler))
                                     && !i.IsAssignableTo(typeof(IEventHandler)));

            if (interfaceType is not null)
            {
                services.AddScoped(interfaceType, implementationType);
            }
        }
    }
    
    private static void RegisterEventHandlers(IServiceCollection services, Assembly assembly)
    {
        var eventHandlerTypes = assembly.GetTypes()
            .Where(t => t is { IsClass: true, IsAbstract: false } 
                        && t.GetInterfaces().Any(i => i.IsGenericType 
                                                      && i.GetGenericTypeDefinition() == typeof(IEventHandler<>)))
            .ToList();

        foreach (var implementationType in eventHandlerTypes)
        {
            var eventHandlerInterfaces = implementationType.GetInterfaces()
                .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEventHandler<>))
                .ToList();

            foreach (var interfaceType in eventHandlerInterfaces)
            {
                services.AddScoped(interfaceType, implementationType);
            }
        }
    }
}
