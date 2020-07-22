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
    public class UserGroupsController : Controller
    {
        private readonly ResolveCaseContext _context;

        public UserGroupsController(ResolveCaseContext context)
        {
            _context = context;
        }

        // GET: UserGroups
        public async Task<IActionResult> Index()
        {
            var resolveCaseContext = _context.UserGroup.Include(u => u.LocalGroup).Include(u => u.LocalUser);
            return View(await resolveCaseContext.ToListAsync());
        }

        // GET: UserGroups/Details/5
        public async Task<IActionResult> Details(string lid, string gid)
        {
            if (lid == null || gid == null)
            {
                return NotFound();
            }

            var userGroup = await _context.UserGroup
                .Include(u => u.LocalGroup)
                .Include(u => u.LocalUser)
                .FirstOrDefaultAsync(m => m.LocalUserID == lid && m.LocalGroupID == gid);
            if (userGroup == null)
            {
                return NotFound();
            }

            return View(userGroup);
        }

        // GET: UserGroups/Create
        public IActionResult Create()
        {
            ViewData["LocalGroupID"] = new SelectList(_context.LocalGroup, "LocalGroupID", "LocalGroupID");
            ViewData["LocalUserID"] = new SelectList(_context.LocalUser, "LocalUserID", "LocalUserID");
            return View();
        }

        // POST: UserGroups/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LocalUserID,LocalGroupID")] UserGroup userGroup)
        {
            if (ModelState.IsValid)
            {
                _context.Add(userGroup);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["LocalGroupID"] = new SelectList(_context.LocalGroup, "LocalGroupID", "LocalGroupID", userGroup.LocalGroupID);
            ViewData["LocalUserID"] = new SelectList(_context.LocalUser, "LocalUserID", "LocalUserID", userGroup.LocalUserID);
            return View(userGroup);
        }

        // GET: UserGroups/Edit/5
        public async Task<IActionResult> Edit(string lid, string gid)
        {
            if (lid == null || gid == null)
            {
                return NotFound();
            }

            var userGroup = await _context.UserGroup.FindAsync(lid, gid);
            if (userGroup == null)
            {
                return NotFound();
            }
            ViewData["LocalGroupID"] = new SelectList(_context.LocalGroup, "LocalGroupID", "LocalGroupID", userGroup.LocalGroupID);
            ViewData["LocalUserID"] = new SelectList(_context.LocalUser, "LocalUserID", "LocalUserID", userGroup.LocalUserID);
            return View(userGroup);
        }

        // POST: UserGroups/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string lid, string gid, [Bind("LocalUserID,LocalGroupID")] UserGroup userGroup)
        {
            if (lid != userGroup.LocalUserID || gid != userGroup.LocalGroupID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userGroup);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserGroupExists(userGroup.LocalUserID, userGroup.LocalGroupID))
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
            ViewData["LocalGroupID"] = new SelectList(_context.LocalGroup, "LocalGroupID", "LocalGroupID", userGroup.LocalGroupID);
            ViewData["LocalUserID"] = new SelectList(_context.LocalUser, "LocalUserID", "LocalUserID", userGroup.LocalUserID);
            return View(userGroup);
        }

        // GET: UserGroups/Delete/5
        public async Task<IActionResult> Delete(string lid, string gid)
        {
            if (lid == null || gid == null)
            {
                return NotFound();
            }

            var userGroup = await _context.UserGroup
                .Include(u => u.LocalGroup)
                .Include(u => u.LocalUser)
                .FirstOrDefaultAsync(m => m.LocalUserID == lid && m.LocalGroupID == gid);
            if (userGroup == null)
            {
                return NotFound();
            }

            return View(userGroup);
        }

        // POST: UserGroups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string LocalUserid, string LocalGroupid)
        {
            var userGroup = await _context.UserGroup
                .Include(u => u.LocalGroup)
                .Include(u => u.LocalUser)
                .FirstOrDefaultAsync(m => m.LocalUserID == LocalUserid && m.LocalGroupID == LocalGroupid);
            _context.UserGroup.Remove(userGroup);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserGroupExists(string lid, string gid)
        {
            return _context.UserGroup.Any(e => e.LocalUserID == lid && e.LocalGroupID == gid);
        }
    }
}
