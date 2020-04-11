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
    public class CaseAuditsController : Controller
    {
        private readonly ResolveCaseContext _context;

        public CaseAuditsController(ResolveCaseContext context)
        {
            _context = context;
        }

        // GET: CaseAudits
        public async Task<IActionResult> Index()
        {
            var resolveCaseContext = _context.CaseAudit.Include(c => c.Case).Include(c => c.LocalUser);
            return View(await resolveCaseContext.ToListAsync());
        }

        // GET: CaseAudits/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var caseAudit = await _context.CaseAudit
                .Include(c => c.Case)
                .Include(c => c.LocalUser)
                .FirstOrDefaultAsync(m => m.CaseAuditID == id);
            if (caseAudit == null)
            {
                return NotFound();
            }

            return View(caseAudit);
        }

        // GET: CaseAudits/Create
        public IActionResult Create()
        {
            ViewData["CaseID"] = new SelectList(_context.Case, "CaseID", "CaseID");
            ViewData["LocalUserID"] = new SelectList(_context.LocalUser, "LocalUserID", "LocalUserID");
            return View();
        }

        // POST: CaseAudits/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CaseAuditID,AuditTimestamp,AuditLog,CaseID,LocalUserID")] CaseAudit caseAudit)
        {
            if (ModelState.IsValid)
            {
                _context.Add(caseAudit);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CaseID"] = new SelectList(_context.Case, "CaseID", "CaseID", caseAudit.CaseID);
            ViewData["LocalUserID"] = new SelectList(_context.LocalUser, "LocalUserID", "LocalUserID", caseAudit.LocalUserID);
            return View(caseAudit);
        }

        // GET: CaseAudits/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var caseAudit = await _context.CaseAudit.FindAsync(id);
            if (caseAudit == null)
            {
                return NotFound();
            }
            ViewData["CaseID"] = new SelectList(_context.Case, "CaseID", "CaseID", caseAudit.CaseID);
            ViewData["LocalUserID"] = new SelectList(_context.LocalUser, "LocalUserID", "LocalUserID", caseAudit.LocalUserID);
            return View(caseAudit);
        }

        // POST: CaseAudits/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CaseAuditID,AuditTimestamp,AuditLog,CaseID,LocalUserID")] CaseAudit caseAudit)
        {
            if (id != caseAudit.CaseAuditID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(caseAudit);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CaseAuditExists(caseAudit.CaseAuditID))
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
            ViewData["CaseID"] = new SelectList(_context.Case, "CaseID", "CaseID", caseAudit.CaseID);
            ViewData["LocalUserID"] = new SelectList(_context.LocalUser, "LocalUserID", "LocalUserID", caseAudit.LocalUserID);
            return View(caseAudit);
        }

        // GET: CaseAudits/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var caseAudit = await _context.CaseAudit
                .Include(c => c.Case)
                .Include(c => c.LocalUser)
                .FirstOrDefaultAsync(m => m.CaseAuditID == id);
            if (caseAudit == null)
            {
                return NotFound();
            }

            return View(caseAudit);
        }

        // POST: CaseAudits/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var caseAudit = await _context.CaseAudit.FindAsync(id);
            _context.CaseAudit.Remove(caseAudit);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CaseAuditExists(int id)
        {
            return _context.CaseAudit.Any(e => e.CaseAuditID == id);
        }
    }
}
