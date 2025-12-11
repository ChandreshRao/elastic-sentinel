using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ElasticSentinel.Domain.Entities;
using ElasticSentinel.Infrastructure.Services;

namespace ElasticSentinel.Pages.ElasticsearchSettings
{
    public class DeleteModel : PageModel
    {
        private readonly ApiClientService _apiClient;

        public DeleteModel(ApiClientService apiClient)
        {
            _apiClient = apiClient;
        }

        [BindProperty]
        public required ElasticConfiguration ElasticConfiguration { get; set; }

        public async Task<IActionResult> OnGetAsync(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var elasticconfiguration = await _apiClient.GetAsync<ElasticConfiguration>($"/api/elastic-configurations/{id}");

            if (elasticconfiguration == null)
            {
                return NotFound();
            }
            
            ElasticConfiguration = elasticconfiguration;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var success = await _apiClient.DeleteAsync($"/api/elastic-configurations/{id}");
            if (!success)
            {
                ModelState.AddModelError(string.Empty, "Failed to delete Elasticsearch configuration.");
                return Page();
            }

            return RedirectToPage("./Index");
        }
    }
}
