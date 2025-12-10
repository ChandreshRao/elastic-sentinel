using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ElasticSentinel.Infrastructure.Persistence;
using ElasticSentinel.Domain.Entities;

namespace ElasticSentinel.Pages.Queries.QueryResponse.QueryResponseStructure
{
    public class CreateModel : PageModel
    {
        private readonly SentinelDbContext _context;

        public CreateModel(SentinelDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["ElasticDynamicQueryResponseDetailId"] = new SelectList(_context.ElasticDynamicQueryResponseDetails, "ElasticDynamicQueryResponseDetailId", "QueryResponseMapperName");
            return Page();
        }

        [BindProperty]
        public required ElasticDynamicQueryResponseStructure ElasticDynamicQueryResponseStructure { get; set; }
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.ElasticDynamicQueryResponseStructures.Add(ElasticDynamicQueryResponseStructure);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
