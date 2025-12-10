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

namespace ElasticSentinel.Pages.Queries.QueryResponse.QueryResponseStructure
{
    public class EditModel : PageModel
    {
        private readonly SentinelDbContext _context;

        public EditModel(SentinelDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public ElasticDynamicQueryResponseStructure ElasticDynamicQueryResponseStructure { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.ElasticDynamicQueryResponseStructures == null)
            {
                return NotFound();
            }

            var elasticdynamicqueryresponsestructure =  await _context.ElasticDynamicQueryResponseStructures.FirstOrDefaultAsync(m => m.ElasticDynamicQueryResponseStructureId == id);
            if (elasticdynamicqueryresponsestructure == null)
            {
                return NotFound();
            }
            ElasticDynamicQueryResponseStructure = elasticdynamicqueryresponsestructure;
           ViewData["ElasticDynamicQueryResponseDetailId"] = new SelectList(_context.ElasticDynamicQueryResponseDetails, "ElasticDynamicQueryResponseDetailId", "QueryResponseMapperName");
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

            _context.Attach(ElasticDynamicQueryResponseStructure).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ElasticDynamicQueryResponseStructureExists(ElasticDynamicQueryResponseStructure.ElasticDynamicQueryResponseStructureId))
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

        private bool ElasticDynamicQueryResponseStructureExists(int id)
        {
          return _context.ElasticDynamicQueryResponseStructures.Any(e => e.ElasticDynamicQueryResponseStructureId == id);
        }
    }
}
