namespace Lullaby.Admin.Controllers;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

public class LogoutController() : Controller
{
    [Authorize]
    [Route("/logout")]
    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await this.HttpContext.SignOutAsync();
        return this.RedirectToAction("Index", "Index");
    }
}
