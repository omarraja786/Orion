using Orion.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Orion.Data;
using System.Security.Claims;

namespace Orion.Controllers
{
    [AllowAnonymous]
    public class LogoutController(ILogger<LogoutController> logger, DataContext dataContext) : Controller
    {
        private readonly ILogger<LogoutController> _logger = logger;

        public IActionResult Logout()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LogoutButton(string returnUrl = null)
        {
            var userEmail = HttpContext.User.FindFirstValue(ClaimTypes.Email);

            if (!string.IsNullOrEmpty(userEmail))
            {
                var foundUser = dataContext.Users.SingleOrDefault(x => x.Email == userEmail);
                foundUser.IsLoggedIn = false;
                dataContext.SaveChanges();
            }

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            if (returnUrl != null)
            {
                return LocalRedirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
            
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
