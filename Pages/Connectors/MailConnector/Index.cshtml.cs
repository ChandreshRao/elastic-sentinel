using Microsoft.AspNetCore.Mvc.RazorPages;
using ElasticSentinel.Domain.Entities;
using ElasticSentinel.Infrastructure.Services;

namespace ElasticSentinel.Pages.Connectors.MailConnector
{
    public class IndexModel : PageModel
    {
        private readonly ApiClientService _apiClient;

        public IndexModel(ApiClientService apiClient)
        {
            _apiClient = apiClient;
        }

        public IList<EmailConnector> EmailConnector { get;set; } = default!;

        public async Task OnGetAsync()
        {
            EmailConnector = await _apiClient.GetAsync<List<EmailConnector>>("/api/connectors/email") ?? new List<EmailConnector>();
        }
    }
}
