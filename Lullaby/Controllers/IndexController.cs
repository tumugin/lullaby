namespace Lullaby.Controllers;

using Common.Groups;
using Microsoft.AspNetCore.Mvc;
using ViewModels.Index;

[Route("/")]
[ApiExplorerSettings(IgnoreApi = true)]
public class IndexController : Controller
{
    [HttpGet]
    public IActionResult Index([FromServices] IEnumerable<IGroup> groups) =>
        this.View(new IndexViewModel
        {
            AvailableGroups = groups
                .ToArray()
                .GroupBy(v => v.GroupKey)
                .Select(v => v.First())
                .ToArray()
        });
}
