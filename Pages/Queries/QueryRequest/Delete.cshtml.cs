using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ElasticSentinel.Infrastructure.Persistence;
using ElasticSentinel.Domain.Entities;

namespace ElasticSentinel.Pages.Queries.QueryRequest
{
    public class DeleteModel : PageModel
    {
        private readonly SentinelDbContext _context;

        public DeleteModel(SentinelDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public required ElasticDynamicQueryRequestDetail ElasticDynamicQueryRequestDetail { get; set; }        public async Task<IActionResult> OnGetAsync(short? id)
        {
            if (id == null || _context.ElasticDynamicQueryRequestDetails == null)
            {
                return NotFound();
            }

            var elasticdynamicqueryrequestdetail = await _context.ElasticDynamicQueryRequestDetails.FirstOrDefaultAsync(m => m.ElasticDynamicQueryDetailId == id);

            if (elasticdynamicqueryrequestdetail == null)
            {
                return NotFound();
            }
            else 
            {
                ElasticDynamicQueryRequestDetail = elasticdynamicqueryrequestdetail;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(short? id)
        {
            if (id == null || _context.ElasticDynamicQueryRequestDetails == null)
            {
                return NotFound();
            }
            var elasticdynamicqueryrequestdetail = await _context.ElasticDynamicQueryRequestDetails.FindAsync(id);

            if (elasticdynamicqueryrequestdetail != null)
            {
                ElasticDynamicQueryRequestDetail = elasticdynamicqueryrequestdetail;
                _context.ElasticDynamicQueryRequestDetails.Remove(ElasticDynamicQueryRequestDetail);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
