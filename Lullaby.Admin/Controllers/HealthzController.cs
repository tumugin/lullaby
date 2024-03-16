namespace Lullaby.Admin.Controllers;

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class HealthzController : Controller
{
    public IActionResult Index() => this.Ok(new { });
}
