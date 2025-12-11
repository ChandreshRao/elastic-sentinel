using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ElasticSentinel.Domain.Entities;
using ElasticSentinel.Infrastructure.Services;

namespace ElasticSentinel.Pages.Connectors.MailConnector
{
    public class CreateModel : PageModel
    {
        private readonly ApiClientService _apiClient;

        public CreateModel(ApiClientService apiClient)
        {
            _apiClient = apiClient;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public required EmailConnector EmailConnector { get; set; }
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var result = await _apiClient.PostAsync<EmailConnector>("/api/connectors/email", EmailConnector);
            if (result == null)
            {
                ModelState.AddModelError(string.Empty, "Failed to create email connector.");
                return Page();
            }

            return RedirectToPage("./Index");
        }
    }
}
