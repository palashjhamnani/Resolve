using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Resolve.Data;
using Resolve.Models;
using Resolve.ViewModels;
using Microsoft.AspNetCore.Hosting;
using static Microsoft.AspNetCore.Hosting.IWebHostEnvironment;
using Microsoft.AspNetCore.Http;
using Microsoft.Identity.Web;

namespace Resolve.Controllers
{
    public class CasesController : Controller
    {
        private readonly ResolveCaseContext _context;
        private readonly IHostingEnvironment hostingEnvironment;

        public CasesController(ResolveCaseContext context, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            this.hostingEnvironment = hostingEnvironment;
        }

        


        // GET: CaseComments/Create
        public IActionResult CreateCommment(int? id)
        {
            //ViewData["CaseID"] = new SelectList(_context.Case, "CaseID", "CaseID");
            //ViewData["UserID"] = new SelectList(_context.User, "UserID", "UserID");
            return View();
        }

        // POST: CaseComments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateComment(int id, [Bind("CaseCommentID,Comment,CommentTimestamp,CaseID,LocalUserID")] CaseComment caseComment)
        {
            //caseComment.CaseID = cid;
            //HttpContext.Request.Form["UserName"];
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(caseComment);
                    await _context.SaveChangesAsync();
                    var cid = HttpContext.Request.Form["CaseID"];
                    return RedirectToAction("Details", new { id = cid });
                    //return RedirectToAction(nameof(Details));

                }
            }
            catch (Exception)
            {

                Console.WriteLine("Error!");
            }

            return View(caseComment);
        }




        // GET: Cases
        public async Task<IActionResult> Index()
        {
            var resolveCaseContext = _context.Case.Include(s => s.CaseType).Include(u => u.LocalUser);
            //var resolveCaseContext = _context.Case;
            return View(await resolveCaseContext.ToListAsync());
        }



            // GET: Cases/Details/5
            public async Task<IActionResult> Details(int? id, int? approved)
            {
            if (id == null)
            {
                return NotFound();
            }
            ViewData["Approved"] = "NotSet";
            if (approved == 1)
            {
                ViewData["Approved"] = "Success";
            }
            else
                if (approved == 0)
                {
                    ViewData["Approved"] = "Failed";
                }
            else
                if (approved == 2)
            {
                ViewData["Approved"] = "Reopened";
            }
            else
                if (approved == 3)
            {
                ViewData["Approved"] = "AttachDeleted";
            }
            else
                if (approved == -1)
            {
                ViewData["Approved"] = "RejectSuccess";
            }
            bool is_user_admin = false;
            var user_name = User.GetDisplayName();
            string u_name = user_name.ToString();
            var u_admin = _context.UserGroup
                .Where(b => b.LocalUserID == u_name && b.LocalGroupID == "773d56cf-4ede-494e-8823-1956116230f1");
            if (u_admin.Count() != 0)
            {
                is_user_admin = true;
            }
            ViewData["is_user_admin"] = is_user_admin;
            var @case = await _context.Case
                .Include(s => s.CaseType)
                .Include(u => u.LocalUser)
                .Include(p => p.CaseComments).ThenInclude(e => e.LocalUser)
                .Include(a => a.CaseAudits)
                .Include(a => a.CaseAttachments).ThenInclude(e => e.LocalUser)
                .Include(a => a.GroupAssignments).ThenInclude(e => e.LocalGroup)
                .Include(a => a.Approvers).ThenInclude(e => e.LocalUser)
                .Include(a => a.OnBehalves).ThenInclude(e => e.LocalUser)
                .Include(p => p.SampleCaseType)
                .Include(p => p.Sample2)
                .Include(p => p.HRServiceStaff)
                .Include(p => p.HRServiceGradStudent)
                .Include(p => p.HRServiceFaculty)
                .FirstOrDefaultAsync(m => m.CaseID == id);
            if (@case == null)
            {
                return NotFound();
            }
            return View(@case);
        }



        // GET: Cases/Create
        public IActionResult Create()
        {          
            //ViewData["LocalUserID"] = LUserID[0];
            ViewData["CaseTypeTitle"] = new SelectList(_context.CaseType, "CaseTypeTitle", "CaseTypeTitle");
            ViewData["OnBehalfUser"] = new SelectList(_context.LocalUser, "LocalUserID", "LocalUserID");
                       
            return View();
        }
               

        // POST: Cases/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CaseID,LocalUserID,OnBehalfOf,CaseCreationTimestamp")] Case @case)
        {
            string CTypeTitle = HttpContext.Request.Form["CTypeTitle"].ToString();            
            var CTypeMiddle = _context.CaseType.Single(p => p.CaseTypeTitle == CTypeTitle);
            int CTypeID = CTypeMiddle.CaseTypeID;
            @case.CaseTypeID = CTypeID;
            @case.CaseStatus = "Open";

            if (ModelState.IsValid)
            {                
                _context.Add(@case);
                await _context.SaveChangesAsync();
                var cid = @case.CaseID;
                // Adding On Behalf user
                if (@case.OnBehalfOf == true)
                {
                    string OnBehalfUser = HttpContext.Request.Form["OnBehalfUser"].ToString();
                    var behalf_add = new OnBehalf { CaseID = cid, LocalUserID = OnBehalfUser };
                    _context.Add(behalf_add);
                }
                // Adding audit log
                var audit = new CaseAudit {AuditLog = "Case Created", CaseID = cid, LocalUserID = User.Identity.Name};
                _context.Add(audit);
                // Creating helper variables
                var CType = _context.CaseType
                    .Single(b => b.CaseTypeID == @case.CaseTypeID);
                var CTypeGroups = _context.CaseTypeGroup
                    .Where(b => b.CaseTypeID == @case.CaseTypeID);
                foreach (var item in CTypeGroups)
                {                    
                    var app = _context.LocalGroup
                        .Single(b => b.LocalGroupID == item.LocalGroupID);
                    var app_add = new Approver { CaseID = cid, LocalUserID = app.LocalUserID, Approved = 0, Order = Convert.ToInt32(item.Order) };
                    _context.Add(app_add);
                    var grp_add = new GroupAssignment { CaseID = cid, LocalGroupID = item.LocalGroupID };
                    _context.Add(grp_add);
                }


                await _context.SaveChangesAsync();
                
                var redirectFunctionName = CType.CaseTypeTitle;
                return RedirectToAction("Create", redirectFunctionName, new { id = cid, area = "CaseSpecificDetails" });

            }
            ViewData["CaseTypeID"] = new SelectList(_context.CaseType, "CaseTypeTitle", "CaseTypeTitle", @case.CaseTypeID);
            ViewData["OnBehalfUser"] = new SelectList(_context.LocalUser, "LocalUserID", "LocalUserID");
            return View(@case);
        }


        // GET: Cases/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @case = await _context.Case.FindAsync(id);
            if (@case == null)
            {
                return NotFound();
            }
            ViewData["CaseTypeID"] = new SelectList(_context.CaseType, "CaseTypeID", "CaseTypeID", @case.CaseTypeID);
            ViewData["LocalUserID"] = new SelectList(_context.LocalUser, "LocalUserID", "LocalUserID", @case.LocalUserID);
            return View(@case);
        }

        // POST: Cases/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CaseID,LocalUserID,OnBehalfOf,CaseStatus,CaseCreationTimestamp,CaseTypeID")] Case @case)
        {
            if (id != @case.CaseID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(@case);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CaseExists(@case.CaseID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CaseTypeID"] = new SelectList(_context.CaseType, "CaseTypeID", "CaseTypeID", @case.CaseTypeID);
            ViewData["LocalUserID"] = new SelectList(_context.LocalUser, "LocalUserID", "LocalUserID", @case.LocalUserID);
            return View(@case);
        }

        // GET: Cases/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @case = await _context.Case
                .Include(s => s.CaseType)
                .Include(u => u.LocalUser)
                .FirstOrDefaultAsync(m => m.CaseID == id);
            if (@case == null)
            {
                return NotFound();
            }

            return View(@case);
        }

        // POST: Cases/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var @case = await _context.Case.FindAsync(id);
            _context.Case.Remove(@case);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CaseExists(int id)
        {
            return _context.Case.Any(e => e.CaseID == id);
        }


        // Approve
        public IActionResult Process(int? id)
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Process(int id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var cid = HttpContext.Request.Form["CaseID"];
            var process_value = HttpContext.Request.Form["ProcessValue"];
            int int_cid = Convert.ToInt32(cid);
            int details_arg = 0;
            var caseForApproval = await _context.Approver.FindAsync(int_cid, User.Identity.Name);
            var caseProcessed = await _context.Case.FindAsync(int_cid);
            if (caseForApproval == null)
            {
                return NotFound();
            }
            try
            {
                if (process_value == "Approve")
                {
                    caseForApproval.Approved = 1;
                    _context.Update(caseForApproval);
                    var audit = new CaseAudit { AuditLog = "Case Approved", CaseID = int_cid, LocalUserID = User.Identity.Name };
                    _context.Add(audit);
                    await _context.SaveChangesAsync();
                    details_arg = 1;
                }
                else
                    if (process_value == "Reopen")
                {
                    caseForApproval.Approved = 0;
                    _context.Update(caseForApproval);
                    var audit = new CaseAudit { AuditLog = "Case Reopened and marked as not-processed. Case put in pending status.", CaseID = int_cid, LocalUserID = User.Identity.Name };
                    _context.Add(audit);
                    // Mark Case as Not-Processed                    
                    caseProcessed.Processed = 0;
                    _context.Update(caseProcessed);                    
                    await _context.SaveChangesAsync();
                    details_arg = 2;
                }
                else
                    if (process_value == "Reject")
                {
                    caseForApproval.Approved = -1;
                    _context.Update(caseForApproval);
                    caseProcessed.CaseStatus = "Rejected";
                    _context.Update(caseProcessed);
                    var audit = new CaseAudit { AuditLog = "Case Rejected", CaseID = int_cid, LocalUserID = User.Identity.Name };
                    _context.Add(audit);
                    await _context.SaveChangesAsync();
                    details_arg = -1;
                }
                
                // Mark Case as Processed if all approvers have taken action or if any of the approver has rejected the case

                var ApproversForCase = await _context.Approver
                    .Where(p => p.CaseID == int_cid)
                    .ToListAsync();
                int approved_count = 0;                
                int reject_count = 0;
                foreach (var item in ApproversForCase)
                {
                    if (item.Approved == 1)
                    {
                        approved_count += 1;                        
                    }
                    else
                        if (item.Approved == -1)
                    {                        
                        reject_count += 1;
                    }
                }
                int processed_count = approved_count + reject_count;
                int total_processors = ApproversForCase.Count;
                int processors_remaining = total_processors - processed_count;
                if (processors_remaining == 0 && reject_count == 0)
                    {
                    // Mark Case as Processed and Approved
                    caseProcessed.CaseStatus = "Approved";
                    _context.Update(caseProcessed);
                    caseProcessed.Processed = 1;
                    _context.Update(caseProcessed);
                    var audit = new CaseAudit { AuditLog = "Case Processed", CaseID = int_cid, LocalUserID = User.Identity.Name };
                    _context.Add(audit);
                    await _context.SaveChangesAsync();
                }        
                else
                if (reject_count != 0)
                    {
                        // Mark Case as Processed and Rejected
                        caseProcessed.CaseStatus = "Rejected";
                        _context.Update(caseProcessed);
                        caseProcessed.Processed = 1;
                        _context.Update(caseProcessed);
                        var audit = new CaseAudit { AuditLog = "Case Processed", CaseID = int_cid, LocalUserID = User.Identity.Name };
                        _context.Add(audit);
                        await _context.SaveChangesAsync();
                }
                else
                {
                    // Mark Case in Pending
                    caseProcessed.CaseStatus = "Pending";
                    _context.Update(caseProcessed);                    
                    var audit = new CaseAudit { AuditLog = "Case still in Pending status.", CaseID = int_cid, LocalUserID = User.Identity.Name };
                    _context.Add(audit);
                    await _context.SaveChangesAsync();
                }
                        


                ViewData["Approved"] = "Success";
                //var cid = HttpContext.Request.Form["CaseID"];
                return RedirectToAction("Details", new { id = cid, approved = details_arg });
                //return RedirectToAction(nameof(Details));
            }
            catch (Exception)
            {
                ViewData["Approved"] = "Error";
            }

            return RedirectToAction("Details", new { id = cid, approved = 0 });
        }

        


    }
}
