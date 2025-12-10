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

namespace ElasticSentinel.Pages.Queries.QueryResponse
{
    public class EditModel : PageModel
    {
        private readonly SentinelDbContext _context;

        public EditModel(SentinelDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public ElasticDynamicQueryResponseDetail ElasticDynamicQueryResponseDetail { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(short? id)
        {
            if (id == null || _context.ElasticDynamicQueryResponseDetails == null)
            {
                return NotFound();
            }

            var elasticdynamicqueryresponsedetail =  await _context.ElasticDynamicQueryResponseDetails.FirstOrDefaultAsync(m => m.ElasticDynamicQueryResponseDetailId == id);
            if (elasticdynamicqueryresponsedetail == null)
            {
                return NotFound();
            }
            ElasticDynamicQueryResponseDetail = elasticdynamicqueryresponsedetail;
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

            _context.Attach(ElasticDynamicQueryResponseDetail).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ElasticDynamicQueryResponseDetailExists(ElasticDynamicQueryResponseDetail.ElasticDynamicQueryResponseDetailId))
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

        private bool ElasticDynamicQueryResponseDetailExists(short id)
        {
          return _context.ElasticDynamicQueryResponseDetails.Any(e => e.ElasticDynamicQueryResponseDetailId == id);
        }
    }
}
