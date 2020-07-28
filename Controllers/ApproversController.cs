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
        public async Task<IActionResult> Details(int? Caseid, string Userid)
        {
            if (Caseid == null || Userid == "")
            {
                return NotFound();
            }

            var approver = await _context.Approver
                .Include(a => a.Case)
                .Include(a => a.LocalUser)
                .Include(a => a.LocalGroup)
                .FirstOrDefaultAsync(m => m.CaseID == Caseid && m.LocalUserID == Userid);
            if (approver == null)
            {
                return NotFound();
            }

            return View(approver);
        }

        // GET: Approvers/Create
        public IActionResult Create()
        {
            ViewData["CaseID"] = new SelectList(_context.Case, "CaseID", "CaseID");
            ViewData["LocalUserID"] = new SelectList(_context.LocalUser, "LocalUserID", "LocalUserID");
            ViewData["LocalGroupID"] = new SelectList(_context.LocalGroup, "LocalGroupID", "LocalGroupID");
            return View();
        }

        // POST: Approvers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CaseID,LocalUserID,Approved,Order,LocalGroupID")] Approver approver)
        {
            if (ModelState.IsValid)
            {
                _context.Add(approver);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CaseID"] = new SelectList(_context.Case, "CaseID", "CaseID", approver.CaseID);
            ViewData["LocalUserID"] = new SelectList(_context.LocalUser, "LocalUserID", "LocalUserID", approver.LocalUserID);
            ViewData["LocalGroupID"] = new SelectList(_context.LocalGroup, "LocalGroupID", "LocalGroupID", approver.LocalGroupID);
            return View(approver);
        }

        // GET: Approvers/Edit/5
        public async Task<IActionResult> Edit(int? Caseid, string Userid)
        {
            if (Caseid == null || Userid == "")
            {
                return NotFound();
            }

            var approver = await _context.Approver.FindAsync(Caseid, Userid);
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
        public async Task<IActionResult> Edit(int Caseid, string Userid, [Bind("CaseID,LocalUserID,Approved,Order,LocalGroupID")] Approver approver)
        {
            if (Caseid != approver.CaseID || Userid != approver.LocalUserID)
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
        public async Task<IActionResult> Delete(int? Caseid, string Userid)
        {
            if (Caseid == null || Userid == "")
            {
                return NotFound();
            }

            var approver = await _context.Approver
                .Include(a => a.Case)
                .Include(a => a.LocalUser)
                .Include(a => a.LocalGroup)
                .FirstOrDefaultAsync(m => m.CaseID == Caseid && m.LocalUserID == Userid);
            if (approver == null)
            {
                return NotFound();
            }

            return View(approver);
        }

        // POST: Approvers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int Caseid, string Userid)
        {
            var approver = await _context.Approver.FindAsync(Caseid, Userid);
            _context.Approver.Remove(approver);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ApproverExists(int Caseid, string Userid)
        {
            return _context.Approver.Any(e => e.CaseID == Caseid && e.LocalUserID == Userid);
        }
    }
}
