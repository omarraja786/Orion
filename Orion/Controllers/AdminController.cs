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
using Orion.Data;

namespace Orion.Controllers
{
    [Authorize(Policy = "IsAdmin")]
    public class AdminController(ILogger<RegisterController> logger, IConfiguration configuration,
            IWebHostEnvironment environment, DataContext dataContext)
        : Controller
    {
        private readonly ILogger<RegisterController> _logger = logger;
        public IConfiguration Configuration { get; } = configuration;
        private IWebHostEnvironment Environment = environment;

        public IActionResult Admin()
        {
            ViewBag.UsersLoggedIn = dataContext.Users.Where(x => x.IsLoggedIn).ToList();
            ViewBag.UsersRegistered = dataContext.Users.ToList();
            return View();
        }

             


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
