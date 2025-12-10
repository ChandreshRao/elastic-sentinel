using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ElasticSentinel.Infrastructure.Persistence;
using ElasticSentinel.Domain.Entities;

namespace ElasticSentinel.Pages.Queries
{
    public class DeleteModel : PageModel
    {
        private readonly SentinelDbContext _context;

        public DeleteModel(SentinelDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public required ElasticQuery ElasticQuery { get; set; }        public async Task<IActionResult> OnGetAsync(short? id)
        {
            if (id == null || _context.ElasticQueries == null)
            {
                return NotFound();
            }

            var elasticquery = await _context.ElasticQueries.FirstOrDefaultAsync(m => m.ElasticQueryId == id);

            if (elasticquery == null)
            {
                return NotFound();
            }
            else 
            {
                ElasticQuery = elasticquery;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(short? id)
        {
            if (id == null || _context.ElasticQueries == null)
            {
                return NotFound();
            }
            var elasticquery = await _context.ElasticQueries.FindAsync(id);

            if (elasticquery != null)
            {
                ElasticQuery = elasticquery;
                _context.ElasticQueries.Remove(ElasticQuery);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
