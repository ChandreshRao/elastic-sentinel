using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ElasticSentinel.Domain.Entities;
using ElasticSentinel.Infrastructure.Services;

namespace ElasticSentinel.Pages.Connectors.MailConnectorDetail
{
    public class DetailsModel : PageModel
    {
        private readonly ApiClientService _apiClient;

        public DetailsModel(ApiClientService apiClient)
        {
            _apiClient = apiClient;
        }

        public EmailConnectorDetail EmailConnectorDetail { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var emailconnectordetail = await _apiClient.GetAsync<EmailConnectorDetail>($"/api/connectors/email-details/{id}");
            if (emailconnectordetail == null)
            {
                return NotFound();
            }
            
            EmailConnectorDetail = emailconnectordetail;
            return Page();
        }
    }
}
