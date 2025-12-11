using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ElasticSentinel.Domain.Entities;
using ElasticSentinel.Infrastructure.Services;

namespace ElasticSentinel.Pages.Connectors.MailConnector
{
    public class EditModel : PageModel
    {
        private readonly ApiClientService _apiClient;

        public EditModel(ApiClientService apiClient)
        {
            _apiClient = apiClient;
        }

        [BindProperty]
        public EmailConnector EmailConnector { get; set; } = default!;

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

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var result = await _apiClient.PutAsync<EmailConnector>($"/api/connectors/email/{EmailConnector.EmailConnectorId}", EmailConnector);
            if (result == null)
            {
                ModelState.AddModelError(string.Empty, "Failed to update email connector.");
                return Page();
            }

            return RedirectToPage("./Index");
        }
    }
}
