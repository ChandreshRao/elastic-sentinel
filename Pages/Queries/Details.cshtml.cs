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
    public class DetailsModel : PageModel
    {
        private readonly SentinelDbContext _context;

        public DetailsModel(SentinelDbContext context)
        {
            _context = context;
        }

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
    }
}
