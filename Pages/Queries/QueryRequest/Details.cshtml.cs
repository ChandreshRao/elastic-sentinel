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
    public class DetailsModel : PageModel
    {
        private readonly SentinelDbContext _context;

        public DetailsModel(SentinelDbContext context)
        {
            _context = context;
        }

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
    }
}
