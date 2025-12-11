using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ElasticSentinel.Domain.Entities;
using ElasticSentinel.Infrastructure.Services;

namespace ElasticSentinel.Pages.Connectors.MailConnector
{
    public class DetailsModel : PageModel
    {
        private readonly ApiClientService _apiClient;

        public DetailsModel(ApiClientService apiClient)
        {
            _apiClient = apiClient;
        }

        public required EmailConnector EmailConnector { get; set; }

        public async Task<IActionResult> OnGetAsync(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var emailconnector = await _apiClient.GetAsync<EmailConnector>($"/api/connectors/email/{id}");
            if (emailconnector == null)
            {
                return NotFound();
            }
            
            EmailConnector = emailconnector;
            return Page();
        }
    }
}
