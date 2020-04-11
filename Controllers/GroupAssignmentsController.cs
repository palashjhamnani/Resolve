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
    public class GroupAssignmentsController : Controller
    {
        private readonly ResolveCaseContext _context;

        public GroupAssignmentsController(ResolveCaseContext context)
        {
            _context = context;
        }

        // GET: GroupAssignments
        public async Task<IActionResult> Index()
        {
            var resolveCaseContext = _context.GroupAssignment.Include(g => g.Case).Include(g => g.LocalGroup);
            return View(await resolveCaseContext.ToListAsync());
        }

        // GET: GroupAssignments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var groupAssignment = await _context.GroupAssignment
                .Include(g => g.Case)
                .Include(g => g.LocalGroup)
                .FirstOrDefaultAsync(m => m.CaseID == id);
            if (groupAssignment == null)
            {
                return NotFound();
            }

            return View(groupAssignment);
        }

        // GET: GroupAssignments/Create
        public IActionResult Create()
        {
            ViewData["CaseID"] = new SelectList(_context.Case, "CaseID", "CaseID");
            ViewData["LocalGroupID"] = new SelectList(_context.LocalGroup, "LocalGroupID", "LocalGroupID");
            return View();
        }

        // POST: GroupAssignments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CaseID,LocalGroupID")] GroupAssignment groupAssignment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(groupAssignment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CaseID"] = new SelectList(_context.Case, "CaseID", "CaseID", groupAssignment.CaseID);
            ViewData["LocalGroupID"] = new SelectList(_context.LocalGroup, "LocalGroupID", "LocalGroupID", groupAssignment.LocalGroupID);
            return View(groupAssignment);
        }

        // GET: GroupAssignments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var groupAssignment = await _context.GroupAssignment.FindAsync(id);
            if (groupAssignment == null)
            {
                return NotFound();
            }
            ViewData["CaseID"] = new SelectList(_context.Case, "CaseID", "CaseID", groupAssignment.CaseID);
            ViewData["LocalGroupID"] = new SelectList(_context.LocalGroup, "LocalGroupID", "LocalGroupID", groupAssignment.LocalGroupID);
            return View(groupAssignment);
        }

        // POST: GroupAssignments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CaseID,LocalGroupID")] GroupAssignment groupAssignment)
        {
            if (id != groupAssignment.CaseID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(groupAssignment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GroupAssignmentExists(groupAssignment.CaseID))
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
            ViewData["CaseID"] = new SelectList(_context.Case, "CaseID", "CaseID", groupAssignment.CaseID);
            ViewData["LocalGroupID"] = new SelectList(_context.LocalGroup, "LocalGroupID", "LocalGroupID", groupAssignment.LocalGroupID);
            return View(groupAssignment);
        }

        // GET: GroupAssignments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var groupAssignment = await _context.GroupAssignment
                .Include(g => g.Case)
                .Include(g => g.LocalGroup)
                .FirstOrDefaultAsync(m => m.CaseID == id);
            if (groupAssignment == null)
            {
                return NotFound();
            }

            return View(groupAssignment);
        }

        // POST: GroupAssignments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var groupAssignment = await _context.GroupAssignment.FindAsync(id);
            _context.GroupAssignment.Remove(groupAssignment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GroupAssignmentExists(int id)
        {
            return _context.GroupAssignment.Any(e => e.CaseID == id);
        }
    }
}
