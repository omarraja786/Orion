using System.ComponentModel.DataAnnotations;
using Orion.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using Orion.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;

namespace Orion.Controllers
{
    [AllowAnonymous]
    public class LoginController(ILogger<LoginController> logger, DataContext dataContext) : Controller
    {
        private readonly ILogger<LoginController> _logger = logger;

        [BindProperty] public InputModel Input { get; set; }

        [BindProperty] public string RedirectUrl { get; set; }

        public class InputModel
        {
            [Required] [EmailAddress] public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }


        }

        public IActionResult Login(string returnUrl)
        {
            var x = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LoginButton(string returnUrl = "")
        {
            //returnUrl ??= Url.Content("~/");

            var user = dataContext.Users.FirstOrDefault(f => f.Email == Input.Email && f.Password == Input.Password);

            if (user == null)
            {
                ViewBag.FailLogin = "Invalid Email or Password";
                return View("Login");
            }

            user.IsLoggedIn = true;
            dataContext.SaveChanges(); 

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.FirstName + " " + user.LastName),
                new Claim(ClaimTypes.Email, user.Email),
            };

            
            if (user.IsAdmin)
            {
                claims.Add(new Claim("Admin", "true"));
            }

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                new AuthenticationProperties {IsPersistent = true});

            return LocalRedirect(returnUrl);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}
