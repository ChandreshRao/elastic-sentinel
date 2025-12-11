using Microsoft.AspNetCore.Mvc.RazorPages;
using ElasticSentinel.Domain.Entities;
using ElasticSentinel.Infrastructure.Services;

namespace ElasticSentinel.Pages.AlertMessageTemplates
{
    public class IndexModel : PageModel
    {
        private readonly ApiClientService _apiClient;

        public IndexModel(ApiClientService apiClient)
        {
            _apiClient = apiClient;
        }

        public IList<NotificationTemplate> NotificationTemplate { get;set; } = default!;

        public async Task OnGetAsync()
        {
            NotificationTemplate = await _apiClient.GetAsync<List<NotificationTemplate>>("/api/templates") ?? new List<NotificationTemplate>();
        }
    }
}
