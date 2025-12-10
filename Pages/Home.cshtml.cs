using ElasticSentinel.Application.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ElasticSentinel.Application.Common.Models;
using Quartz.Impl.Matchers;
using Quartz;

namespace ElasticSentinel.Pages
{
    public class HomeModel : PageModel
    {
        private readonly ILogger<HomeModel> _logger;
        private readonly IJobManagerService _jobManagerService;

        [BindProperty]
        public List<QuartzJob> QuartzJobs { get; set; } = new List<QuartzJob>();

        public HomeModel(ILogger<HomeModel> logger, IJobManagerService jobManagerService)
        {
            _logger = logger;
            _jobManagerService = jobManagerService;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostRestartJobsAsync()
        {
            var scheduler = _jobManagerService.GetCurrentScheduler();
            if (scheduler != null)
            {
                // Get all job keys
                var allJobKeys = await scheduler.GetJobKeys(GroupMatcher<JobKey>.AnyGroup());

                // Filter out job keys that are not in "MyGroup"
                var jobKeysToDelete = allJobKeys
                    .Where(jobKey => !jobKey.Group.Equals("Alert-Main-Group"))
                    .ToList();

                await scheduler.DeleteJobs(jobKeysToDelete);

                JobKey jobKey = new("Alert-Scheduler-Job", "Alert-Main-Group");

                //// trigger the job manually
                await scheduler.TriggerJob(jobKey);

                await scheduler.Start();
            }
            return RedirectToPage("/Home");
        }

        public async Task<IActionResult> OnGetRunningJobsAsync()
        {
            var lst = await _jobManagerService.GetRunningJobs();
            if (lst != null)
            {
                foreach (var item in lst)
                {
                    QuartzJob job = new()
                    {
                        JobName = item.JobDetail.Key.Name
                    };
                    QuartzJobs.Add(job);
                }
            }

            return Partial("_RunningJobs", QuartzJobs);
        }
    }
}
