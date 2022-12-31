namespace Lullaby.Controllers.Toybox;

using Microsoft.AspNetCore.Mvc;
using Quartz;
using ViewModels.Toybox;

[Route("/toybox/job")]
public class ToyboxJobController : Controller
{
    private ISchedulerFactory SchedulerFactory { get; }

    public ToyboxJobController(ISchedulerFactory schedulerFactory) => this.SchedulerFactory = schedulerFactory;

    [HttpGet]
    public IActionResult Index()
    {
        return this.View(new ToyboxJobViewModel());
    }

    [HttpPost]
    public async Task<IActionResult> IndexPost(string JobKey)
    {
        var scheduler = await this.SchedulerFactory.GetScheduler();
        await scheduler.TriggerJob(new JobKey(JobKey));
        return this.Redirect("/toybox/job");
    }
}
