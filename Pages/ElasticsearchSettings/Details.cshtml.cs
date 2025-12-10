using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ElasticSentinel.Infrastructure.Persistence;
using ElasticSentinel.Domain.Entities;

namespace ElasticSentinel.Pages.ElasticsearchSettings
{
    public class DetailsModel : PageModel
    {
        private readonly SentinelDbContext _context;

        public DetailsModel(SentinelDbContext context)
        {
            _context = context;
        }

        public required ElasticConfiguration ElasticConfiguration { get; set; }        public async Task<IActionResult> OnGetAsync(short? id)
        {
            if (id == null || _context.ElasticConfigurations == null)
            {
                return NotFound();
            }

            var elasticconfiguration = await _context.ElasticConfigurations.FirstOrDefaultAsync(m => m.ElasticConfigId == id);
            if (elasticconfiguration == null)
            {
                return NotFound();
            }
            else 
            {
                ElasticConfiguration = elasticconfiguration;
            }
            return Page();
        }
    }
}
