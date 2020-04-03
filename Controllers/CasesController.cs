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
    public class CasesController : Controller
    {
        private readonly ResolveCaseContext _context;

        public CasesController(ResolveCaseContext context)
        {
            _context = context;
        }



        // GET: CaseComments/Create
        public IActionResult CreateCommment(int? id)
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
        public async Task<IActionResult> CreateComment(int id, [Bind("CaseCommentID,Comment,CommentTimestamp,CaseID,UserID")] CaseComment caseComment)
        {
            //caseComment.CaseID = cid;
            //HttpContext.Request.Form["UserName"];
            try
            {
                if (ModelState.IsValid)
                {
                    //Console.WriteLine("Check Check");
                    Console.WriteLine(id);
                    _context.Add(caseComment);
                    await _context.SaveChangesAsync();
                    var cid = HttpContext.Request.Form["CaseID"];
                    return RedirectToAction("Details", new { id = cid });
                    //return RedirectToAction(nameof(Details));

                }
            }
            catch (Exception)
            {

                Console.WriteLine("Error Palash!");
            }

            return View(caseComment);
        }




        // GET: Cases
        public async Task<IActionResult> Index()
        {
            var resolveCaseContext = _context.Case.Include(s => s.CaseType).Include(u => u.User);
            //var resolveCaseContext = _context.Case;
            return View(await resolveCaseContext.ToListAsync());
        }

        // GET: Cases/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @case = await _context.Case
                .Include(s => s.CaseType)
                //.ThenInclude(q => q.CaseTypeTitle)
                .Include(u => u.User)
                .Include(p => p.CaseComments)
                .ThenInclude(e => e.User)
                .Include(a => a.CaseAudits)
                .FirstOrDefaultAsync(m => m.CaseID == id);
            if (@case == null)
            {
                return NotFound();
            }

            return View(@case);
        }



        // GET: Cases/Create
        public IActionResult Create()
        {
            ViewData["CaseTypeID"] = new SelectList(_context.CaseType, "CaseTypeID", "CaseTypeID");
            ViewData["UserID"] = new SelectList(_context.User, "UserID", "UserID");
            return View();
        }

        

        // POST: Cases/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CaseID,UserID,OnBehalfOf,CaseStatus,CaseCreationTimestamp,CaseTypeID")] Case @case)
        {
            if (ModelState.IsValid)
            {
                _context.Add(@case);
                var test = new CaseAudit {AuditLog = "Case Created", CaseID = 1, UserID = 1};
                _context.Add(test);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CaseTypeID"] = new SelectList(_context.CaseType, "CaseTypeID", "CaseTypeID", @case.CaseTypeID);
            ViewData["UserID"] = new SelectList(_context.User, "UserID", "UserID", @case.UserID);
            return View(@case);
        }

        // GET: Cases/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @case = await _context.Case.FindAsync(id);
            if (@case == null)
            {
                return NotFound();
            }
            ViewData["CaseTypeID"] = new SelectList(_context.CaseType, "CaseTypeID", "CaseTypeID", @case.CaseTypeID);
            ViewData["UserID"] = new SelectList(_context.User, "UserID", "UserID", @case.UserID);
            return View(@case);
        }

        // POST: Cases/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CaseID,UserID,OnBehalfOf,CaseStatus,CaseCreationTimestamp,CaseTypeID")] Case @case)
        {
            if (id != @case.CaseID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(@case);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CaseExists(@case.CaseID))
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
            ViewData["CaseTypeID"] = new SelectList(_context.CaseType, "CaseTypeID", "CaseTypeID", @case.CaseTypeID);
            ViewData["UserID"] = new SelectList(_context.User, "UserID", "UserID", @case.UserID);
            return View(@case);
        }

        // GET: Cases/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @case = await _context.Case
                .Include(s => s.CaseType)
                .Include(u => u.User)
                .FirstOrDefaultAsync(m => m.CaseID == id);
            if (@case == null)
            {
                return NotFound();
            }

            return View(@case);
        }

        // POST: Cases/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var @case = await _context.Case.FindAsync(id);
            _context.Case.Remove(@case);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CaseExists(int id)
        {
            return _context.Case.Any(e => e.CaseID == id);
        }
    }
}
