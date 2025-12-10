using ElasticSentinel.Domain.Entities;
using ElasticSentinel.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace ElasticSentinel.Pages.Scheduler
{
    public class IndexModel : PageModel
    {
        private readonly SentinelDbContext _context;

        public IndexModel(SentinelDbContext context)
        {
            _context = context;
        }

        public IList<AlertSchedulerConfig> AlertSchedulerConfig { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.AlertSchedulerConfigs != null)
            {
                AlertSchedulerConfig = await _context.AlertSchedulerConfigs.ToListAsync();
            }
        }
    }
}
