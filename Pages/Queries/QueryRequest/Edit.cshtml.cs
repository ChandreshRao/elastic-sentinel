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

namespace ElasticSentinel.Pages.Queries.QueryRequest
{
    public class EditModel : PageModel
    {
        private readonly SentinelDbContext _context;

        public EditModel(SentinelDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public ElasticDynamicQueryRequestDetail ElasticDynamicQueryRequestDetail { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(short? id)
        {
            if (id == null || _context.ElasticDynamicQueryRequestDetails == null)
            {
                return NotFound();
            }

            var elasticdynamicqueryrequestdetail =  await _context.ElasticDynamicQueryRequestDetails.FirstOrDefaultAsync(m => m.ElasticDynamicQueryDetailId == id);
            if (elasticdynamicqueryrequestdetail == null)
            {
                return NotFound();
            }
            ElasticDynamicQueryRequestDetail = elasticdynamicqueryrequestdetail;
           ViewData["ElasticDynamicQuerySourceId"] = new SelectList(_context.ElasticDynamicQuerySources, "ElasticDynamicQuerySourceId", "SourceName");
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

            _context.Attach(ElasticDynamicQueryRequestDetail).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ElasticDynamicQueryRequestDetailExists(ElasticDynamicQueryRequestDetail.ElasticDynamicQueryDetailId))
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

        private bool ElasticDynamicQueryRequestDetailExists(short id)
        {
          return _context.ElasticDynamicQueryRequestDetails.Any(e => e.ElasticDynamicQueryDetailId == id);
        }
    }
}
