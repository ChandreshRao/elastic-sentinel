using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ElasticSentinel.Domain.Entities;
using ElasticSentinel.Infrastructure.Services;

namespace ElasticSentinel.Pages.Connectors.TeamsConnector
{
    public class EditModel : PageModel
    {
        private readonly ApiClientService _apiClient;

        public EditModel(ApiClientService apiClient)
        {
            _apiClient = apiClient;
        }

        [BindProperty]
        public MSTeamsConnector MSTeamsConnector { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var msteamsconnector = await _apiClient.GetAsync<MSTeamsConnector>($"/api/connectors/teams/{id}");
            if (msteamsconnector == null)
            {
                return NotFound();
            }
            MSTeamsConnector = msteamsconnector;
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

            var result = await _apiClient.PutAsync<MSTeamsConnector>($"/api/connectors/teams/{MSTeamsConnector.MSTeamsConnectorId}", MSTeamsConnector);
            if (result == null)
            {
                ModelState.AddModelError(string.Empty, "Failed to update Teams connector.");
                return Page();
            }

            return RedirectToPage("./Index");
        }
    }
}
