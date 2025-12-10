using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ElasticSentinel.Infrastructure.Persistence;
using ElasticSentinel.Domain.Entities;

namespace ElasticSentinel.Pages.AlertMessageTemplates
{
    public class DetailsModel : PageModel
    {
        private readonly SentinelDbContext _context;

        public DetailsModel(SentinelDbContext context)
        {
            _context = context;
        }

        public required NotificationTemplate NotificationTemplate { get; set; }        public async Task<IActionResult> OnGetAsync(short? id)
        {
            if (id == null || _context.NotificationTemplateDetails == null)
            {
                return NotFound();
            }

            var notificationtemplate = await _context.NotificationTemplateDetails.FirstOrDefaultAsync(m => m.NotificationTemplateId == id);
            if (notificationtemplate == null)
            {
                return NotFound();
            }
            else 
            {
                NotificationTemplate = notificationtemplate;
            }
            return Page();
        }
    }
}
