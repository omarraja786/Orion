using Orion.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using Orion.Data;

namespace Orion.Controllers
{
    [Authorize]
    public class HomeController(ILogger<HomeController> logger, DataContext dataContext) : Controller
    {
        public IActionResult Index()
        {
            ViewBag.UsersRegistered = dataContext.Users.Count();
            ViewBag.UsersLoggedIn = dataContext.Users.Count(x => x.IsLoggedIn);
            ViewBag.ProjectsStarted = dataContext.ProjectData.Count(x => x.ProjectStatus);
            ViewBag.ProjectsPending = dataContext.ProjectData.Count(x => !x.ProjectStatus && !x.ProjectCompleted);
            ViewBag.ProjectsComplete = dataContext.ProjectData.Count(x => x.ProjectCompleted);
            var projects = dataContext.ProjectData.ToList();

            var completedProjects = projects.Where(p => p.ProjectCompleted == true).ToList();
            var inProgressProjects = projects.Where(p => p.ProjectCompleted == false && p.ProjectTasksCompleted > 0).ToList();
            var notStartedProjects = projects.Where(p => p.ProjectCompleted == false && p.ProjectTasksCompleted == 0).ToList();

            ViewBag.CompletedProjects = completedProjects;
            ViewBag.InProgressProjects = inProgressProjects;
            ViewBag.NotStartedProjects = notStartedProjects;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(string name, string position, string area, string company, string industry, string size,
            string valuePropInput, string product, string myName, string myTitle, string myCompany)
        {
            try
            {
                var client = new HttpClient();

                var formModel = new FormModel
                {
                    Name = name,
                    Position = position,
                    Area = area,
                    Company = company,
                    Industry = industry,
                    Size = size,
                    ValuePropInput = valuePropInput,
                    Product = product,
                    MyName = myName,
                    MyTitle = myTitle,
                    MyCompany = myCompany
                    
                };
                var jsonFormModel = JsonConvert.SerializeObject(formModel);
                var httpContent = new StringContent(jsonFormModel, Encoding.UTF8, "application/json");
                var response = await client.PostAsync($"http://gpt:5000/gpt", httpContent);
                var responseString = await response.Content.ReadAsStringAsync();
                ViewBag.Status = responseString;
                var htmlResponseString = FormatEmailText(responseString);
                var testDecodeHtml = System.Net.WebUtility.HtmlDecode(htmlResponseString);
                ViewBag.HtmlStatus = testDecodeHtml;

                if (response.IsSuccessStatusCode && responseString.Contains($"Hi {formModel.Name}"))
                {
                    ViewBag.RenderMyScript = true;
                }
                

                return View();
            }
            catch (Exception e)
            {
                throw;
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(int? statusCode)
        {
            if (statusCode == 404)
            {
                return View("Error404");
            }

            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }



        public string FormatEmailText(string email)
        {
            try
            {
                
                var emailText = JsonConvert.DeserializeObject<FormatEmail>(email);

                //Split each line into an array and add bold tags to Hi and thank you (first and last line)
                if (emailText != null)
                {
                    var emailSplit = emailText.Body.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

                    emailSplit[0] = "<b>" + emailSplit[0] + "</b>";
                    //emailSplit[^1] = "<b>" + emailSplit[^1] + "</b>"; Not adding this anymore because there is no "regards/thankyou"

                    //Add HTML line breaks to each line
                    string[] newStringArray = emailSplit
                        .Select(s => s + "<br>")
                        .ToArray();

                    // String array back to a string
                    emailText.Body = string.Concat(newStringArray);
                }

                //Serialize to Json String and Return
                var emailTextString = JsonConvert.SerializeObject(emailText);
                var unescapedString = Regex.Unescape(emailText.Body);


                // post -> this json back to front end plugin
                return unescapedString;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return ex.Message;
            }
        }
    }
}
