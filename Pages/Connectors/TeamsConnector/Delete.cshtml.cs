using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ElasticSentinel.Domain.Entities;
using ElasticSentinel.Infrastructure.Services;

namespace ElasticSentinel.Pages.Connectors.TeamsConnector
{
    public class DeleteModel : PageModel
    {
        private readonly ApiClientService _apiClient;

        public DeleteModel(ApiClientService apiClient)
        {
            _apiClient = apiClient;
        }

        [BindProperty]
        public required MSTeamsConnector MSTeamsConnector { get; set; }

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

        public async Task<IActionResult> OnPostAsync(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var success = await _apiClient.DeleteAsync($"/api/connectors/teams/{id}");
            if (!success)
            {
                ModelState.AddModelError(string.Empty, "Failed to delete Teams connector.");
                return Page();
            }

            return RedirectToPage("./Index");
        }
    }
}
