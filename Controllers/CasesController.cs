using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Resolve.Data;
using Resolve.Helpers;
using Resolve.Models;
using Resolve.ViewModels;
using Microsoft.AspNetCore.Hosting;
using static Microsoft.AspNetCore.Hosting.IWebHostEnvironment;
using Microsoft.AspNetCore.Http;
using Microsoft.Identity.Web;
using Microsoft.Extensions.Configuration;

namespace Resolve.Controllers
{
    public class CasesController : Controller
    {
        private readonly IConfiguration _config;
        private readonly ResolveCaseContext _context;
        private readonly IHostingEnvironment hostingEnvironment;

        public CasesController(ResolveCaseContext context, IHostingEnvironment hostingEnvironment, IConfiguration config)
        {
            _context = context;
            this.hostingEnvironment = hostingEnvironment;
            _config = config;
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
            var cid = HttpContext.Request.Form["CaseID"];
            //caseComment.CaseID = cid;
            //HttpContext.Request.Form["UserName"];
            try
            {                
                if (ModelState.IsValid)
                {
                    _context.Add(caseComment);
                    // Adding audit log
                    //var audit = new CaseAudit { AuditLog = "Comment Created", CaseID = caseComment.CaseID, LocalUserID = User.Identity.Name };
                    //_context.Add(audit);
                    await _context.SaveChangesAsync();
                    try
                    {
                        var case_c = _context.Case
                        .Single(a => a.CaseID == caseComment.CaseID);
                        var luser = _context.LocalUser
                        .Single(a => a.LocalUserID == caseComment.LocalUserID);
                        var case_creator =_context.LocalUser
                        .Single(a => a.LocalUserID == case_c.LocalUserID);
                        //Send Notification
                        //Collect all current approvers
                        var approvers = _context.Approver
                        .Where(b => b.CaseID == caseComment.CaseID)
                        .Include(c => c.LocalUser)
                        .ToList();
                        //Add them to a list
                        List<LocalUser> to_addresses = new List<LocalUser>();
                        foreach (var item in approvers)
                        {
                            // If the user is an approver, check if the case has already been processed by this user, if yes, no need to notify
                            if (item.Approved == 0)
                            {
                                to_addresses.Add(item.LocalUser);
                            }                            
                        }
                        // Add the Case Creator to the list as well
                        to_addresses.Add(case_creator);
                        // If case is on behalf of someone, add that user as well
                        var on_behalf = _context.OnBehalf
                            .Where(b => b.CaseID == caseComment.CaseID)
                            .ToList();
                        if (on_behalf.Count != 0)
                        {
                            to_addresses.Add(on_behalf[0].LocalUser);
                        }
                        // Remove all duplicate users from the to_addresses list
                        ICollection<LocalUser> withoutDuplicates = new HashSet<LocalUser>(to_addresses);
                        foreach (var item in withoutDuplicates)
                        {
                            // Not sending notif to comment creator
                            if (item.LocalUserID != caseComment.LocalUserID)
                            {                                
                                var u_pref = _context.EmailPreference
                                .Single(b => b.LocalUserID == item.LocalUserID);
                                // Check if the email recievers have subscribed to alerts
                                if (u_pref.CommentCreation == true)
                                {
                                    var notif_result = new Notifications(_config).SendEmail(case_id: caseComment.CaseID.ToString(),
                                        case_cid: case_c.CaseCID, luser: item, template: "comment", comment_by: luser, comment_on_case: caseComment.Comment);                                                                   
                                }                                                                
                            }
                        }
                        return RedirectToAction("Details", new { id = cid, err_message = "Comment posted and notified all stakeholders!" });
                    }
                    catch
                    {
                        return RedirectToAction("Details", new { id = cid, err_message = "Notification to all stakeholders could not be sent at this time!" });
                    }                    
                }
            }
            catch (Exception)
            {
                return RedirectToAction("Details", new { id = cid, err_message = "Comment could not be posted at this time!" });
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
            public async Task<IActionResult> Details(int? id, string err_message)
            {
            if (id == null)
            {
                return NotFound();
            }
            ViewData["err_message"] = err_message;
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
            var current_u_groups = _context.UserGroup
                .Where(b => b.LocalUserID == u_name).ToList();
            List<string> current_user_groups = new List<string>();
            foreach (var item in current_u_groups)
            {
                current_user_groups.Add(item.LocalGroupID);
            }
            ViewData["current_user_groups"] = current_user_groups;
            var @case = await _context.Case
                .Include(s => s.CaseType)
                .Include(u => u.LocalUser)
                .Include(p => p.CaseComments).ThenInclude(e => e.LocalUser)
                .Include(a => a.CaseAudits)
                .Include(a => a.CaseAttachments).ThenInclude(e => e.LocalUser)
                .Include(a => a.GroupAssignments).ThenInclude(e => e.LocalGroup)
                .Include(a => a.Approvers).ThenInclude(e => e.LocalUser)
                .Include(a => a.Approvers).ThenInclude(e => e.LocalGroup)
                .Include(a => a.OnBehalves).ThenInclude(e => e.LocalUser)
                .Include(p => p.SampleCaseType)
                .Include(p => p.SAR4)
                .Include(p => p.HRServiceFaculty)
                .Include(p => p.HRServiceGradStudent)
                .Include(p => p.HRServiceStaff)
                 .Include(p => p.HRServiceScholarResident)
                .Include(p => p.PerioLimitedCare)
                .Include(p => p.HiringAffiliateFaculty)
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
        public async Task<IActionResult> Create([Bind("CaseID,LocalUserID,OnBehalfOf,CaseCreationTimestamp,Description")] Case @case)
        {
            string CTypeTitle = HttpContext.Request.Form["CTypeTitle"].ToString();            
            var CTypeMiddle = _context.CaseType.Single(p => p.CaseTypeTitle == CTypeTitle);
            int CTypeID = CTypeMiddle.CaseTypeID;
            @case.CaseTypeID = CTypeID;
            @case.CaseStatus = "Pending";

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
                // Ordered processing by approvers as per designated order
                if (CType.Hierarchical_Approval == true)
                {
                    // Assigning the first approver
                    var CTypeGroup = _context.CaseTypeGroup
                    .Single(b => b.CaseTypeID == @case.CaseTypeID && b.Order == 1);
                    var app_group = _context.LocalGroup
                            .Single(b => b.LocalGroupID == CTypeGroup.LocalGroupID);
                    var app_luser = _context.LocalUser
                        .Single(b => b.LocalUserID == app_group.LocalUserID);                    
                    var approver_preference = _context.EmailPreference
                        .Single(b => b.LocalUserID == app_group.LocalUserID);
                    var appr_add = new Approver { CaseID = cid, LocalUserID = app_group.LocalUserID, Approved = 0, Order = 1, LocalGroupID = CTypeGroup.LocalGroupID };
                    _context.Add(appr_add);
                    //Send Notification to the approver
                    if (approver_preference.CaseAssignment == true)
                    {
                        var notif_result = new Notifications(_config).SendEmail(related_case: @case, case_id: @case.CaseID.ToString(), case_cid: @case.CaseCID, luser: app_luser, template: "assignment");
                        if (notif_result != "Sent")
                        {
                            Console.WriteLine("Could not send assignment notification!");
                        }
                    }
                    // Assign the associated group
                    var grp_add = new GroupAssignment { CaseID = cid, LocalGroupID = CTypeGroup.LocalGroupID };
                    _context.Add(grp_add);
                    await _context.SaveChangesAsync();
                    //Send Notification to the group members to which the case is currently assigned to since Case Type is hierarchical
                    var group_members = _context.UserGroup.Where(p => p.LocalGroupID == CTypeGroup.LocalGroupID).ToList();
                    foreach (var item in group_members)
                    {
                        // We don't want to notify the approver again, but only the other group members
                        if (item.LocalUserID != app_luser.LocalUserID)
                        {
                            // Check group members preference
                            var group_user = _context.LocalUser.Single(p => p.LocalUserID == item.LocalUserID);
                            var email_preference = _context.EmailPreference
                            .Single(b => b.LocalUserID == item.LocalUserID);
                            if (email_preference.GroupAssignment == true)
                            {
                                var notif_result = new Notifications(_config).SendEmail(case_id: @case.CaseID.ToString(), case_cid: @case.CaseCID, luser: group_user, template: "g_assignment", group_name: app_group.GroupName);
                                if (notif_result != "Sent")
                                {
                                    Console.WriteLine("Could not send group assignment notification!");
                                }
                            }
                        }
                    }
                }
                // Parallel processing by all approvers
                else
                {                    
                    foreach (var item in CTypeGroups)
                    {                      
                        var app = _context.LocalGroup
                            .Single(b => b.LocalGroupID == item.LocalGroupID);
                        var approver_luser = _context.LocalUser
                            .Single(b => b.LocalUserID == app.LocalUserID);
                        var approver_preferences = _context.EmailPreference
                            .Single(b => b.LocalUserID == app.LocalUserID);
                        // Adding the default approver from this group
                        var app_add = new Approver { CaseID = cid, LocalUserID = app.LocalUserID, Approved = 0, Order = Convert.ToInt32(item.Order), LocalGroupID = item.LocalGroupID };
                        _context.Add(app_add);
                        //Send Notification to this approver
                        if (approver_preferences.CaseAssignment == true)
                        {
                            var notif_result = new Notifications(_config).SendEmail(related_case: @case, case_id: @case.CaseID.ToString(), case_cid: @case.CaseCID, luser: approver_luser, template: "assignment");
                            if (notif_result != "Sent")
                            {
                                Console.WriteLine("Could not send assignment notification!");
                            }
                        }
                        // Assigning Case to this group
                        var grp_add = new GroupAssignment { CaseID = cid, LocalGroupID = item.LocalGroupID };
                        _context.Add(grp_add);
                        // Sending notification to this group's members
                        var g_members = _context.UserGroup.Where(p => p.LocalGroupID == item.LocalGroupID).ToList();
                        foreach (var member in g_members)
                        {
                            // We don't want to notify the approver again, but only the other group members
                            if (member.LocalUserID != approver_luser.LocalUserID)
                            {
                                // Check group members preference
                                var g_user = _context.LocalUser.Single(p => p.LocalUserID == member.LocalUserID);
                                var e_preference = _context.EmailPreference
                                .Single(b => b.LocalUserID == member.LocalUserID);
                                if (e_preference.GroupAssignment == true)
                                {
                                    var notif_result = new Notifications(_config).SendEmail(case_id: @case.CaseID.ToString(), case_cid: @case.CaseCID, luser: g_user, template: "g_assignment", group_name: app.GroupName);
                                    if (notif_result != "Sent")
                                    {
                                        Console.WriteLine("Could not send group assignment notification!");
                                    }
                                }
                            }
                        }
                    }
                    await _context.SaveChangesAsync();
                }
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
        public async Task<IActionResult> Edit(int id, [Bind("CaseID,LocalUserID,OnBehalfOf,CaseStatus,CaseCreationTimestamp,CaseTypeID,Description,Processed")] Case @case)
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


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UserEdit()
        {
            string desc = HttpContext.Request.Form["CaseDesc"].ToString();
            int cid = Convert.ToInt32(HttpContext.Request.Form["CaseID"]);            
            try
            {
                var case_to_edit = await _context.Case.FindAsync(cid);
                case_to_edit.Description = desc;
                _context.Update(case_to_edit);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", new { id = cid, err_message = "Case details have been updated successfully!" });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CaseExists(cid))
                {
                    return NotFound();
                }
                else
                {
                    return RedirectToAction("Details", new { id = cid, err_message = "Case detail cannot be updated at this time. Please contact administrator." });
                }
            }            
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
            var final_comment = HttpContext.Request.Form["FinalComment"];
            int int_cid = Convert.ToInt32(cid);
            string details_arg = "";
            var caseForApproval = await _context.Approver.FindAsync(int_cid, User.Identity.Name);
            var caseProcessed = await _context.Case.FindAsync(int_cid);
            var caseCreator = await _context.LocalUser.FindAsync(caseProcessed.LocalUserID);
            var CType = await _context.CaseType.FindAsync(caseProcessed.CaseTypeID);
            //var CTypeGroups = _context.CaseTypeGroup
            //       .Where(b => b.CaseTypeID == CType.CaseTypeID);
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
                    // Adding final approval comment
                    if (final_comment != "")
                    {
                        var f_comment = new CaseComment { Comment = "Approval Comment: " + final_comment, CaseID = int_cid, LocalUserID = User.Identity.Name };
                        _context.Add(f_comment);
                    }
                    await _context.SaveChangesAsync();
                    // Add next approver if Case Type is hierarchical
                    if (CType.Hierarchical_Approval == true)
                    {
                        // Check if next approver exists
                        var CTGroup = await _context.CaseTypeGroup
                            .Where(b => b.CaseTypeID == CType.CaseTypeID && b.Order == caseForApproval.Order + 1)
                            .ToListAsync();
                        if (CTGroup.Count != 0)
                        {                            
                            var app_group = _context.LocalGroup
                                .Single(b => b.LocalGroupID == CTGroup[0].LocalGroupID);
                            var app_luser = _context.LocalUser
                                .Single(b => b.LocalUserID == app_group.LocalUserID);                            
                            var approver_preference = _context.EmailPreference
                                .Single(b => b.LocalUserID == app_group.LocalUserID);
                            var appr_add = new Approver { CaseID = int_cid, LocalUserID = app_group.LocalUserID, Approved = 0, Order = Convert.ToInt32(CTGroup[0].Order), LocalGroupID = CTGroup[0].LocalGroupID };
                            _context.Add(appr_add);
                            var audit_log = new CaseAudit { AuditLog = "Case has been assigned to "+app_luser.FirstName+" "+app_luser.LastName+" as the next approver in hierarchy.", CaseID = int_cid, LocalUserID = User.Identity.Name };
                            _context.Add(audit_log);
                            //Send Notification
                            if (approver_preference.CaseAssignment == true)
                            {
                                var notif_result = new Notifications(_config).SendEmail(related_case: caseProcessed, case_id: cid, case_cid: caseProcessed.CaseCID, luser: app_luser, template: "assignment");
                                if (notif_result != "Sent")
                                {
                                    Console.WriteLine("Could not send assignment notification!");
                                }
                            }
                            var grp_add = new GroupAssignment { CaseID = int_cid, LocalGroupID = CTGroup[0].LocalGroupID };
                            _context.Add(grp_add);
                            await _context.SaveChangesAsync();
                            //Send Notification to the group members to which the case is currently assigned to since Case Type is hierarchical
                            var group_members = _context.UserGroup.Where(p => p.LocalGroupID == app_group.LocalGroupID).ToList();
                            foreach (var item in group_members)
                            {
                                // We don't want to notify the approver again, but only the other group members
                                if (item.LocalUserID != app_luser.LocalUserID)
                                {
                                    // Check group members preference
                                    var group_user = _context.LocalUser.Single(p => p.LocalUserID == item.LocalUserID);
                                    var email_preference = _context.EmailPreference
                                    .Single(b => b.LocalUserID == item.LocalUserID);
                                    if (email_preference.GroupAssignment == true)
                                    {
                                        var notif_result = new Notifications(_config).SendEmail(case_id: cid, case_cid: caseProcessed.CaseCID, luser: group_user, template: "g_assignment", group_name: app_group.GroupName);
                                        if (notif_result != "Sent")
                                        {
                                            Console.WriteLine("Could not send group assignment notification!");
                                        }
                                    }
                                }
                            }
                        }
                    }                    
                    details_arg = "The case has been approved successfully!";
                }
                else
                    if (process_value == "Reopen")
                {
                    // Manage the hierarchical scenario, i.e. remove all approvers ahead of the current approver who reopened the case
                    if (CType.Hierarchical_Approval == true)
                    {
                        // Collect all approvers ahead of current approver
                        var approvers_ahead = await _context.Approver
                            .Where(p => p.CaseID == int_cid && p.Order > caseForApproval.Order).ToListAsync();
                        var groups_ahead = await _context.CaseTypeGroup
                            .Where(p => p.CaseTypeID == CType.CaseTypeID && p.Order > caseForApproval.Order).ToListAsync();
                        if (approvers_ahead.Count != 0 || groups_ahead.Count != 0)
                        {
                            foreach (var item in approvers_ahead)
                            {
                                _context.Approver.Remove(item);
                            }
                            foreach (var item in groups_ahead)
                            {
                                var @group = await _context.GroupAssignment.FindAsync(int_cid, item.LocalGroupID);
                                _context.GroupAssignment.Remove(@group);
                            }
                            await _context.SaveChangesAsync();
                            var audit_remove = new CaseAudit { AuditLog = "Groups and Approvers ahead of current approver in hierarchy have been removed.", CaseID = int_cid, LocalUserID = User.Identity.Name };
                            _context.Add(audit_remove);
                        }                        
                    }
                    caseForApproval.Approved = 0;
                    _context.Update(caseForApproval);
                    var audit = new CaseAudit { AuditLog = "Case Reopened and marked as not-processed. Case put in pending status.", CaseID = int_cid, LocalUserID = User.Identity.Name };
                    _context.Add(audit);
                    // Mark Case as Not-Processed
                    caseProcessed.Processed = 0;
                    _context.Update(caseProcessed);
                    // Adding final reopen comment
                    if (final_comment != "")
                    {
                        var f_comment = new CaseComment { Comment = "Reopen Comment: " + final_comment, CaseID = int_cid, LocalUserID = User.Identity.Name };
                        _context.Add(f_comment);
                    }
                    await _context.SaveChangesAsync();
                    details_arg = "The case has been reopened successfully!";
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
                    // Adding final rejection comment
                    if (final_comment != "")
                    {
                        var f_comment = new CaseComment { Comment = "Rejection Comment: " + final_comment, CaseID = int_cid, LocalUserID = User.Identity.Name };
                        _context.Add(f_comment);
                    }
                    await _context.SaveChangesAsync();
                    details_arg = "The case has been rejected successfully!";
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
                    var audit = new CaseAudit { AuditLog = "Case marked as processed", CaseID = int_cid, LocalUserID = User.Identity.Name };
                    _context.Add(audit);
                    await _context.SaveChangesAsync();
                    // Send notification
                    var pref = _context.EmailPreference.Single(p => p.LocalUserID == caseCreator.LocalUserID);
                    if (pref.CaseProcessed == true)
                    {
                        var notif_result = new Notifications(_config).SendEmail(case_id: cid, case_cid: caseProcessed.CaseCID, luser: caseCreator, template: "approved");
                        if (notif_result != "Sent")
                        {
                            Console.WriteLine("Could not send approval notification to the case creator!");
                        }
                    }
                    // Sending notification to On Behalf user if it exists
                    var on_behalf = _context.OnBehalf
                        .Where(b => b.CaseID == int_cid)
                        .ToList();
                    if (on_behalf.Count != 0)
                    {
                        var behalf_user = _context.LocalUser.Single(p => p.LocalUserID == on_behalf[0].LocalUserID);
                        var behalf_pref = _context.EmailPreference.Single(p => p.LocalUserID == behalf_user.LocalUserID);
                        if (behalf_pref.CaseProcessed == true)
                        {
                            var notif_result = new Notifications(_config).SendEmail(case_id: cid, case_cid: caseProcessed.CaseCID, luser: behalf_user, template: "approved");
                            if (notif_result != "Sent")
                            {
                                Console.WriteLine("Could not send approval notification to the user on whose behalf the case was created!");
                            }
                        }
                    }
                }        
                else
                if (reject_count != 0)
                    {
                        // Mark Case as Processed and Rejected
                        caseProcessed.CaseStatus = "Rejected";
                        _context.Update(caseProcessed);
                        caseProcessed.Processed = 1;
                        _context.Update(caseProcessed);
                        var audit = new CaseAudit { AuditLog = "Case marked as processed", CaseID = int_cid, LocalUserID = User.Identity.Name };
                        _context.Add(audit);
                        await _context.SaveChangesAsync();
                        // Send notification of Case Rejection
                        var pref = _context.EmailPreference.Single(p => p.LocalUserID == caseCreator.LocalUserID);
                        if (pref.CaseProcessed == true)
                        {
                            var notif_result = new Notifications(_config).SendEmail(case_id: cid, case_cid: caseProcessed.CaseCID, luser: caseCreator, template: "rejected");
                            if (notif_result != "Sent")
                            {
                                Console.WriteLine("Could not send rejection notification to the case creator!");
                            }
                        }
                        // Sending notification to On Behalf user if it exists                    
                        var on_behalf = _context.OnBehalf
                        .Where(b => b.CaseID == int_cid)
                        .ToList();
                        if (on_behalf.Count != 0)
                        {
                            var behalf_user = _context.LocalUser.Single(p => p.LocalUserID == on_behalf[0].LocalUserID);
                            var behalf_pref = _context.EmailPreference.Single(p => p.LocalUserID == behalf_user.LocalUserID);
                            if (behalf_pref.CaseProcessed == true)
                            {
                                var notif_result = new Notifications(_config).SendEmail(case_id: cid, case_cid: caseProcessed.CaseCID, luser: behalf_user, template: "rejected");
                                if (notif_result != "Sent")
                                {
                                    Console.WriteLine("Could not send rejection notification to the user on whose behalf the case was created!");
                                }
                            }
                        }
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
                return RedirectToAction("Details", new { id = cid, err_message = details_arg });
                //return RedirectToAction(nameof(Details));
            }
            catch (Exception)
            {
                ViewData["Approved"] = "Error";
            }

            return RedirectToAction("Details", new { id = cid, err_message = "Could not process the request at this time" });
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AdminProcess(int id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var cid = HttpContext.Request.Form["CaseID"];
            var process_value = HttpContext.Request.Form["ProcessValue"];
            var final_comment = HttpContext.Request.Form["FinalComment"];
            int int_cid = Convert.ToInt32(cid);
            string details_arg = "";
            var all_approvers = await _context.Approver.Where(p => p.CaseID == int_cid).ToListAsync();
            var CaseToProcess = await _context.Case.FindAsync(int_cid);
            var CType = await _context.CaseType.FindAsync(CaseToProcess.CaseTypeID);
            try
            {
                foreach (var item in all_approvers)
                {
                    if (process_value == "Approve")
                    {
                        item.Approved = 1;
                        _context.Update(item);
                    }
                    else
                        if (process_value == "Reopen")
                    {
                        item.Approved = 0;
                        _context.Update(item);
                    }
                    else
                        if (process_value == "Reject")
                    {
                        item.Approved = -1;
                        _context.Update(item);
                    }
                }
                await _context.SaveChangesAsync();
                if (process_value == "Approve")
                {
                    // Mark Case as Approved
                    CaseToProcess.CaseStatus = "Approved";
                    CaseToProcess.Processed = 1;
                    _context.Update(CaseToProcess);
                    var audit = new CaseAudit { AuditLog = "Case Approved by Admin. Case marked as processed.", CaseID = int_cid, LocalUserID = User.Identity.Name };
                    _context.Add(audit);
                    // Adding final approval comment
                    if (final_comment != "")
                    {
                        var f_comment = new CaseComment { Comment = "Approval Comment: " + final_comment, CaseID = int_cid, LocalUserID = User.Identity.Name };
                        _context.Add(f_comment);
                    }
                    await _context.SaveChangesAsync();
                    details_arg = "The case has been approved successfully!";
                }
                else
                    if (process_value == "Reopen")
                {
                    // Mark Case in Pending
                    CaseToProcess.CaseStatus = "Pending";
                    CaseToProcess.Processed = 0;
                    _context.Update(CaseToProcess);
                    var audit = new CaseAudit { AuditLog = "Case Reopened by Admin. Case still in Pending status.", CaseID = int_cid, LocalUserID = User.Identity.Name };
                    _context.Add(audit);
                    // Adding final reopening comment
                    if (final_comment != "")
                    {
                        var f_comment = new CaseComment { Comment = "Reopening Comment: " + final_comment, CaseID = int_cid, LocalUserID = User.Identity.Name };
                        _context.Add(f_comment);
                    }
                    await _context.SaveChangesAsync();
                    details_arg = "The case has been reopened successfully!";
                }
                else
                    if (process_value == "Reject")
                {
                    CaseToProcess.CaseStatus = "Rejected";
                    CaseToProcess.Processed = 1;
                    _context.Update(CaseToProcess);
                    var audit = new CaseAudit { AuditLog = "Case Rejected by Admin. Case marked as processed.", CaseID = int_cid, LocalUserID = User.Identity.Name };
                    _context.Add(audit);
                    // Adding final rejection comment
                    if (final_comment != "")
                    {
                        var f_comment = new CaseComment { Comment = "Rejection Comment: " + final_comment, CaseID = int_cid, LocalUserID = User.Identity.Name };
                        _context.Add(f_comment);
                    }
                    await _context.SaveChangesAsync();
                    details_arg = "The case has been rejected successfully!";
                }
                ViewData["Approved"] = "Success";
                return RedirectToAction("Details", new { id = cid, err_message = details_arg });
            }
            catch (Exception)
            {
                ViewData["Approved"] = "Error";
            }

            return RedirectToAction("Details", new { id = cid, err_message = "The request could not be completed at this time!" });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelCase(int id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var cid = HttpContext.Request.Form["CaseID"];
            var process_value = HttpContext.Request.Form["ProcessValue"];
            var final_comment = HttpContext.Request.Form["FinalComment"];
            int int_cid = Convert.ToInt32(cid);
            string details_arg = "";
            var all_approvers = await _context.Approver.Where(p => p.CaseID == int_cid).ToListAsync();
            var CaseToProcess = await _context.Case.FindAsync(int_cid);
            var CType = await _context.CaseType.FindAsync(CaseToProcess.CaseTypeID);
            try
            {
                foreach (var item in all_approvers)
                {
                    if (process_value == "Cancel")
                    {
                        item.Approved = 3;
                        _context.Update(item);
                    }                    
                }
                await _context.SaveChangesAsync();
                if (process_value == "Cancel")
                {
                    // Mark Case as Cancelled
                    CaseToProcess.CaseStatus = "Cancelled";
                    CaseToProcess.Processed = 1;
                    _context.Update(CaseToProcess);
                    var audit = new CaseAudit { AuditLog = "Case has been marked as Cancelled.", CaseID = int_cid, LocalUserID = User.Identity.Name };
                    _context.Add(audit);
                    // Adding final approval comment
                    if (final_comment != "")
                    {
                        var f_comment = new CaseComment { Comment = "Cancellation Comment: " + final_comment, CaseID = int_cid, LocalUserID = User.Identity.Name };
                        _context.Add(f_comment);
                    }
                    await _context.SaveChangesAsync();
                    details_arg = "The case has been cancelled successfully!";
                }
                ViewData["Approved"] = "Success";
                return RedirectToAction("Details", new { id = cid, err_message = details_arg });
            }
            catch (Exception)
            {
                ViewData["Approved"] = "Error";
            }

            return RedirectToAction("Details", new { id = cid, err_message = "Cancel case request could not be completed at this time!" });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SwapApprover(int id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var cid = HttpContext.Request.Form["CaseID"];
            var gid = HttpContext.Request.Form["GrpID"].ToString();
            var old_approver = HttpContext.Request.Form["OldApprover"].ToString();
            var final_comment = HttpContext.Request.Form["FinalComment"];
            int int_cid = Convert.ToInt32(cid);
            string details_arg = "";
            try
            {
                var approver_old = await _context.Approver.FindAsync(int_cid, old_approver);
                _context.Approver.Remove(approver_old);
                await _context.SaveChangesAsync();
                var swapped_approver = new Approver { LocalUserID = User.Identity.Name, CaseID = int_cid, LocalGroupID = gid };
                _context.Add(swapped_approver);
                var audit = new CaseAudit { AuditLog = "Approver "+old_approver+" has been swapped with "+User.Identity.Name, CaseID = int_cid, LocalUserID = User.Identity.Name };
                _context.Add(audit);
                // Adding final approval comment
                if (final_comment != "")
                {
                    var f_comment = new CaseComment { Comment = "Assignment Swap Comment: " + final_comment, CaseID = int_cid, LocalUserID = User.Identity.Name };
                    _context.Add(f_comment);
                }
                await _context.SaveChangesAsync();
                details_arg = "You have been assigned as an approver to this case!";
                
                ViewData["Approved"] = "Success";
                return RedirectToAction("Details", new { id = cid, err_message = details_arg });
            }
            catch (Exception)
            {
                ViewData["Approved"] = "Error";
            }

            return RedirectToAction("Details", new { id = cid, err_message = "Swap approver request could not be completed at this time!" });
        }

        // GET
        public IActionResult Reassign(int id, string gid)
        {
            var group_members = _context.UserGroup.Where(p => p.LocalGroupID == gid).ToList();
            List<string> g_members = new List<string>();
            foreach (var item in group_members)
            {
                if (item.LocalUserID != User.Identity.Name)
                {
                    g_members.Add(item.LocalUserID);
                }                
            }
            ViewData["group_members"] = new SelectList(g_members);
            ViewData["group_name"] = _context.LocalGroup.Find(gid).GroupName;
            ViewData["Case_CID"] = _context.Case.Find(id).CaseCID;
            ViewData["Case_ID"] = id;
            ViewData["Group_ID"] = gid;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reassign()
        {
            var cid = HttpContext.Request.Form["Case_ID"];
            var ccid = HttpContext.Request.Form["Case_CID"];
            var gid = HttpContext.Request.Form["Group_ID"].ToString();
            var final_comment = HttpContext.Request.Form["FinalComment"];
            var reassign_to = HttpContext.Request.Form["reassign_to"].ToString();
            int int_cid = Convert.ToInt32(cid);
            string details_arg = "";
            try
            {
                // Removing current approver
                var approver_old = await _context.Approver.FindAsync(int_cid, User.Identity.Name);
                _context.Approver.Remove(approver_old);
                await _context.SaveChangesAsync();
                // Adding requested new approver from the same group
                var reassigned_approver = new Approver { LocalUserID = reassign_to, CaseID = int_cid, LocalGroupID = gid };
                _context.Add(reassigned_approver);
                var audit = new CaseAudit { AuditLog = "Approver " + User.Identity.Name + " has been swapped with " + reassign_to, CaseID = int_cid, LocalUserID = User.Identity.Name };
                _context.Add(audit);
                // Adding final approval comment
                if (final_comment != "")
                {
                    var f_comment = new CaseComment { Comment = "Reassignment Comment: " + final_comment, CaseID = int_cid, LocalUserID = User.Identity.Name };
                    _context.Add(f_comment);
                }
                await _context.SaveChangesAsync();
                details_arg = "Case successfully reassigned to " + reassign_to + "! ";
                // Sending Notification
                var pref = _context.EmailPreference.Single(p => p.LocalUserID == reassign_to);
                var new_user = _context.LocalUser.Single(p => p.LocalUserID == reassign_to);
                var rel_case = _context.Case.Single(p => p.CaseID == int_cid);
                if (pref.CaseAssignment == true)
                {
                    var notif_result = new Notifications(_config).SendEmail(related_case: rel_case, case_id: cid, case_cid: ccid, luser: new_user, template: "assignment");
                    if (notif_result != "Sent")
                    {
                        details_arg = details_arg + "Could not notify new approver!";
                    }
                    else
                        if (notif_result == "Sent")
                    {
                        details_arg = details_arg + "Notified new approver!";
                    }
                }
                return RedirectToAction("Details", new { id = cid, err_message = details_arg });
            }
            catch (Exception)
            {
                return RedirectToAction("Details", new { id = cid, err_message = "Reassign approver request could not be completed at this time!" });
            }            
        }

    }
}
