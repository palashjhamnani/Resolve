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
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Resolve.Models;
using Resolve.Data;

namespace Resolve.Controllers
{
    [Authorize]
    public class HomeController : Controller


    {
        
        readonly ITokenAcquisition tokenAcquisition;
        readonly WebOptions webOptions;
        private readonly ResolveCaseContext _context;

        public HomeController(ITokenAcquisition tokenAcquisition, IOptions<WebOptions> webOptionValue, ResolveCaseContext context)
        {
            this.tokenAcquisition = tokenAcquisition;
            this.webOptions = webOptionValue.Value;
            _context = context;
        }


        /*
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        */

        [AuthorizeForScopes(Scopes = new[] { Constants.ScopeUserRead })]
        public async Task<IActionResult> Index()
        {
            var ADemail = User.Identity.Name;
            //Console.WriteLine(ADemail);
            var LocalUserEmail = _context.LocalUser
                .FromSqlRaw("SELECT * FROM dbo.LocalUser where EmailID={0}", ADemail)
                .ToList();
            var c = LocalUserEmail.Count;

            if (c == 0)
            {
                //Console.WriteLine("Creating Local User");
                Graph::GraphServiceClient graphClient = GetGraphServiceClient(new[] { Constants.ScopeUserRead });
                var me = await graphClient.Me.Request().GetAsync() as Microsoft.Graph.User;
                var properties = me.GetType().GetProperties();
                IDictionary<string, string> LocalUserAttributes = new Dictionary<string, string>();

                foreach (var child in properties)
                {
                    object value = child.GetValue(me);
                    string stringRepresentation;
                    if (value is string)
                    {
                        stringRepresentation = value?.ToString();
                    }
                    else
                    {
                        stringRepresentation = "";
                    }
                    
                    if (child.Name == "OnPremisesSamAccountName")
                    {                        
                        LocalUserAttributes.Add("NetID", stringRepresentation);
                    }
                    else
                        if (child.Name == "GivenName")
                    {
                        LocalUserAttributes.Add("FirstName", stringRepresentation);
                    }
                    else
                        if (child.Name == "Surname")
                    {
                        LocalUserAttributes.Add("LastName", stringRepresentation);
                    }
                    else
                        if (child.Name == "Mail")
                    {
                        LocalUserAttributes.Add("EmailID", stringRepresentation);
                    }
                        
                }
                //ViewData["Me"] = me;              
                var CreateUser = new LocalUser { LocalUserID = LocalUserAttributes["NetID"],
                FirstName = LocalUserAttributes["FirstName"],
                LastName = LocalUserAttributes["LastName"],
                EmailID = LocalUserAttributes["EmailID"]
                };
                _context.Add(CreateUser);
                await _context.SaveChangesAsync();
                /*
                Console.WriteLine("Created");
                Console.WriteLine(LocalUserAttributes["NetID"]);
                Console.WriteLine(LocalUserAttributes["FirstName"]);
                Console.WriteLine(LocalUserAttributes["LastName"]);
                Console.WriteLine(LocalUserAttributes["EmailID"]);
                */
            }
            else
            {
                //Console.WriteLine("User Already Exists!");
                Console.WriteLine(LocalUserEmail[0].LocalUserID);
            }
            
            /*                       
            var @User = await _context.User
            .Include(s => s.U)
            .Include(u => u.User)
            .FirstOrDefaultAsync(m => m. == id);
            var DBemail = "test";
            */

            // test
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
            Console.WriteLine("Privacy");
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
