using Microsoft.AspNetCore.Mvc.RazorPages;
using ElasticSentinel.Domain.Entities;
using ElasticSentinel.Infrastructure.Services;

namespace ElasticSentinel.Pages.Queries
{
    public class IndexModel : PageModel
    {
        private readonly ApiClientService _apiClient;

        public IndexModel(ApiClientService apiClient)
        {
            _apiClient = apiClient;
        }

        public IList<ElasticQuery> ElasticQuery { get;set; } = default!;

        public async Task OnGetAsync()
        {
            ElasticQuery = await _apiClient.GetAsync<List<ElasticQuery>>("/api/queries") ?? new List<ElasticQuery>();
        }
    }
}
