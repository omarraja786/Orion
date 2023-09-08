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
    public class RegisterController : Controller
    {
        private readonly ILogger<RegisterController> _logger;
        public IConfiguration Configuration { get; }
        private IWebHostEnvironment Environment;
        private DataContext _dataContext;

        public RegisterController(ILogger<RegisterController> logger, IConfiguration configuration, IWebHostEnvironment environment, DataContext dataContext)
        {
            _logger = logger;
            Configuration = configuration;
            Environment = environment;
            _dataContext = dataContext;
        }

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

            if (_dataContext.Users.Any(x => x.Email == userModel.Email))
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

                _dataContext.Users.Add(userToRegister);
                _dataContext.SaveChanges();
                ViewBag.Success = "The account has been registered successfully";


                return View("Register");
            }

            catch (Exception e)
            {
                _logger.LogError(e.Message);
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
