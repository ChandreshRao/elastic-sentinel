using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ElasticSentinel.Domain.Entities;
using ElasticSentinel.Infrastructure.Services;

namespace ElasticSentinel.Pages.Scheduler
{
    public class DeleteModel : PageModel
    {
        private readonly ApiClientService _apiClient;

        public DeleteModel(ApiClientService apiClient)
        {
            _apiClient = apiClient;
        }

        [BindProperty]
        public required AlertSchedulerConfig AlertSchedulerConfig { get; set; }

        public async Task<IActionResult> OnGetAsync(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var alertschedulerconfig = await _apiClient.GetAsync<AlertSchedulerConfig>($"/api/scheduler/configs/{id}");

            if (alertschedulerconfig == null)
            {
                return NotFound();
            }
            
            AlertSchedulerConfig = alertschedulerconfig;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var success = await _apiClient.DeleteAsync($"/api/scheduler/configs/{id}");
            if (!success)
            {
                ModelState.AddModelError(string.Empty, "Failed to delete scheduler config.");
                return Page();
            }

            return RedirectToPage("./Index");
        }
    }
}
