using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ElasticSentinel.Infrastructure.Persistence;
using ElasticSentinel.Domain.Entities;

namespace ElasticSentinel.Pages.Connectors.MailConnectorDetail
{
    public class DetailsModel : PageModel
    {
        private readonly SentinelDbContext _context;

        public DetailsModel(SentinelDbContext context)
        {
            _context = context;
        }

      public EmailConnectorDetail EmailConnectorDetail { get; set; } = default!; 

        public async Task<IActionResult> OnGetAsync(short? id)
        {
            if (id == null || _context.EmailConnectorDetails == null)
            {
                return NotFound();
            }

            var emailconnectordetail = await _context.EmailConnectorDetails.FirstOrDefaultAsync(m => m.EmailAlertDetailId == id);
            if (emailconnectordetail == null)
            {
                return NotFound();
            }
            else 
            {
                EmailConnectorDetail = emailconnectordetail;
            }
            return Page();
        }
    }
}
