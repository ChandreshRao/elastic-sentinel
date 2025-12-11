using ElasticSentinel.Domain.Entities;
using ElasticSentinel.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ElasticSentinel.Pages.Scheduler
{
    public class IndexModel : PageModel
    {
        private readonly ApiClientService _apiClient;

        public IndexModel(ApiClientService apiClient)
        {
            _apiClient = apiClient;
        }

        public IList<AlertSchedulerConfig> AlertSchedulerConfig { get;set; } = default!;

        public async Task OnGetAsync()
        {
            AlertSchedulerConfig = await _apiClient.GetAsync<List<AlertSchedulerConfig>>("/api/scheduler/configs") ?? new List<AlertSchedulerConfig>();
        }
    }
}
