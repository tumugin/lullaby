namespace Lullaby.Admin.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

public class ErrorController : Controller
{
    [Authorize]
    [Route("/Error/{errorCode}")]
    public IActionResult Error(int errorCode) =>
        errorCode switch
        {
            404 => this.View("404"),
            _ => this.View(errorCode)
        };
}
