using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ElasticSentinel.Domain.Entities;
using ElasticSentinel.Infrastructure.Services;

namespace ElasticSentinel.Pages.ElasticsearchSettings
{
    public class DetailsModel : PageModel
    {
        private readonly ApiClientService _apiClient;

        public DetailsModel(ApiClientService apiClient)
        {
            _apiClient = apiClient;
        }

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
    }
}
