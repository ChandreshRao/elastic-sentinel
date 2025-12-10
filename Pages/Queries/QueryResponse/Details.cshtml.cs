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
    public class DetailsModel : PageModel
    {
        private readonly SentinelDbContext _context;

        public DetailsModel(SentinelDbContext context)
        {
            _context = context;
        }

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
    }
}
