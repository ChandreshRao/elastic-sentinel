using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ElasticSentinel.Domain.Entities;
using ElasticSentinel.Infrastructure.Services;

namespace ElasticSentinel.Pages.AlertMessageTemplates
{
    public class DetailsModel : PageModel
    {
        private readonly ApiClientService _apiClient;

        public DetailsModel(ApiClientService apiClient)
        {
            _apiClient = apiClient;
        }

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
    }
}
