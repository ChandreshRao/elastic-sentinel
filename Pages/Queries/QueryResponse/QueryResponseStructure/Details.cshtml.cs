using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ElasticSentinel.Infrastructure.Persistence;
using ElasticSentinel.Domain.Entities;

namespace ElasticSentinel.Pages.Queries.QueryResponse.QueryResponseStructure
{
    public class DetailsModel : PageModel
    {
        private readonly SentinelDbContext _context;

        public DetailsModel(SentinelDbContext context)
        {
            _context = context;
        }

        public required ElasticDynamicQueryResponseStructure ElasticDynamicQueryResponseStructure { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.ElasticDynamicQueryResponseStructures == null)
            {
                return NotFound();
            }

            var elasticdynamicqueryresponsestructure = await _context.ElasticDynamicQueryResponseStructures.Include(r => r.DynamicQueryResponseDetail).FirstOrDefaultAsync(m => m.ElasticDynamicQueryResponseStructureId == id);
            if (elasticdynamicqueryresponsestructure == null)
            {
                return NotFound();
            }
            else
            {
                ElasticDynamicQueryResponseStructure = elasticdynamicqueryresponsestructure;
            }
            return Page();
        }
    }
}
