using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ElasticSentinel.Domain.Entities;
using ElasticSentinel.Infrastructure.Services;

namespace ElasticSentinel.Pages.Connectors.MailConnectorDetail
{
    public class EditModel : PageModel
    {
        private readonly ApiClientService _apiClient;

        public EditModel(ApiClientService apiClient)
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

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var result = await _apiClient.PutAsync<EmailConnectorDetail>($"/api/connectors/email-details/{EmailConnectorDetail.EmailAlertDetailId}", EmailConnectorDetail);
            if (result == null)
            {
                ModelState.AddModelError(string.Empty, "Failed to update email connector detail.");
                return Page();
            }

            return RedirectToPage("./Index");
        }
    }
}
