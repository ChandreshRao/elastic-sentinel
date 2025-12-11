using Microsoft.AspNetCore.Mvc.RazorPages;
using ElasticSentinel.Domain.Entities;
using ElasticSentinel.Infrastructure.Services;

namespace ElasticSentinel.Pages.Connectors.TeamsConnector
{
    public class IndexModel : PageModel
    {
        private readonly ApiClientService _apiClient;

        public IndexModel(ApiClientService apiClient)
        {
            _apiClient = apiClient;
        }

        public IList<MSTeamsConnector> MSTeamsConnector { get;set; } = default!;

        public async Task OnGetAsync()
        {
            MSTeamsConnector = await _apiClient.GetAsync<List<MSTeamsConnector>>("/api/connectors/teams") ?? new List<MSTeamsConnector>();
        }
    }
}
