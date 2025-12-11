using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ElasticSentinel.Domain.Entities;
using ElasticSentinel.Infrastructure.Services;

namespace ElasticSentinel.Pages.ElasticsearchSettings
{
    public class EditModel : PageModel
    {
        private readonly ApiClientService _apiClient;

        public EditModel(ApiClientService apiClient)
        {
            _apiClient = apiClient;
        }

        [BindProperty]
        public ElasticConfiguration ElasticConfiguration { get; set; } = default!;

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

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var result = await _apiClient.PutAsync<ElasticConfiguration>($"/api/elastic-configurations/{ElasticConfiguration.ElasticConfigId}", ElasticConfiguration);
            if (result == null)
            {
                ModelState.AddModelError(string.Empty, "Failed to update Elasticsearch configuration.");
                return Page();
            }

            return RedirectToPage("./Index");
        }
    }
}
