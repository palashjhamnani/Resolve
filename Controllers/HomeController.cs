using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Graph = Microsoft.Graph;
using Microsoft.Identity.Web;
using Resolve.Services;
using Resolve.Infrastructure;
using Resolve.Models;

namespace Resolve.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {

        readonly ITokenAcquisition tokenAcquisition;
        readonly WebOptions webOptions;

        public HomeController(ITokenAcquisition tokenAcquisition, IOptions<WebOptions> webOptionValue)
        {
            this.tokenAcquisition = tokenAcquisition;
            this.webOptions = webOptionValue.Value;
        }


        /*
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        */

        public IActionResult Index()
        {
            var email = User.Identity.Name;
            Console.WriteLine(email);
            var prop = Profile().GetType().GetProperties();            
            
            foreach (var child in prop)
            {
                Console.WriteLine(child.Name);
                /*
                object value = child.GetValue(prop);
                if (child.Name == "GivenName")
                {
                    string fname = value?.ToString();
                    ViewData["Fname"] = fname;
                }
                */
                
            }
            
            

            return View();
        }


        [AuthorizeForScopes(Scopes = new[] { Constants.ScopeUserRead })]
        public async Task<IActionResult> Profile()
        {
            // Initialize the GraphServiceClient. 
            Graph::GraphServiceClient graphClient = GetGraphServiceClient(new[] { Constants.ScopeUserRead });

            var me = await graphClient.Me.Request().GetAsync();
            ViewData["Me"] = me;

            try
            {
                // Get user photo
                using (var photoStream = await graphClient.Me.Photo.Content.Request().GetAsync())
                {
                    byte[] photoByte = ((MemoryStream)photoStream).ToArray();
                    ViewData["Photo"] = Convert.ToBase64String(photoByte);
                }
            }
            catch (System.Exception)
            {
                ViewData["Photo"] = null;
            }

            return View();
        }

        private Graph::GraphServiceClient GetGraphServiceClient(string[] scopes)
        {
            return GraphServiceClientFactory.GetAuthenticatedGraphClient(async () =>
            {
                string result = await tokenAcquisition.GetAccessTokenForUserAsync(scopes);
                return result;
            }, webOptions.GraphApiUrl);
        }

        public IActionResult Privacy()
        {
            return View();
        }




        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
