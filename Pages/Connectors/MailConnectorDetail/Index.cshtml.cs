using Microsoft.AspNetCore.Mvc.RazorPages;
using ElasticSentinel.Domain.Entities;
using ElasticSentinel.Infrastructure.Services;

namespace ElasticSentinel.Pages.Connectors.MailConnectorDetail
{
    public class IndexModel : PageModel
    {
        private readonly ApiClientService _apiClient;

        public IndexModel(ApiClientService apiClient)
        {
            _apiClient = apiClient;
        }

        public IList<EmailConnectorDetail> EmailConnectorDetail { get;set; } = default!;

        public async Task OnGetAsync()
        {
            EmailConnectorDetail = await _apiClient.GetAsync<List<EmailConnectorDetail>>("/api/connectors/email-details") ?? new List<EmailConnectorDetail>();
        }
    }
}
