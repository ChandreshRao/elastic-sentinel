using Microsoft.AspNetCore.Mvc.RazorPages;
using ElasticSentinel.Domain.Entities;
using ElasticSentinel.Infrastructure.Services;

namespace ElasticSentinel.Pages.ElasticsearchSettings
{
    public class IndexModel : PageModel
    {
        private readonly ApiClientService _apiClient;

        public IndexModel(ApiClientService apiClient)
        {
            _apiClient = apiClient;
        }

        public IList<ElasticConfiguration> ElasticConfiguration { get;set; } = default!;

        public async Task OnGetAsync()
        {
            ElasticConfiguration = await _apiClient.GetAsync<List<ElasticConfiguration>>("/api/elastic-configurations") ?? new List<ElasticConfiguration>();
        }
    }
}
