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

namespace Orion.Controllers
{
    [Authorize(Policy = "IsAdmin")]
    public class RegisterController : Controller
    {
        private readonly ILogger<RegisterController> _logger;
        public IConfiguration Configuration { get; }
        private IWebHostEnvironment Environment;

        public RegisterController(ILogger<RegisterController> logger, IConfiguration configuration, IWebHostEnvironment environment)
        {
            _logger = logger;
            Configuration = configuration;
            Environment = environment;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(IFormFile postedFile)
        {
            if (postedFile == null)
            {
                ViewBag.Message = "Failed to upload file";
                return View();
            }

            try
            {
                //Create a Folder.
                var path = Path.Combine(this.Environment.WebRootPath, "Uploads");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                //Save the uploaded Excel file.
                var fileName = Path.GetFileName(postedFile.FileName);
                var filePath = Path.Combine(path, fileName);
                using (FileStream stream = new FileStream(filePath, FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                }

                //Read the connection string for the Excel file.
                var conString = Configuration.GetConnectionString("ExcelConString");
                var dt = new DataTable();
                conString = string.Format(conString, filePath);

#pragma warning disable CA1416 // Validate platform compatibility
                using (var connExcel = new OleDbConnection(conString))
                {
                    using (var cmdExcel = new OleDbCommand())
                    {
                        using (var odaExcel = new OleDbDataAdapter())
                        {
                            cmdExcel.Connection = connExcel;

                            //Get the name of First Sheet.
                            connExcel.Open();
                            var dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                            var sheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
                            connExcel.Close();

                            //Read Data from First Sheet.
                            connExcel.Open();
                            cmdExcel.CommandText = "SELECT * From [" + sheetName + "]";
                            odaExcel.SelectCommand = cmdExcel;
                            odaExcel.Fill(dt);
                            connExcel.Close();
                        }
                    }
                }
#pragma warning restore CA1416 // Validate platform compatibility

                //Insert the Data read from the Excel file to Database Table.
                conString = Configuration.GetConnectionString("DefaultConnection");
                using (var con = new SqlConnection(conString))
                {
                    using (var sqlBulkCopy = new SqlBulkCopy(con))
                    {
                        //Set the database table name.
                        sqlBulkCopy.DestinationTableName = "dbo.Users";

                        //[OPTIONAL]: Map the Excel columns with that of the database table.
                        sqlBulkCopy.ColumnMappings.Add("Email", "Email");
                        sqlBulkCopy.ColumnMappings.Add("Password", "Password");
                        sqlBulkCopy.ColumnMappings.Add("FirstName", "FirstName");
                        sqlBulkCopy.ColumnMappings.Add("LastName", "LastName");
                        sqlBulkCopy.ColumnMappings.Add("Organization", "Organization");
                        sqlBulkCopy.ColumnMappings.Add("IsAdmin", "IsAdmin");

                        con.Open();
                        sqlBulkCopy.WriteToServer(dt);
                        con.Close();
                        ViewBag.Success = "Users saved to database!";
                    }
                }

                return View();
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
