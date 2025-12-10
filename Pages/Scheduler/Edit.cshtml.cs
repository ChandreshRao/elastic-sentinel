using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ElasticSentinel.Infrastructure.Persistence;
using ElasticSentinel.Domain.Entities;

namespace ElasticSentinel.Pages.Scheduler
{
    public class EditModel : PageModel
    {
        private readonly SentinelDbContext _context;

        public EditModel(SentinelDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public AlertSchedulerConfig AlertSchedulerConfig { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(short? id)
        {
            if (id == null || _context.AlertSchedulerConfigs == null)
            {
                return NotFound();
            }

            var alertschedulerconfig =  await _context.AlertSchedulerConfigs.FirstOrDefaultAsync(m => m.AlertSchedulerConfigId == id);
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

            _context.Attach(AlertSchedulerConfig).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AlertSchedulerConfigExists(AlertSchedulerConfig.AlertSchedulerConfigId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool AlertSchedulerConfigExists(short id)
        {
          return _context.AlertSchedulerConfigs.Any(e => e.AlertSchedulerConfigId == id);
        }
    }
}
