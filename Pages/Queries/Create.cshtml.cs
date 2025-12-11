using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ElasticSentinel.Domain.Entities;
using ElasticSentinel.Infrastructure.Services;

namespace ElasticSentinel.Pages.Queries
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
        public required ElasticQuery ElasticQuery { get; set; }
        

        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid)
            {
                return Page();
            }

            var result = await _apiClient.PostAsync<ElasticQuery>("/api/queries", ElasticQuery);
            
            if (result != null)
            {
                return RedirectToPage("./Index");
            }

            ModelState.AddModelError(string.Empty, "Failed to create query");
            return Page();
        }
    }
}
