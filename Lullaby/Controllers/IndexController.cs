namespace Lullaby.Controllers;

using Microsoft.AspNetCore.Mvc;

[Route("/")]
public class IndexController : Controller
{
    [HttpGet]
    public IActionResult Index() => this.View();
}
