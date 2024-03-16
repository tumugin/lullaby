namespace Lullaby.Admin.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

[Route("[controller]")]
public class ErrorController : Controller
{
    [AllowAnonymous]
    [Route("{errorCode}")]
    public IActionResult Error(int errorCode)
    {
        var statusCodeReExecuteFeature = this.HttpContext.Features.Get<IStatusCodeReExecuteFeature>();

        if (statusCodeReExecuteFeature == null)
        {
            return this.NotFound();
        }

        return errorCode switch
        {
            404 => this.View("404"),
            _ => this.View(errorCode)
        };
    }
}
