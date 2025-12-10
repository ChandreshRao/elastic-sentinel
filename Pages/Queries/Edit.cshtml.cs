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

namespace ElasticSentinel.Pages.Queries
{
    public class EditModel : PageModel
    {
        private readonly SentinelDbContext _context;

        public EditModel(SentinelDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public ElasticQuery ElasticQuery { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(short? id)
        {
            if (id == null || _context.ElasticQueries == null)
            {
                return NotFound();
            }

            var elasticquery =  await _context.ElasticQueries.FirstOrDefaultAsync(m => m.ElasticQueryId == id);
            if (elasticquery == null)
            {
                return NotFound();
            }
            ElasticQuery = elasticquery;
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

            _context.Attach(ElasticQuery).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ElasticQueryExists(ElasticQuery.ElasticQueryId))
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

        private bool ElasticQueryExists(short id)
        {
          return _context.ElasticQueries.Any(e => e.ElasticQueryId == id);
        }
    }
}
