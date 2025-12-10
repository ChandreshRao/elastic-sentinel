using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ElasticSentinel.Infrastructure.Persistence;
using ElasticSentinel.Domain.Entities;

namespace ElasticSentinel.Pages.ElasticsearchSettings
{
    public class EditModel : PageModel
    {
        private readonly SentinelDbContext _context;

        public EditModel(SentinelDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public ElasticConfiguration ElasticConfiguration { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(short? id)
        {
            if (id == null || _context.ElasticConfigurations == null)
            {
                return NotFound();
            }

            var elasticconfiguration =  await _context.ElasticConfigurations.FirstOrDefaultAsync(m => m.ElasticConfigId == id);
            if (elasticconfiguration == null)
            {
                return NotFound();
            }
            ElasticConfiguration = elasticconfiguration;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(ElasticConfiguration).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ElasticConfigurationExists(ElasticConfiguration.ElasticConfigId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool ElasticConfigurationExists(short id)
        {
          return _context.ElasticConfigurations.Any(e => e.ElasticConfigId == id);
        }
    }
}
