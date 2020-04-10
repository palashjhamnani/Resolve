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
    public class LocalGroupsController : Controller
    {
        private readonly ResolveCaseContext _context;

        public LocalGroupsController(ResolveCaseContext context)
        {
            _context = context;
        }

        // GET: LocalGroups
        public async Task<IActionResult> Index()
        {
            var resolveCaseContext = _context.LocalGroup.Include(l => l.LocalUser);
            return View(await resolveCaseContext.ToListAsync());
        }

        // GET: LocalGroups/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var localGroup = await _context.LocalGroup
                .Include(l => l.LocalUser)
                .FirstOrDefaultAsync(m => m.LocalGroupID == id);
            if (localGroup == null)
            {
                return NotFound();
            }

            return View(localGroup);
        }

        // GET: LocalGroups/Create
        public IActionResult Create()
        {
            ViewData["LocalUserID"] = new SelectList(_context.LocalUser, "LocalUserID", "LocalUserID");
            return View();
        }

        // POST: LocalGroups/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LocalGroupID,GroupName,GroupDescription,LocalUserID")] LocalGroup localGroup)
        {
            if (ModelState.IsValid)
            {
                _context.Add(localGroup);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["LocalUserID"] = new SelectList(_context.LocalUser, "LocalUserID", "LocalUserID", localGroup.LocalUserID);
            return View(localGroup);
        }

        // GET: LocalGroups/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var localGroup = await _context.LocalGroup.FindAsync(id);
            if (localGroup == null)
            {
                return NotFound();
            }
            ViewData["LocalUserID"] = new SelectList(_context.LocalUser, "LocalUserID", "LocalUserID", localGroup.LocalUserID);
            return View(localGroup);
        }

        // POST: LocalGroups/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("LocalGroupID,GroupName,GroupDescription,LocalUserID")] LocalGroup localGroup)
        {
            if (id != localGroup.LocalGroupID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(localGroup);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LocalGroupExists(localGroup.LocalGroupID))
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
            ViewData["LocalUserID"] = new SelectList(_context.LocalUser, "LocalUserID", "LocalUserID", localGroup.LocalUserID);
            return View(localGroup);
        }

        // GET: LocalGroups/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var localGroup = await _context.LocalGroup
                .Include(l => l.LocalUser)
                .FirstOrDefaultAsync(m => m.LocalGroupID == id);
            if (localGroup == null)
            {
                return NotFound();
            }

            return View(localGroup);
        }

        // POST: LocalGroups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var localGroup = await _context.LocalGroup.FindAsync(id);
            _context.LocalGroup.Remove(localGroup);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LocalGroupExists(string id)
        {
            return _context.LocalGroup.Any(e => e.LocalGroupID == id);
        }
    }
}
