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

namespace ElasticSentinel.Pages.Connectors.TeamsConnector
{
    public class EditModel : PageModel
    {
        private readonly SentinelDbContext _context;

        public EditModel(SentinelDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public MSTeamsConnector MSTeamsConnector { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(short? id)
        {
            if (id == null || _context.MSTeamsConnectors == null)
            {
                return NotFound();
            }

            var msteamsconnector =  await _context.MSTeamsConnectors.FirstOrDefaultAsync(m => m.MSTeamsConnectorId == id);
            if (msteamsconnector == null)
            {
                return NotFound();
            }
            MSTeamsConnector = msteamsconnector;
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

            _context.Attach(MSTeamsConnector).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MSTeamsConnectorExists(MSTeamsConnector.MSTeamsConnectorId))
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

        private bool MSTeamsConnectorExists(short id)
        {
          return _context.MSTeamsConnectors.Any(e => e.MSTeamsConnectorId == id);
        }
    }
}
