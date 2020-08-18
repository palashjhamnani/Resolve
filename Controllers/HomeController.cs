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
        //[Authorize("Admin")]
        [AuthorizeForScopes(Scopes = new[] { Constants.ScopeUserRead })]
        public async Task<IActionResult> Index()
        {
            var ADemail = User.Identity.Name;
            //Console.WriteLine(ADemail);
            var LocalUserEmail = _context.LocalUser
                .FromSqlRaw("SELECT * FROM dbo.LocalUser where LocalUserID={0}", ADemail)
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
                var CreateUser = new LocalUser { LocalUserID = LocalUserAttributes["EmailID"],
                FirstName = LocalUserAttributes["FirstName"],
                LastName = LocalUserAttributes["LastName"],
                EmailID = LocalUserAttributes["NetID"]
                };
                _context.Add(CreateUser);
                EmailPreference e_pref = new EmailPreference
                {
                    LocalUserID = CreateUser.LocalUserID,
                    CaseCreation = true,
                    CaseAssignment = true,
                    GroupAssignment = true,
                    CommentCreation = true,
                    AttachmentCreation = true,
                    CaseProcessed = true,
                    CasesCreatedByUser = true,
                    CasesAssignedToUser = false,
                    CasesAssignedToUsersGroups = false
                };
                _context.Add(e_pref);
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
            }
            // Populate or Check validity of User's group membership

            var UsersLGroups = _context.UserGroup
                .Where(m => m.LocalUserID == ADemail)
                .ToList();
            HashSet<string> UsersLocalGroups = new HashSet<string>();
            foreach (var g in UsersLGroups)
            {
                UsersLocalGroups.Add(g.LocalGroupID);
            }
            HashSet<string> UsersRetrievedGroups = new HashSet<string>();
            foreach (var claim in User.Claims)
            {
                if (claim.Type == "groups")
                {
                    UsersRetrievedGroups.Add(claim.Value);
                }                
            }
            HashSet<string> AllLocalGroups = new HashSet<string>();
            var AllGroups = _context.LocalGroup.ToList();
            foreach (var g in AllGroups)
            {
                AllLocalGroups.Add(g.LocalGroupID);
            }
            HashSet<string> GroupsToBeAdded = new HashSet<string>();
            HashSet<string> GroupsToBeRemoved = new HashSet<string>();

            // Check for every groups that is associated with our application (Resolve)
            foreach (var group in AllLocalGroups)
            {
                if (UsersRetrievedGroups.Contains(group)){
                    if (UsersLocalGroups.Contains(group))
                    { }
                    else
                    {
                        GroupsToBeAdded.Add(group);
                    }
                }
                else
                {
                    if (UsersLocalGroups.Contains(group))
                    {
                        GroupsToBeRemoved.Add(group);
                    }
                    else { }
                }
            }

            foreach (var g in GroupsToBeAdded)
            {
                var test = new UserGroup { LocalGroupID = g, LocalUserID = ADemail};
                _context.Add(test);                
            }
            foreach (var g in GroupsToBeRemoved)
            {
                var test = _context.UserGroup.Single(b => b.LocalUserID == ADemail && b.LocalGroupID == g);
                _context.Remove(test);
            }
            await _context.SaveChangesAsync();

            // Cases created by the User(or on behalf of user), assigned to the User, and assigned to the groups to which the User belongs to
            var UCases = await _context.LocalUser
                // Created by the User
                .Include(s => s.Cases.Where(p => p.Processed == 0))
                    .ThenInclude(w => w.CaseType)
                    // Including Approvers
                    .Include(s => s.Cases.Where(p => p.Processed == 0))
                        .ThenInclude(w => w.Approvers)
                        .ThenInclude(w => w.LocalUser)
                    // Created on behalf of the User
                    .Include(s => s.OnBehalves.Where(p => p.Case.Processed == 0))
                        .ThenInclude(s => s.Case)
                        .ThenInclude(s => s.CaseType)
                    // Including Approvers
                    .Include(s => s.OnBehalves.Where(p => p.Case.Processed == 0))
                        .ThenInclude(s => s.Case)
                        .ThenInclude(s => s.Approvers)
                        .ThenInclude(w => w.LocalUser)
                // Assigned to the User
                .Include(q => q.CasesforApproval.Where(p => p.Case.Processed == 0 && p.Approved == 0))
                    .ThenInclude(q => q.Case)
                    .ThenInclude(q => q.CaseType)
                    // Including Case Creator
                    .Include(q => q.CasesforApproval.Where(p => p.Case.Processed == 0 && p.Approved == 0))
                        .ThenInclude(q => q.Case)
                        .ThenInclude(q => q.LocalUser)
                    // Including Approvers
                    .Include(q => q.CasesforApproval.Where(p => p.Case.Processed == 0 && p.Approved == 0))
                        .ThenInclude(q => q.Case)
                        .ThenInclude(q => q.Approvers)
                        .ThenInclude(w => w.LocalUser)
                // Assigned to the Groups to which the user belongs to
                .Include(e => e.UserGroups)
                        .ThenInclude(e => e.LocalGroup)
                        .ThenInclude(e => e.GroupCases.Where(p => p.Case.Processed == 0))
                        .ThenInclude(e => e.Case)
                        .ThenInclude(e => e.CaseType)
                    // Including Case Creator
                    .Include(e => e.UserGroups)
                            .ThenInclude(e => e.LocalGroup)
                            .ThenInclude(e => e.GroupCases.Where(p => p.Case.Processed == 0))
                            .ThenInclude(e => e.Case)
                            .ThenInclude(e => e.LocalUser)
                    // Including Approvers
                    .Include(e => e.UserGroups)
                            .ThenInclude(e => e.LocalGroup)
                            .ThenInclude(e => e.GroupCases.Where(p => p.Case.Processed == 0))
                            .ThenInclude(e => e.Case)
                            .ThenInclude(e => e.Approvers)
                            .ThenInclude(w => w.LocalUser)
                .Include(e => e.EmailPreference)
            .FirstOrDefaultAsync(m => m.LocalUserID == ADemail);
            int group_case_count = 0;
            foreach (var item in UCases.UserGroups)
            {
                foreach (var item2 in item.LocalGroup.GroupCases)
                {
                    group_case_count += 1;
                }
            }
            ViewData["group_case_count"] = group_case_count;
            return View(UCases);
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



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Search()
        {
            try
            {
                var ss = HttpContext.Request.Form["SearchString"];
                var SearchResults = from m in _context.Case
                                        select m;
                if (!String.IsNullOrEmpty(ss))
                {
                    SearchResults = SearchResults.Where(s => s.CaseCID.Contains(ss));
                    SearchResults = SearchResults.Include(p => p.CaseType);
                }
                    
                return View(await SearchResults.ToListAsync());
            }
            catch (Exception)
            {
                return RedirectToAction("Index");
            }
        }


        public async Task<IActionResult> ByYou(string? cases)
        {           
            var PastCases = await _context.LocalUser
            .Include(s => s.Cases.Where(p => p.Processed == 1))
                .ThenInclude(w => w.CaseType)
            .Include(s => s.OnBehalves.Where(p => p.Case.Processed == 1))
                .ThenInclude(w => w.Case).ThenInclude(q => q.CaseType)
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.LocalUserID == User.Identity.Name);
            return View(PastCases);          
        }

        public async Task<IActionResult> ToYou(string? cases)
        {
            var PastCases = await _context.LocalUser
            .Include(q => q.CasesforApproval.Where(p => p.Case.Processed == 1 || p.Approved != 0))
                .ThenInclude(q => q.Case)
                .ThenInclude(q => q.CaseType)
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.LocalUserID == User.Identity.Name);
            return View(PastCases);
        }

        public async Task<IActionResult> ToYourGroups()
        {            
            var PastCases = await _context.LocalUser
            .Include(e => e.UserGroups)
                    .ThenInclude(e => e.LocalGroup)
                    .ThenInclude(e => e.GroupCases.Where(p => p.Case.Processed == 1))
                    .ThenInclude(e => e.Case)
                    .ThenInclude(e => e.CaseType)
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.LocalUserID == User.Identity.Name);
            return View(PastCases);          
        }


        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        public FileResult Download(string id)
        {
            string filename = $"resolve_user_guide.pdf";
            if (id == "dg")
            {
                filename = $"resolve_dev_guide.pdf";
            }
            else
                if (id == "ag")
            {
                filename = $"resolve_admin_guide.pdf";
            }
            var filepath = $"Documentation/{filename}";
            byte[] fileBytes = System.IO.File.ReadAllBytes(filepath);
            return File(fileBytes, "application/x-msdownload", filename);
        }

    }
}