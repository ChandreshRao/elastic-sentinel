using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ElasticSentinel.Infrastructure.Persistence;
using ElasticSentinel.Domain.Entities;

namespace ElasticSentinel.Pages.Connectors.MailConnectorDetail
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
            return Page();
        }

        [BindProperty]
        public EmailConnectorDetail EmailConnectorDetail { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid || _context.EmailConnectorDetails == null || EmailConnectorDetail == null)
            {
                return Page();
            }

            _context.EmailConnectorDetails.Add(EmailConnectorDetail);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
