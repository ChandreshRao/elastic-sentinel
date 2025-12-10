using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ElasticSentinel.Infrastructure.Persistence;
using ElasticSentinel.Domain.Entities;

namespace ElasticSentinel.Pages.Queries.QueryResponse
{
    public class DeleteModel : PageModel
    {
        private readonly SentinelDbContext _context;

        public DeleteModel(SentinelDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public required ElasticDynamicQueryResponseDetail ElasticDynamicQueryResponseDetail { get; set; }        public async Task<IActionResult> OnGetAsync(short? id)
        {
            if (id == null || _context.ElasticDynamicQueryResponseDetails == null)
            {
                return NotFound();
            }

            var elasticdynamicqueryresponsedetail = await _context.ElasticDynamicQueryResponseDetails.FirstOrDefaultAsync(m => m.ElasticDynamicQueryResponseDetailId == id);

            if (elasticdynamicqueryresponsedetail == null)
            {
                return NotFound();
            }
            else 
            {
                ElasticDynamicQueryResponseDetail = elasticdynamicqueryresponsedetail;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(short? id)
        {
            if (id == null || _context.ElasticDynamicQueryResponseDetails == null)
            {
                return NotFound();
            }
            var elasticdynamicqueryresponsedetail = await _context.ElasticDynamicQueryResponseDetails.FindAsync(id);

            if (elasticdynamicqueryresponsedetail != null)
            {
                ElasticDynamicQueryResponseDetail = elasticdynamicqueryresponsedetail;
                _context.ElasticDynamicQueryResponseDetails.Remove(ElasticDynamicQueryResponseDetail);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
