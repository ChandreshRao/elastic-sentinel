using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ElasticSentinel.Infrastructure.Persistence;
using ElasticSentinel.Domain.Entities;

namespace ElasticSentinel.Pages.Scheduler
{
    public class DeleteModel : PageModel
    {
        private readonly SentinelDbContext _context;

        public DeleteModel(SentinelDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public required AlertSchedulerConfig AlertSchedulerConfig { get; set; }        public async Task<IActionResult> OnGetAsync(short? id)
        {
            if (id == null || _context.AlertSchedulerConfigs == null)
            {
                return NotFound();
            }

            var alertschedulerconfig = await _context.AlertSchedulerConfigs.FirstOrDefaultAsync(m => m.AlertSchedulerConfigId == id);

            if (alertschedulerconfig == null)
            {
                return NotFound();
            }
            else 
            {
                AlertSchedulerConfig = alertschedulerconfig;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(short? id)
        {
            if (id == null || _context.AlertSchedulerConfigs == null)
            {
                return NotFound();
            }
            var alertschedulerconfig = await _context.AlertSchedulerConfigs.FindAsync(id);

            if (alertschedulerconfig != null)
            {
                AlertSchedulerConfig = alertschedulerconfig;
                _context.AlertSchedulerConfigs.Remove(AlertSchedulerConfig);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
