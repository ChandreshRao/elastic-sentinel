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
    public class DetailsModel : PageModel
    {
        private readonly SentinelDbContext _context;

        public DetailsModel(SentinelDbContext context)
        {
            _context = context;
        }

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
    }
}
