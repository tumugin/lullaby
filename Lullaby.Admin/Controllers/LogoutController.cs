namespace Lullaby.Admin.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

public class LogoutController(SignInManager<IdentityUser> signInManager) : Controller
{
    [Authorize]
    [Route("/logout")]
    [HttpPost]
    public IActionResult Logout()
    {
        signInManager.SignOutAsync();
        return this.RedirectToAction("Index", "Index");
    }
}
