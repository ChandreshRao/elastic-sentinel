using ElasticSentinel.Application.Common.Abstractions;
using ElasticSentinel.Infrastructure.Persistence;

namespace ElasticSentinel.Application.Features.Documents;

/// <summary>
/// Handler for processing pending documents from the database.
/// This is a command handler as it modifies document state.
/// </summary>
public interface IProcessDocumentsHandler : ICommandHandler<ProcessDocumentsRequest>
{
}

/// <summary>
/// Request to process pending documents
/// </summary>
/// <param name="DbContext">The database context to query and update documents</param>
public record ProcessDocumentsRequest(SentinelDbContext DbContext);
