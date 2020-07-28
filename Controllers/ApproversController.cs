using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Resolve.Data;
using Resolve.Models;

namespace Resolve.Controllers
{
    public class ApproversController : Controller
    {
        private readonly ResolveCaseContext _context;

        public ApproversController(ResolveCaseContext context)
        {
            _context = context;
        }

        // GET: Approvers
        public async Task<IActionResult> Index()
        {
            var resolveCaseContext = _context.Approver.Include(a => a.Case).Include(a => a.LocalUser).Include(a => a.LocalGroup);
            return View(await resolveCaseContext.ToListAsync());
        }

        // GET: Approvers/Details/5
        public async Task<IActionResult> Details(int? CaseID, string LocalUserID)
        {
            if (CaseID == null || LocalUserID == "")
            {
                return NotFound();
            }

            var approver = await _context.Approver
                .Include(a => a.Case)
                .Include(a => a.LocalUser)
                .Include(a => a.LocalGroup)
                .FirstOrDefaultAsync(m => m.CaseID == CaseID && m.LocalUserID == LocalUserID);
            if (approver == null)
            {
                return NotFound();
            }

            return View(approver);
        }

        // GET: Approvers/Create
        public IActionResult Create()
        {
            ViewData["CaseCID"] = new SelectList(_context.Case, "CaseCID", "CaseCID");
            ViewData["LocalUserID"] = new SelectList(_context.LocalUser, "LocalUserID", "LocalUserID");
            ViewData["GroupName"] = new SelectList(_context.LocalGroup, "GroupName", "GroupName");
            return View();
        }

        // POST: Approvers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LocalUserID,Approved,Order")] Approver approver)
        {
            var GName = HttpContext.Request.Form["GroupName"].ToString();
            var Cid = HttpContext.Request.Form["CaseCID"].ToString();
            var LGroup = _context.LocalGroup.Single(p => p.GroupName == GName);
            approver.LocalGroupID = LGroup.LocalGroupID;
            var Case = _context.Case.Single(p => p.CaseCID == Cid);
            approver.CaseID = Case.CaseID;
            if (ModelState.IsValid)
            {
                _context.Add(approver);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CaseCID"] = new SelectList(_context.Case, "CaseCID", "CaseCID");
            ViewData["LocalUserID"] = new SelectList(_context.LocalUser, "LocalUserID", "LocalUserID", approver.LocalUserID);
            ViewData["GroupName"] = new SelectList(_context.LocalGroup, "GroupName", "GroupName");
            return View(approver);
        }

        // GET: Approvers/Edit/5
        public async Task<IActionResult> Edit(int? CaseID, string LocalUserID)
        {
            if (CaseID == null || LocalUserID == "")
            {
                return NotFound();
            }

            var approver = await _context.Approver.FindAsync(CaseID, LocalUserID);
            if (approver == null)
            {
                return NotFound();
            }
            ViewData["CaseID"] = new SelectList(_context.Case, "CaseID", "CaseID", approver.CaseID);
            ViewData["LocalUserID"] = new SelectList(_context.LocalUser, "LocalUserID", "LocalUserID", approver.LocalUserID);
            ViewData["LocalGroupID"] = new SelectList(_context.LocalGroup, "LocalGroupID", "LocalGroupID", approver.LocalGroupID);
            return View(approver);
        }

        // POST: Approvers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int CaseID, string LocalUserID, [Bind("CaseID,LocalUserID,Approved,Order,LocalGroupID")] Approver approver)
        {
            if (CaseID != approver.CaseID || LocalUserID != approver.LocalUserID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(approver);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ApproverExists(approver.CaseID, approver.LocalUserID))
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
            ViewData["CaseID"] = new SelectList(_context.Case, "CaseID", "CaseID", approver.CaseID);
            ViewData["LocalUserID"] = new SelectList(_context.LocalUser, "LocalUserID", "LocalUserID", approver.LocalUserID);
            ViewData["LocalGroupID"] = new SelectList(_context.LocalGroup, "LocalGroupID", "LocalGroupID", approver.LocalGroupID);
            return View(approver);
        }

        // GET: Approvers/Delete/5
        public async Task<IActionResult> Delete(int? CaseID, string LocalUserID)
        {
            if (CaseID == null || LocalUserID == "")
            {
                return NotFound();
            }

            var approver = await _context.Approver
                .Include(a => a.Case)
                .Include(a => a.LocalUser)
                .Include(a => a.LocalGroup)
                .FirstOrDefaultAsync(m => m.CaseID == CaseID && m.LocalUserID == LocalUserID);
            if (approver == null)
            {
                return NotFound();
            }

            return View(approver);
        }

        // POST: Approvers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int CaseID, string LocalUserID)
        {
            var approver = await _context.Approver.FindAsync(CaseID, LocalUserID);
            _context.Approver.Remove(approver);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ApproverExists(int CaseID, string LocalUserID)
        {
            return _context.Approver.Any(e => e.CaseID == CaseID && e.LocalUserID == LocalUserID);
        }
    }
}
