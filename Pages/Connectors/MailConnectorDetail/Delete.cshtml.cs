using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ElasticSentinel.Domain.Entities;
using ElasticSentinel.Infrastructure.Services;

namespace ElasticSentinel.Pages.Connectors.MailConnectorDetail
{
    public class DeleteModel : PageModel
    {
        private readonly ApiClientService _apiClient;

        public DeleteModel(ApiClientService apiClient)
        {
            _apiClient = apiClient;
        }

        [BindProperty]
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

        public async Task<IActionResult> OnPostAsync(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var success = await _apiClient.DeleteAsync($"/api/connectors/email-details/{id}");
            if (!success)
            {
                ModelState.AddModelError(string.Empty, "Failed to delete email connector detail.");
                return Page();
            }

            return RedirectToPage("./Index");
        }
    }
}
