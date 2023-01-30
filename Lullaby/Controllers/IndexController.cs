namespace Lullaby.Controllers;

using Microsoft.AspNetCore.Mvc;

[Route("/")]
[ApiExplorerSettings(IgnoreApi = true)]
public class IndexController : Controller
{
    [HttpGet]
    public IActionResult Index() => this.View();
}
