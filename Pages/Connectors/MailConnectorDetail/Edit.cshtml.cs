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

namespace ElasticSentinel.Pages.Connectors.MailConnectorDetail
{
    public class EditModel : PageModel
    {
        private readonly SentinelDbContext _context;

        public EditModel(SentinelDbContext context)
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

            var emailconnectordetail =  await _context.EmailConnectorDetails.FirstOrDefaultAsync(m => m.EmailAlertDetailId == id);
            if (emailconnectordetail == null)
            {
                return NotFound();
            }
            EmailConnectorDetail = emailconnectordetail;
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

            _context.Attach(EmailConnectorDetail).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmailConnectorDetailExists(EmailConnectorDetail.EmailAlertDetailId))
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

        private bool EmailConnectorDetailExists(short id)
        {
          return (_context.EmailConnectorDetails?.Any(e => e.EmailAlertDetailId == id)).GetValueOrDefault();
        }
    }
}
