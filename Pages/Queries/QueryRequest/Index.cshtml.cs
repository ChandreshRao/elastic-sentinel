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
    public class IndexModel : PageModel
    {
        private readonly SentinelDbContext _context;

        public IndexModel(SentinelDbContext context)
        {
            _context = context;
        }

        public IList<ElasticDynamicQueryRequestDetail> ElasticDynamicQueryRequestDetail { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.ElasticDynamicQueryRequestDetails != null)
            {
                ElasticDynamicQueryRequestDetail = await _context.ElasticDynamicQueryRequestDetails
                .Include(e => e.QuerySource).ToListAsync();
            }
        }
    }
}
