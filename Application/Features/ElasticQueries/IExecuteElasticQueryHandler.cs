using ElasticSentinel.Application.Common.Abstractions;
using ElasticSentinel.Application.Common.Models;

namespace ElasticSentinel.Application.Features.ElasticQueries;

/// <summary>
/// Handler for executing Elasticsearch queries.
/// This is a query handler as it retrieves data without modifying state.
/// </summary>
public interface IExecuteElasticQueryHandler : IQueryHandler<ExecuteElasticQueryRequest, List<Dictionary<string, string>>?>
{
}

/// <summary>
/// Request to execute an Elasticsearch query
/// </summary>
public record ExecuteElasticQueryRequest(
    ElasticQueryAPIRequest ApiRequest,
    Dictionary<string, Dictionary<string, string>> ResponseMap,
    ILogger Logger
);
