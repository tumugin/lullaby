namespace Lullaby.Controllers;

using Microsoft.AspNetCore.Mvc;

[Route("/")]
public class IndexController : Controller
{
    public IActionResult Index() => this.View();
}
