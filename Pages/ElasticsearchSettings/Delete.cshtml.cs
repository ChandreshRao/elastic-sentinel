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
    public class DeleteModel : PageModel
    {
        private readonly SentinelDbContext _context;

        public DeleteModel(SentinelDbContext context)
        {
            _context = context;
        }

        [BindProperty]
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

        public async Task<IActionResult> OnPostAsync(short? id)
        {
            if (id == null || _context.ElasticConfigurations == null)
            {
                return NotFound();
            }
            var elasticconfiguration = await _context.ElasticConfigurations.FindAsync(id);

            if (elasticconfiguration != null)
            {
                ElasticConfiguration = elasticconfiguration;
                _context.ElasticConfigurations.Remove(ElasticConfiguration);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
