using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ElasticSentinel.Domain.Entities;
using ElasticSentinel.Infrastructure.Services;

namespace ElasticSentinel.Pages.Connectors.MailConnector
{
    public class DeleteModel : PageModel
    {
        private readonly ApiClientService _apiClient;

        public DeleteModel(ApiClientService apiClient)
        {
            _apiClient = apiClient;
        }

        [BindProperty]
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

        public async Task<IActionResult> OnPostAsync(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var success = await _apiClient.DeleteAsync($"/api/connectors/email/{id}");
            if (!success)
            {
                ModelState.AddModelError(string.Empty, "Failed to delete email connector.");
                return Page();
            }

            return RedirectToPage("./Index");
        }
    }
}
