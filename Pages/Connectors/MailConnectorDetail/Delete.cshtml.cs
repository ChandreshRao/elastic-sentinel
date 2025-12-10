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
    public class DeleteModel : PageModel
    {
        private readonly SentinelDbContext _context;

        public DeleteModel(SentinelDbContext context)
        {
            _context = context;
        }

        [BindProperty]
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

        public async Task<IActionResult> OnPostAsync(short? id)
        {
            if (id == null || _context.EmailConnectorDetails == null)
            {
                return NotFound();
            }
            var emailconnectordetail = await _context.EmailConnectorDetails.FindAsync(id);

            if (emailconnectordetail != null)
            {
                EmailConnectorDetail = emailconnectordetail;
                _context.EmailConnectorDetails.Remove(EmailConnectorDetail);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
