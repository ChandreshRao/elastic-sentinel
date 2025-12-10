using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ElasticSentinel.Infrastructure.Persistence;
using ElasticSentinel.Domain.Entities;

namespace ElasticSentinel.Pages.Scheduler
{
    public class CreateModel : PageModel
    {
        private readonly SentinelDbContext _context;

        public CreateModel(SentinelDbContext context)
        {
            _context = context;
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

            _context.AlertSchedulerConfigs.Add(AlertSchedulerConfig);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
