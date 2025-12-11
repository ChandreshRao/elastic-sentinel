using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ElasticSentinel.Infrastructure.Persistence;
using ElasticSentinel.Domain.Entities;
using ElasticSentinel.Infrastructure.Services;

namespace ElasticSentinel.Pages.Scheduler
{
    public class CreateModel : PageModel
    {
        private readonly SentinelDbContext _context;
        private readonly ApiClientService _apiClient;

        public CreateModel(SentinelDbContext context, ApiClientService apiClient)
        {
            _context = context;
            _apiClient = apiClient;
        }

        public IActionResult OnGet()
        {
            ViewData["ElasticConfigId"] = new SelectList(_context.ElasticConfigurations, "ElasticConfigId", "ClusterName");
            ViewData["ElasticQueryId"] = new SelectList(_context.ElasticQueries, "ElasticQueryId", "QueryName");
            ViewData["EmailConnectorId"] = new SelectList(_context.EmailConnectors, "EmailConnectorId", "Name");
            ViewData["MSTeamsConnectorId"] = new SelectList(_context.MSTeamsConnectors, "MSTeamsConnectorId", "Name");
            ViewData["EmailAlertDetailId"] = new SelectList(_context.EmailConnectorDetails, "EmailAlertDetailId", "Name");
            ViewData["NotificationTemplateId"] = new SelectList(_context.NotificationTemplateDetails, "NotificationTemplateId", "TemplateName");
            return Page();
        }

        [BindProperty]
        public required AlertSchedulerConfig AlertSchedulerConfig { get; set; }
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var result = await _apiClient.PostAsync<AlertSchedulerConfig>("/api/scheduler/configs", AlertSchedulerConfig);
            if (result == null)
            {
                ModelState.AddModelError(string.Empty, "Failed to create scheduler config.");
                return Page();
            }

            return RedirectToPage("./Index");
        }
    }
}
