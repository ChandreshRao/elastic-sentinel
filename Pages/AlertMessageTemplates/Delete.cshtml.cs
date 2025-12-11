using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ElasticSentinel.Domain.Entities;
using ElasticSentinel.Infrastructure.Services;

namespace ElasticSentinel.Pages.AlertMessageTemplates
{
    public class DeleteModel : PageModel
    {
        private readonly ApiClientService _apiClient;

        public DeleteModel(ApiClientService apiClient)
        {
            _apiClient = apiClient;
        }

        [BindProperty]
        public required NotificationTemplate NotificationTemplate { get; set; }

        public async Task<IActionResult> OnGetAsync(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var notificationtemplate = await _apiClient.GetAsync<NotificationTemplate>($"/api/templates/{id}");

            if (notificationtemplate == null)
            {
                return NotFound();
            }
            
            NotificationTemplate = notificationtemplate;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var success = await _apiClient.DeleteAsync($"/api/templates/{id}");
            if (!success)
            {
                ModelState.AddModelError(string.Empty, "Failed to delete notification template.");
                return Page();
            }

            return RedirectToPage("./Index");
        }
    }
}
