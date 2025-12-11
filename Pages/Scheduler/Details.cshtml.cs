using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ElasticSentinel.Domain.Entities;
using ElasticSentinel.Infrastructure.Services;
using ElasticSentinel.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ElasticSentinel.Pages.Scheduler
{
    public class DetailsModel : PageModel
    {
        private readonly ApiClientService _apiClient;
        private readonly SentinelDbContext _context;

        public DetailsModel(ApiClientService apiClient, SentinelDbContext context)
        {
            _apiClient = apiClient;
            _context = context;
        }

        public required AlertSchedulerConfig AlertSchedulerConfig { get; set; }

        public async Task<IActionResult> OnGetAsync(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Use DbContext for Details page to get related entities via Include
            var alertschedulerconfig = await _context.AlertSchedulerConfigs
                .Include(r => r.ElasticConfig)
                .Include(r => r.MailConnector)
                .Include(r => r.MailConnectorDetail)
                .Include(r => r.TeamsConnector)
                .Include(r => r.Query)
                .Include(r => r.Template)
                .FirstOrDefaultAsync(m => m.AlertSchedulerConfigId == id);
            if (alertschedulerconfig == null)
            {
                return NotFound();
            }
            
            AlertSchedulerConfig = alertschedulerconfig;
            return Page();
        }
    }
}
