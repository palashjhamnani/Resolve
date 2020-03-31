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
    public class CaseCommentsController : Controller
    {
        private readonly ResolveCaseContext _context;

        public CaseCommentsController(ResolveCaseContext context)
        {
            _context = context;
        }

        // GET: CaseComments
        public async Task<IActionResult> Index()
        {
            var resolveCaseContext = _context.CaseComment.Include(c => c.Case).Include(c => c.User);
            return View(await resolveCaseContext.ToListAsync());
        }

        // GET: CaseComments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var caseComment = await _context.CaseComment
                .Include(c => c.Case)
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.CaseCommentID == id);
            if (caseComment == null)
            {
                return NotFound();
            }

            return View(caseComment);
        }

        // GET: CaseComments/Create
        public IActionResult Create(int? id)
        {
            //ViewData["CaseID"] = new SelectList(_context.Case, "CaseID", "CaseID");
            //ViewData["UserID"] = new SelectList(_context.User, "UserID", "UserID");
            return View();
        }

        // POST: CaseComments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int id, [Bind("CaseCommentID,Comment,CommentTimestamp,CaseID,UserID")] CaseComment caseComment)
        {
            //caseComment.CaseID = cid;
            //HttpContext.Request.Form["UserName"];
            try
            {
                if (ModelState.IsValid)
                {
                    Console.WriteLine("Check Check");
                    Console.WriteLine(id);
                    _context.Add(caseComment);
                    await _context.SaveChangesAsync();
                    //return RedirectToAction("", new { id = 99 });
                    return RedirectToAction(nameof(Index));

                }
            }
            catch (Exception)
            {

                Console.WriteLine("Error Palash!");
            }
        
            //ViewData["CaseID"] = new SelectList(_context.Case, "CaseID", "CaseID", caseComment.CaseID);
            //ViewData["UserID"] = new SelectList(_context.User, "UserID", "UserID", caseComment.UserID);
            return View(caseComment);
        }

        // GET: CaseComments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var caseComment = await _context.CaseComment.FindAsync(id);
            if (caseComment == null)
            {
                return NotFound();
            }
            ViewData["CaseID"] = new SelectList(_context.Case, "CaseID", "CaseID", caseComment.CaseID);
            ViewData["UserID"] = new SelectList(_context.User, "UserID", "UserID", caseComment.UserID);
            return View(caseComment);
        }

        // POST: CaseComments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CaseCommentID,Comment,CommentTimestamp,CaseID,UserID")] CaseComment caseComment)
        {
            if (id != caseComment.CaseCommentID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(caseComment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CaseCommentExists(caseComment.CaseCommentID))
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
            ViewData["CaseID"] = new SelectList(_context.Case, "CaseID", "CaseID", caseComment.CaseID);
            ViewData["UserID"] = new SelectList(_context.User, "UserID", "UserID", caseComment.UserID);
            return View(caseComment);
        }

        // GET: CaseComments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var caseComment = await _context.CaseComment
                .Include(c => c.Case)
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.CaseCommentID == id);
            if (caseComment == null)
            {
                return NotFound();
            }

            return View(caseComment);
        }

        // POST: CaseComments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var caseComment = await _context.CaseComment.FindAsync(id);
            _context.CaseComment.Remove(caseComment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CaseCommentExists(int id)
        {
            return _context.CaseComment.Any(e => e.CaseCommentID == id);
        }
    }
}
