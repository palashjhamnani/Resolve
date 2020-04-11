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
    public class OnBehalvesController : Controller
    {
        private readonly ResolveCaseContext _context;

        public OnBehalvesController(ResolveCaseContext context)
        {
            _context = context;
        }

        // GET: OnBehalves
        public async Task<IActionResult> Index()
        {
            var resolveCaseContext = _context.OnBehalf.Include(o => o.Case).Include(o => o.LocalUser);
            return View(await resolveCaseContext.ToListAsync());
        }

        // GET: OnBehalves/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var onBehalf = await _context.OnBehalf
                .Include(o => o.Case)
                .Include(o => o.LocalUser)
                .FirstOrDefaultAsync(m => m.CaseID == id);
            if (onBehalf == null)
            {
                return NotFound();
            }

            return View(onBehalf);
        }

        // GET: OnBehalves/Create
        public IActionResult Create()
        {
            ViewData["CaseID"] = new SelectList(_context.Case, "CaseID", "CaseID");
            ViewData["LocalUserID"] = new SelectList(_context.LocalUser, "LocalUserID", "LocalUserID");
            return View();
        }

        // POST: OnBehalves/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CaseID,LocalUserID")] OnBehalf onBehalf)
        {
            if (ModelState.IsValid)
            {
                _context.Add(onBehalf);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CaseID"] = new SelectList(_context.Case, "CaseID", "CaseID", onBehalf.CaseID);
            ViewData["LocalUserID"] = new SelectList(_context.LocalUser, "LocalUserID", "LocalUserID", onBehalf.LocalUserID);
            return View(onBehalf);
        }

        // GET: OnBehalves/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var onBehalf = await _context.OnBehalf.FindAsync(id);
            if (onBehalf == null)
            {
                return NotFound();
            }
            ViewData["CaseID"] = new SelectList(_context.Case, "CaseID", "CaseID", onBehalf.CaseID);
            ViewData["LocalUserID"] = new SelectList(_context.LocalUser, "LocalUserID", "LocalUserID", onBehalf.LocalUserID);
            return View(onBehalf);
        }

        // POST: OnBehalves/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CaseID,LocalUserID")] OnBehalf onBehalf)
        {
            if (id != onBehalf.CaseID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(onBehalf);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OnBehalfExists(onBehalf.CaseID))
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
            ViewData["CaseID"] = new SelectList(_context.Case, "CaseID", "CaseID", onBehalf.CaseID);
            ViewData["LocalUserID"] = new SelectList(_context.LocalUser, "LocalUserID", "LocalUserID", onBehalf.LocalUserID);
            return View(onBehalf);
        }

        // GET: OnBehalves/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var onBehalf = await _context.OnBehalf
                .Include(o => o.Case)
                .Include(o => o.LocalUser)
                .FirstOrDefaultAsync(m => m.CaseID == id);
            if (onBehalf == null)
            {
                return NotFound();
            }

            return View(onBehalf);
        }

        // POST: OnBehalves/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var onBehalf = await _context.OnBehalf.FindAsync(id);
            _context.OnBehalf.Remove(onBehalf);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OnBehalfExists(int id)
        {
            return _context.OnBehalf.Any(e => e.CaseID == id);
        }
    }
}
