namespace Lullaby.Controllers;

using Groups;
using Microsoft.AspNetCore.Mvc;
using ViewModels.Index;

[Route("/")]
[ApiExplorerSettings(IgnoreApi = true)]
public class IndexController : Controller
{
    [HttpGet]
    public IActionResult Index([FromServices] IEnumerable<IGroup> groups) =>
        this.View(new IndexViewModel { AvailableGroups = groups });
}
