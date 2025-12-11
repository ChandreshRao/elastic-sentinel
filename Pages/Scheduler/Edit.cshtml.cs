using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ElasticSentinel.Infrastructure.Persistence;
using ElasticSentinel.Domain.Entities;
using ElasticSentinel.Infrastructure.Services;

namespace ElasticSentinel.Pages.Scheduler
{
    public class EditModel : PageModel
    {
        private readonly SentinelDbContext _context;
        private readonly ApiClientService _apiClient;

        public EditModel(SentinelDbContext context, ApiClientService apiClient)
        {
            _context = context;
            _apiClient = apiClient;
        }

        [BindProperty]
        public AlertSchedulerConfig AlertSchedulerConfig { get; set; } = default!;

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
            ViewData["ElasticConfigId"] = new SelectList(_context.ElasticConfigurations, "ElasticConfigId", "ClusterName");
            ViewData["ElasticQueryId"] = new SelectList(_context.ElasticQueries, "ElasticQueryId", "QueryName");
            ViewData["EmailConnectorId"] = new SelectList(_context.EmailConnectors, "EmailConnectorId", "Name");
            ViewData["MSTeamsConnectorId"] = new SelectList(_context.MSTeamsConnectors, "MSTeamsConnectorId", "Name");
            ViewData["EmailAlertDetailId"] = new SelectList(_context.EmailConnectorDetails, "EmailAlertDetailId", "Name");
            ViewData["NotificationTemplateId"] = new SelectList(_context.NotificationTemplateDetails, "NotificationTemplateId", "TemplateName");

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

            var result = await _apiClient.PutAsync<AlertSchedulerConfig>($"/api/scheduler/configs/{AlertSchedulerConfig.AlertSchedulerConfigId}", AlertSchedulerConfig);
            if (result == null)
            {
                ModelState.AddModelError(string.Empty, "Failed to update scheduler config.");
                return Page();
            }

            return RedirectToPage("./Index");
        }
    }
}
