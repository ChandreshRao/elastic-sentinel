using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ElasticSentinel.Infrastructure.Persistence;
using ElasticSentinel.Domain.Entities;

namespace ElasticSentinel.Pages.Queries.QuerySource
{
    public class DetailsModel : PageModel
    {
        private readonly SentinelDbContext _context;

        public DetailsModel(SentinelDbContext context)
        {
            _context = context;
        }

        public required ElasticDynamicQuerySource ElasticDynamicQuerySource { get; set; }        public async Task<IActionResult> OnGetAsync(short? id)
        {
            if (id == null || _context.ElasticDynamicQuerySources == null)
            {
                return NotFound();
            }

            var elasticdynamicquerysource = await _context.ElasticDynamicQuerySources.FirstOrDefaultAsync(m => m.ElasticDynamicQuerySourceId == id);
            if (elasticdynamicquerysource == null)
            {
                return NotFound();
            }
            else 
            {
                ElasticDynamicQuerySource = elasticdynamicquerysource;
            }
            return Page();
        }
    }
}
