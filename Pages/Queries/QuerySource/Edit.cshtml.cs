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
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ElasticSentinel.Pages.Queries.QuerySource
{
    public class EditModel : PageModel
    {
        private readonly SentinelDbContext _context;

        public EditModel(SentinelDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public ElasticDynamicQuerySource ElasticDynamicQuerySource { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(short? id)
        {
            if (id == null || _context.ElasticDynamicQuerySources == null)
            {
                return NotFound();
            }

            var elasticdynamicquerysource =  await _context.ElasticDynamicQuerySources.FirstOrDefaultAsync(m => m.ElasticDynamicQuerySourceId == id);
            if (elasticdynamicquerysource == null)
            {
                return NotFound();
            }
            ElasticDynamicQuerySource = elasticdynamicquerysource;
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
            
            _context.Attach(ElasticDynamicQuerySource).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ElasticDynamicQuerySourceExists(ElasticDynamicQuerySource.ElasticDynamicQuerySourceId))
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

        private bool ElasticDynamicQuerySourceExists(short id)
        {
          return _context.ElasticDynamicQuerySources.Any(e => e.ElasticDynamicQuerySourceId == id);
        }
    }
}
