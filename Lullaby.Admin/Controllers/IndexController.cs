namespace Lullaby.Admin.Controllers;

using Microsoft.AspNetCore.Mvc;

public class IndexController : Controller
{
    public IActionResult Index() => this.View();
}
