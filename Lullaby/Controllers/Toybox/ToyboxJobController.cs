namespace Lullaby.Controllers.Toybox;

using Microsoft.AspNetCore.Mvc;
using Quartz;
using ViewModels.Toybox;

[Route("/toybox/job")]
[ApiExplorerSettings(IgnoreApi = true)]
public class ToyboxJobController : Controller
{
    private ISchedulerFactory SchedulerFactory { get; }
    private IWebHostEnvironment WebHostEnvironment { get; }

    public ToyboxJobController(ISchedulerFactory schedulerFactory, IWebHostEnvironment webHostEnvironment)
    {
        this.SchedulerFactory = schedulerFactory;
        this.WebHostEnvironment = webHostEnvironment;
    }

    private bool CanShowThisPage()
        => this.WebHostEnvironment.IsDevelopment() || this.WebHostEnvironment.EnvironmentName == "Testing";

    [HttpGet]
    public IActionResult Index()
    {
        if (!this.CanShowThisPage())
        {
            return this.NotFound();
        }

        return this.View(new ToyboxJobViewModel());
    }

    [HttpPost]
    public async Task<IActionResult> IndexPost(string jobKey, CancellationToken cancellationToken)
    {
        if (!this.CanShowThisPage())
        {
            return this.NotFound();
        }

        var scheduler = await this.SchedulerFactory.GetScheduler(cancellationToken);
        await scheduler.TriggerJob(new JobKey(jobKey), cancellationToken);
        return this.Redirect("/toybox/job");
    }
}
