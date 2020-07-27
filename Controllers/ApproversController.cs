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
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var approver = await _context.Approver
                .Include(a => a.Case)
                .Include(a => a.LocalUser)
                .Include(a => a.LocalGroup)
                .FirstOrDefaultAsync(m => m.CaseID == id);
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
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var approver = await _context.Approver.FindAsync(id);
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
        public async Task<IActionResult> Edit(int id, [Bind("CaseID,LocalUserID,Approved,Order,LocalGroupID")] Approver approver)
        {
            if (id != approver.CaseID)
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
                    if (!ApproverExists(approver.CaseID))
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
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var approver = await _context.Approver
                .Include(a => a.Case)
                .Include(a => a.LocalUser)
                .Include(a => a.LocalGroup)
                .FirstOrDefaultAsync(m => m.CaseID == id);
            if (approver == null)
            {
                return NotFound();
            }

            return View(approver);
        }

        // POST: Approvers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var approver = await _context.Approver.FindAsync(id);
            _context.Approver.Remove(approver);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ApproverExists(int id)
        {
            return _context.Approver.Any(e => e.CaseID == id);
        }
    }
}
