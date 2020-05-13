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
    public class CaseTypeGroupsController : Controller
    {
        private readonly ResolveCaseContext _context;

        public CaseTypeGroupsController(ResolveCaseContext context)
        {
            _context = context;
        }

        // GET: CaseTypeGroups
        public async Task<IActionResult> Index()
        {
            var resolveCaseContext = _context.CaseTypeGroup.Include(c => c.CaseType).Include(c => c.LocalGroup);
            return View(await resolveCaseContext.ToListAsync());
        }

        // GET: CaseTypeGroups/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var caseTypeGroup = await _context.CaseTypeGroup
                .Include(c => c.CaseType)
                .Include(c => c.LocalGroup)
                .FirstOrDefaultAsync(m => m.CaseTypeID == id);
            if (caseTypeGroup == null)
            {
                return NotFound();
            }

            return View(caseTypeGroup);
        }

        // GET: CaseTypeGroups/Create
        public IActionResult Create()
        {
            ViewData["CaseTypeID"] = new SelectList(_context.CaseType, "CaseTypeID", "CaseTypeID");
            ViewData["LocalGroupID"] = new SelectList(_context.LocalGroup, "LocalGroupID", "LocalGroupID");
            return View();
        }

        // POST: CaseTypeGroups/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CaseTypeID,LocalGroupID,Approved,Order")] CaseTypeGroup caseTypeGroup)
        {
            if (ModelState.IsValid)
            {
                _context.Add(caseTypeGroup);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CaseTypeID"] = new SelectList(_context.CaseType, "CaseTypeID", "CaseTypeID", caseTypeGroup.CaseTypeID);
            ViewData["LocalGroupID"] = new SelectList(_context.LocalGroup, "LocalGroupID", "LocalGroupID", caseTypeGroup.LocalGroupID);
            return View(caseTypeGroup);
        }

        // GET: CaseTypeGroups/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var caseTypeGroup = await _context.CaseTypeGroup.FindAsync(id);
            if (caseTypeGroup == null)
            {
                return NotFound();
            }
            ViewData["CaseTypeID"] = new SelectList(_context.CaseType, "CaseTypeID", "CaseTypeID", caseTypeGroup.CaseTypeID);
            ViewData["LocalGroupID"] = new SelectList(_context.LocalGroup, "LocalGroupID", "LocalGroupID", caseTypeGroup.LocalGroupID);
            return View(caseTypeGroup);
        }

        // POST: CaseTypeGroups/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CaseTypeID,LocalGroupID,Approved,Order")] CaseTypeGroup caseTypeGroup)
        {
            if (id != caseTypeGroup.CaseTypeID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(caseTypeGroup);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CaseTypeGroupExists(caseTypeGroup.CaseTypeID))
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
            ViewData["CaseTypeID"] = new SelectList(_context.CaseType, "CaseTypeID", "CaseTypeID", caseTypeGroup.CaseTypeID);
            ViewData["LocalGroupID"] = new SelectList(_context.LocalGroup, "LocalGroupID", "LocalGroupID", caseTypeGroup.LocalGroupID);
            return View(caseTypeGroup);
        }

        // GET: CaseTypeGroups/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var caseTypeGroup = await _context.CaseTypeGroup
                .Include(c => c.CaseType)
                .Include(c => c.LocalGroup)
                .FirstOrDefaultAsync(m => m.CaseTypeID == id);
            if (caseTypeGroup == null)
            {
                return NotFound();
            }

            return View(caseTypeGroup);
        }

        // POST: CaseTypeGroups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var caseTypeGroup = await _context.CaseTypeGroup.FindAsync(id);
            _context.CaseTypeGroup.Remove(caseTypeGroup);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CaseTypeGroupExists(int id)
        {
            return _context.CaseTypeGroup.Any(e => e.CaseTypeID == id);
        }
    }
}
