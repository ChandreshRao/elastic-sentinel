using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ElasticSentinel.Domain.Entities;
using ElasticSentinel.Infrastructure.Services;

namespace ElasticSentinel.Pages.AlertMessageTemplates
{
    public class EditModel : PageModel
    {
        private readonly ApiClientService _apiClient;

        public EditModel(ApiClientService apiClient)
        {
            _apiClient = apiClient;
        }

        [BindProperty]
        public NotificationTemplate NotificationTemplate { get; set; } = default!;

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

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var result = await _apiClient.PutAsync<NotificationTemplate>($"/api/templates/{NotificationTemplate.NotificationTemplateId}", NotificationTemplate);
            if (result == null)
            {
                ModelState.AddModelError(string.Empty, "Failed to update notification template.");
                return Page();
            }

            return RedirectToPage("./Index");
        }
    }
}
