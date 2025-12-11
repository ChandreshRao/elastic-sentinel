using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ElasticSentinel.Domain.Entities;
using ElasticSentinel.Infrastructure.Services;

namespace ElasticSentinel.Pages.Queries
{
    public class DetailsModel : PageModel
    {
        private readonly ApiClientService _apiClient;

        public DetailsModel(ApiClientService apiClient)
        {
            _apiClient = apiClient;
        }

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
    }
}
