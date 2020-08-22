using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Resolve.Data;
using Resolve.Models;

namespace Resolve.Areas.CaseSpecificDetails.Controllers
{
    [Area("CaseSpecificDetails")]
    //[Route(nameof(CaseTypes) + "/[controller]")]
    public class SampleCaseTypeController : Controller
    {
        
        private readonly ResolveCaseContext _context;

        public SampleCaseTypeController(ResolveCaseContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        // GET: Cases/CreateCaseTypeData
        public IActionResult Create(int id)
        {
            return View();
        }
        // POST: Cases/CreateCaseTypeData
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int id, [Bind("CaseDescription, EmployeeName, Salary")] SampleCaseType samplecasetype)
        {
            if (ModelState.IsValid)
            {
                samplecasetype.CaseID = id;                
                _context.Add(samplecasetype);
                await _context.SaveChangesAsync();
                var cid = id;
                return RedirectToAction("Details", "Cases", new { id = cid, area = "" });
            }
            return View(samplecasetype);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var editCase = _context.SampleCaseType.Find(id);
            if (editCase == null)
            {
                return NotFound();
            }            
            return View(editCase);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CaseID, CaseDescription, EmployeeName, Salary")] SampleCaseType samplecasetype)

        {
            if (id != samplecasetype.CaseID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    IQueryable<SampleCaseType> beforeCases = _context.SampleCaseType.Where(c => c.CaseID == id).AsNoTracking<SampleCaseType>();
                    SampleCaseType beforeCase = beforeCases.FirstOrDefault();
                    if (beforeCase == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        // Creating an audit log
                        var audit = new CaseAudit { AuditLog = "Case Specific Details Edited", CaseID = id, LocalUserID = User.Identity.Name };
                        _context.Add(audit);
                        await _context.SaveChangesAsync();
                        // Adding old details to tracking
                        var old_details = new SampleCaseTypeTracking
                        {
                            Status = "old",
                            CaseAuditID = audit.CaseAuditID,
                            CaseID = beforeCase.CaseID,
                            CaseDescription = beforeCase.CaseDescription,
                            EmployeeName = beforeCase.EmployeeName,
                            Salary = beforeCase.Salary
                        };
                        _context.Add(old_details);
                        // Adding current details to tracking
                        var new_details = new SampleCaseTypeTracking
                        {
                            Status = "new",
                            CaseAuditID = audit.CaseAuditID,
                            CaseID = samplecasetype.CaseID,
                            CaseDescription = samplecasetype.CaseDescription,
                            EmployeeName = samplecasetype.EmployeeName,
                            Salary = samplecasetype.Salary
                        };
                        _context.Add(new_details);
                        // Adding current details to actual Case Type entity
                        _context.Update(samplecasetype);
                        await _context.SaveChangesAsync();
                    }                    
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SampleCaseTypeExists(samplecasetype.CaseID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                var cid = id;
                return RedirectToAction("Details", "Cases", new { id = cid, area = "" });
            }            
            return View(samplecasetype);
        }

        public IActionResult EditLog(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            try
            {
                var logs = _context.SampleCaseTypeTracking.Where(p => p.CaseAuditID == id).ToList();
                ViewData["Logs"] = logs;
                return View();
            }
            catch (Exception)
            {
                var cid = Convert.ToInt32(id);
                return RedirectToAction("Details", "Cases", new { id = cid, area = "", err_message = "Can not fetch the edit log details currently!" });
            }
            
        }

        private bool SampleCaseTypeExists(int id)
        {
            return _context.CaseAudit.Any(e => e.CaseAuditID == id);
        }

    }
}