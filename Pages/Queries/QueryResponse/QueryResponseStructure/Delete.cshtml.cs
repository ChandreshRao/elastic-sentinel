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
    public class DeleteModel : PageModel
    {
        private readonly SentinelDbContext _context;

        public DeleteModel(SentinelDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public required ElasticDynamicQueryResponseStructure ElasticDynamicQueryResponseStructure { get; set; }        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.ElasticDynamicQueryResponseStructures == null)
            {
                return NotFound();
            }

            var elasticdynamicqueryresponsestructure = await _context.ElasticDynamicQueryResponseStructures.FirstOrDefaultAsync(m => m.ElasticDynamicQueryResponseStructureId == id);

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

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.ElasticDynamicQueryResponseStructures == null)
            {
                return NotFound();
            }
            var elasticdynamicqueryresponsestructure = await _context.ElasticDynamicQueryResponseStructures.FindAsync(id);

            if (elasticdynamicqueryresponsestructure != null)
            {
                ElasticDynamicQueryResponseStructure = elasticdynamicqueryresponsestructure;
                _context.ElasticDynamicQueryResponseStructures.Remove(ElasticDynamicQueryResponseStructure);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
