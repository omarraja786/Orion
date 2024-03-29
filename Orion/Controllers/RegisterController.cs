using System;
using Orion.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.IO;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using static Orion.Controllers.LoginController;
using System.ComponentModel.DataAnnotations;
using Orion.Data;

namespace Orion.Controllers
{
    //[Authorize(Policy = "IsAdmin")]
    public class RegisterController(ILogger<RegisterController> logger, IConfiguration configuration,
            IWebHostEnvironment environment, DataContext dataContext)
        : Controller
    {
        public IConfiguration Configuration { get; } = configuration;
        private IWebHostEnvironment Environment = environment;

        [BindProperty] public RegisterModel RegisterInput { get; set; }

        

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult RegisterUser()
        {
            var userModel = new RegisterModel()
            {
                FirstName = RegisterInput.FirstName,
                LastName = RegisterInput.LastName,
                Email = RegisterInput.Email,
                Organization = RegisterInput.Organization,
                Password = RegisterInput.Password
            };

            if (dataContext.Users.Any(x => x.Email == userModel.Email))
            {
                ViewBag.Message = "This email has already been registered in the system.";
                return View("Register");
            }

            try
            {
                var userToRegister = new Data.UserData()
                {
                    FirstName = userModel.FirstName,
                    LastName = userModel.LastName,
                    Email = userModel.Email,
                    Password = userModel.Password,
                    Organization = userModel.Organization
                };

                dataContext.Users.Add(userToRegister);
                dataContext.SaveChanges();
                ViewBag.Success = "The account has been registered successfully";


                return View("Register");
            }

            catch (Exception e)
            {
                logger.LogError(e.Message);
                ViewBag.Message = e.Message;
                throw;
            }
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
