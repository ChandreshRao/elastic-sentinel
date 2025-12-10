using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ElasticSentinel.Infrastructure.Persistence;
using ElasticSentinel.Domain.Entities;

namespace ElasticSentinel.Pages.Connectors.TeamsConnector
{
    public class DeleteModel : PageModel
    {
        private readonly SentinelDbContext _context;

        public DeleteModel(SentinelDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public required MSTeamsConnector MSTeamsConnector { get; set; }        public async Task<IActionResult> OnGetAsync(short? id)
        {
            if (id == null || _context.MSTeamsConnectors == null)
            {
                return NotFound();
            }

            var msteamsconnector = await _context.MSTeamsConnectors.FirstOrDefaultAsync(m => m.MSTeamsConnectorId == id);

            if (msteamsconnector == null)
            {
                return NotFound();
            }
            else 
            {
                MSTeamsConnector = msteamsconnector;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(short? id)
        {
            if (id == null || _context.MSTeamsConnectors == null)
            {
                return NotFound();
            }
            var msteamsconnector = await _context.MSTeamsConnectors.FindAsync(id);

            if (msteamsconnector != null)
            {
                MSTeamsConnector = msteamsconnector;
                _context.MSTeamsConnectors.Remove(MSTeamsConnector);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
