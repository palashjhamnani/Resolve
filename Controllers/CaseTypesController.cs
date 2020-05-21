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
    public class CaseTypesController : Controller
    {
        private readonly ResolveCaseContext _context;

        public CaseTypesController(ResolveCaseContext context)
        {
            _context = context;
        }

        // GET: CaseTypes
        public async Task<IActionResult> Index()
        {
            var resolveCaseContext = _context.CaseType.Include(c => c.LocalGroup);
            return View(await resolveCaseContext.ToListAsync());
        }

        // GET: CaseTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var caseType = await _context.CaseType
                .Include(c => c.CaseTypeGroups).ThenInclude(p => p.LocalGroup).ThenInclude(p => p.LocalUser)
                .Include(c => c.LocalGroup)
                .FirstOrDefaultAsync(m => m.CaseTypeID == id);
            if (caseType == null)
            {
                return NotFound();
            }

            return View(caseType);
        }

        // GET: CaseTypes/Create
        public IActionResult Create()
        {
            ViewData["LocalGroupID"] = new SelectList(_context.LocalGroup, "LocalGroupID", "LocalGroupID");
            return View();
        }

        // POST: CaseTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CaseTypeID,CaseTypeTitle,LongDescription,LocalGroupID,GroupNumber")] CaseType caseType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(caseType);
                await _context.SaveChangesAsync();
                int ctypeid = caseType.CaseTypeID;
                return RedirectToAction("Create", "CaseTypeGroups", new { id = caseType.GroupNumber, cid = ctypeid });
                //return RedirectToAction(nameof(Index));
            }
            ViewData["LocalGroupID"] = new SelectList(_context.LocalGroup, "LocalGroupID", "LocalGroupID", caseType.LocalGroupID);
            return View(caseType);
        }

        // GET: CaseTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var caseType = await _context.CaseType.FindAsync(id);
            if (caseType == null)
            {
                return NotFound();
            }
            ViewData["LocalGroupID"] = new SelectList(_context.LocalGroup, "LocalGroupID", "LocalGroupID", caseType.LocalGroupID);
            return View(caseType);
        }

        // POST: CaseTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CaseTypeID,CaseTypeTitle,LongDescription,LocalGroupID,GroupNumber")] CaseType caseType)
        {
            if (id != caseType.CaseTypeID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(caseType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CaseTypeExists(caseType.CaseTypeID))
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
            ViewData["LocalGroupID"] = new SelectList(_context.LocalGroup, "LocalGroupID", "LocalGroupID", caseType.LocalGroupID);
            return View(caseType);
        }

        // GET: CaseTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var caseType = await _context.CaseType
                .Include(c => c.LocalGroup)
                .FirstOrDefaultAsync(m => m.CaseTypeID == id);
            if (caseType == null)
            {
                return NotFound();
            }

            return View(caseType);
        }

        // POST: CaseTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var caseType = await _context.CaseType.FindAsync(id);
            _context.CaseType.Remove(caseType);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CaseTypeExists(int id)
        {
            return _context.CaseType.Any(e => e.CaseTypeID == id);
        }
    }
}
