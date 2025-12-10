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
    public class DetailsModel : PageModel
    {
        private readonly SentinelDbContext _context;

        public DetailsModel(SentinelDbContext context)
        {
            _context = context;
        }

        public required AlertSchedulerConfig AlertSchedulerConfig { get; set; }

        public async Task<IActionResult> OnGetAsync(short? id)
        {
            if (id == null || _context.AlertSchedulerConfigs == null)
            {
                return NotFound();
            }

            var alertschedulerconfig = await _context.AlertSchedulerConfigs
                .Include(r => r.ElasticConfig)
                .Include(r => r.MailConnector)
                .Include(r => r.MailConnectorDetail)
                .Include(r => r.TeamsConnector)
                .Include(r => r.Query)
                .Include(r => r.Template)
                .FirstOrDefaultAsync(m => m.AlertSchedulerConfigId == id);
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
    }
}
