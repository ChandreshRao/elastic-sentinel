using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ElasticSentinel.Domain.Entities;
using ElasticSentinel.Infrastructure.Services;

namespace ElasticSentinel.Pages.Queries
{
    public class DeleteModel : PageModel
    {
        private readonly ApiClientService _apiClient;

        public DeleteModel(ApiClientService apiClient)
        {
            _apiClient = apiClient;
        }

        [BindProperty]
        public required ElasticQuery ElasticQuery { get; set; }

        public async Task<IActionResult> OnGetAsync(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var elasticquery = await _apiClient.GetAsync<ElasticQuery>($"/api/queries/{id}");

            if (elasticquery == null)
            {
                return NotFound();
            }
            
            ElasticQuery = elasticquery;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var success = await _apiClient.DeleteAsync($"/api/queries/{id}");
            if (!success)
            {
                ModelState.AddModelError(string.Empty, "Failed to delete query.");
                return Page();
            }

            return RedirectToPage("./Index");
        }
    }
}
