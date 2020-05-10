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
                if (approved == -1)
            {
                ViewData["Approved"] = "RejectSuccess";
            }
            var @case = await _context.Case
                .Include(s => s.CaseType)
                .Include(u => u.LocalUser)
                .Include(p => p.CaseComments).ThenInclude(e => e.LocalUser)
                .Include(a => a.CaseAudits)
                .Include(a => a.CaseAttachments).ThenInclude(e => e.LocalUser)
                .Include(a => a.GroupAssignments).ThenInclude(e => e.LocalGroup)
                .Include(a => a.Approvers).ThenInclude(e => e.LocalUser)
                .Include(p => p.SampleCaseType)
                .Include(p => p.Sample2)
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
            ViewData["CaseTypeID"] = new SelectList(_context.CaseType, "CaseTypeID", "CaseTypeID");
            //ViewData["LocalUserID"] = new SelectList(_context.LocalUser, "LocalUserID", "LocalUserID");
                       
            return View();
        }

        

        // POST: Cases/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CaseID,LocalUserID,OnBehalfOf,CaseStatus,CaseCreationTimestamp,CaseTypeID")] Case @case)
        {

            if (ModelState.IsValid)
            {
                _context.Add(@case);
                await _context.SaveChangesAsync();
                var cid = @case.CaseID;
                // Adding audit log
                var audit = new CaseAudit {AuditLog = "Case Created", CaseID = cid, LocalUserID = User.Identity.Name};
                _context.Add(audit);
                // Creating helper variables
                var redirectFunction = _context.CaseType
                    .Single(b => b.CaseTypeID == @case.CaseTypeID);
                var approver_group = redirectFunction.LocalGroupID;
                var approver = _context.LocalGroup
                    .Single(b => b.LocalGroupID == approver_group);
                var approver_user = approver.LocalUserID;
                // Adding default Approver assignment
                var approver_add = new Approver {CaseID = cid, LocalUserID = approver_user, Approved = 0, Order = 1};
                _context.Add(approver_add);
                // Adding Group Assignment
                var group_add = new GroupAssignment { CaseID = cid, LocalGroupID = approver_group};
                _context.Add(group_add);
                await _context.SaveChangesAsync();
                
                var redirectFunctionName = redirectFunction.CaseTypeTitle;
                return RedirectToAction(redirectFunctionName, "CaseSpecificDetails", new { id = cid });
                //return RedirectToAction("Index", "Home");
            }
            ViewData["CaseTypeID"] = new SelectList(_context.CaseType, "CaseTypeID", "CaseTypeID", @case.CaseTypeID);
            //ViewData["LocalUserID"] = new SelectList(_context.LocalUser, "LocalUserID", "LocalUserID", @case.LocalUserID);
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
        public IActionResult Approve(int? id)
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Approve(int id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var cid = HttpContext.Request.Form["CaseID"];
            int int_cid = Convert.ToInt32(cid);
            var caseForApproval = await _context.Approver.FindAsync(int_cid, User.Identity.Name);
            if (caseForApproval == null)
            {
                return NotFound();
            }
            try
            {
                caseForApproval.Approved = 1;
                _context.Update(caseForApproval);
                var audit = new CaseAudit { AuditLog = "Case Approved", CaseID = int_cid, LocalUserID = User.Identity.Name };
                _context.Add(audit);
                await _context.SaveChangesAsync();
                ViewData["Approved"] = "Success";
                //var cid = HttpContext.Request.Form["CaseID"];
                return RedirectToAction("Details", new { id = cid, approved = 1 });
                //return RedirectToAction(nameof(Details));
            }
            catch (Exception)
            {
                ViewData["Approved"] = "Error";
            }

            return RedirectToAction("Details", new { id = cid, approved = 0 });
        }

        // Approve
        public IActionResult Reject(int? id)
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reject(int id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var cid = HttpContext.Request.Form["CaseID"];
            int int_cid = Convert.ToInt32(cid);
            var caseForReject = await _context.Approver.FindAsync(int_cid, User.Identity.Name);
            if (caseForReject == null)
            {
                return NotFound();
            }
            try
            {
                caseForReject.Approved = -1;
                _context.Update(caseForReject);
                var audit = new CaseAudit { AuditLog = "Case Rejected", CaseID = int_cid, LocalUserID = User.Identity.Name };
                _context.Add(audit);
                await _context.SaveChangesAsync();                
                //var cid = HttpContext.Request.Form["CaseID"];
                return RedirectToAction("Details", new { id = cid, approved = -1 });
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
