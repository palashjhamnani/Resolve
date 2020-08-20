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
        public async Task<IActionResult> Details(int? CaseTypeid, string? LocalGroupid)
        {
            if (CaseTypeid == null || LocalGroupid == "")
            {
                return NotFound();
            }

            var caseTypeGroup = await _context.CaseTypeGroup
                .Include(c => c.CaseType)
                .Include(c => c.LocalGroup)
                .FirstOrDefaultAsync(m => m.CaseTypeID == CaseTypeid && m.LocalGroupID == LocalGroupid);
            if (caseTypeGroup == null)
            {
                return NotFound();
            }

            return View(caseTypeGroup);
        }


        public IActionResult OldCreate()
        {
            ViewData["CaseTypeTitle"] = new SelectList(_context.CaseType, "CaseTypeTitle", "CaseTypeTitle");
            ViewData["GroupName"] = new SelectList(_context.LocalGroup, "GroupName", "GroupName");
            return View();
        }

        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OldCreate([Bind("Order")] CaseTypeGroup caseTypeGroup)
        {
            var GName = HttpContext.Request.Form["GroupName"].ToString();
            var CTypeTitle = HttpContext.Request.Form["CaseTypeTitle"].ToString();
            var LGroup = _context.LocalGroup.Single(p => p.GroupName == GName);
            caseTypeGroup.LocalGroupID = LGroup.LocalGroupID;
            var CType = _context.CaseType.Single(p => p.CaseTypeTitle == CTypeTitle);
            caseTypeGroup.CaseTypeID = CType.CaseTypeID;
            if (ModelState.IsValid)
            {
                _context.Add(caseTypeGroup);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CaseTypeTitle"] = new SelectList(_context.CaseType, "CaseTypeTitle", "CaseTypeTitle");
            ViewData["GroupName"] = new SelectList(_context.LocalGroup, "GroupName", "GroupName");
            return View(caseTypeGroup);
        }

        // GET: CaseTypeGroups/Create
        public IActionResult Create(int? id, int? cid)
        {            
            var hierarchy = _context.CaseType
                .Single(a => a.CaseTypeID == cid);
            if (hierarchy.Hierarchical_Approval == true)
            {
                ViewData["hierarchical"] = "ordered";
            }
            else
            {
                ViewData["hierarchical"] = "unordered";
            }
            ViewData["NumberOfGroups"] = id;
            ViewData["CTypeID"] = cid;
            ViewData["CaseTypeID"] = new SelectList(_context.CaseType, "CaseTypeID", "CaseTypeID");
            ViewData["GroupName"] = new SelectList(_context.LocalGroup, "GroupName", "GroupName");
            return View();
        }

        // POST: CaseTypeGroups/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int? id, int? cid, CaseTypeGroup caseTypeGroup)
        {
            if (id == null)
            {
                id = 1;
            }
            int CType = 1;
            if (ModelState.IsValid)
            {
                for (int i = 0; i < id; i++)
                {
                    string num = i.ToString();
                    string CTypeName = "CaseType" + num;
                    string LGroupName = "LocalGroup" + num;                    
                    string OrderName = "Order" + num;
                    int CTypeID = Convert.ToInt32(HttpContext.Request.Form[CTypeName]);
                    CType = CTypeID;
                    string LGroupN = HttpContext.Request.Form[LGroupName].ToString();
                    var LGroupID = _context.LocalGroup.Single(p => p.GroupName == LGroupN).LocalGroupID;
                    int Order = 1;
                    if (HttpContext.Request.Form[OrderName] != "")
                    {
                        Order = Convert.ToInt32(HttpContext.Request.Form[OrderName]);
                    }                              
                    CaseTypeGroup newCaseTypeGroup = new CaseTypeGroup
                    {
                        CaseTypeID = CTypeID,
                        LocalGroupID = LGroupID,                        
                        Order = Order
                    };
                    _context.Add(newCaseTypeGroup);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "CaseTypes", new { id = CType });
                //return RedirectToAction(nameof(Index));
            }
            
            ViewData["CaseTypeID"] = new SelectList(_context.CaseType, "CaseTypeID", "CaseTypeID", caseTypeGroup.CaseTypeID);
            ViewData["GroupName"] = new SelectList(_context.LocalGroup, "GroupName", "GroupName", caseTypeGroup.LocalGroupID);
            return View(caseTypeGroup);
        }

        // GET: CaseTypeGroups/Edit/5
        public async Task<IActionResult> Edit(int? CaseTypeid, string? LocalGroupid)
        {
            if (CaseTypeid == null || LocalGroupid == "")
            {
                return NotFound();
            }

            var caseTypeGroup = await _context.CaseTypeGroup.FindAsync(CaseTypeid, LocalGroupid);
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
        public async Task<IActionResult> Edit(int CaseTypeid, string LocalGroupid, [Bind("CaseTypeID,LocalGroupID,Approved,Order")] CaseTypeGroup caseTypeGroup)
        {
            if (CaseTypeid != caseTypeGroup.CaseTypeID || LocalGroupid != caseTypeGroup.LocalGroupID)
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
                    if (!CaseTypeGroupExists(caseTypeGroup.CaseTypeID, caseTypeGroup.LocalGroupID))
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
        public async Task<IActionResult> Delete(int? CaseTypeid, string? LocalGroupid)
        {
            if (CaseTypeid == null || LocalGroupid == "")
            {
                return NotFound();
            }

            var caseTypeGroup = await _context.CaseTypeGroup
                .Include(c => c.CaseType)
                .Include(c => c.LocalGroup)
                .FirstOrDefaultAsync(m => m.CaseTypeID == CaseTypeid && m.LocalGroupID == LocalGroupid);
            if (caseTypeGroup == null)
            {
                return NotFound();
            }

            return View(caseTypeGroup);
        }

        // POST: CaseTypeGroups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int CaseTypeid, string LocalGroupid)
        {
            var caseTypeGroup = await _context.CaseTypeGroup.FindAsync(CaseTypeid, LocalGroupid);
            _context.CaseTypeGroup.Remove(caseTypeGroup);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CaseTypeGroupExists(int CaseTypeid, string LocalGroupid)
        {
            return _context.CaseTypeGroup.Any(e => e.CaseTypeID == CaseTypeid && e.LocalGroupID == LocalGroupid);
        }
    }
}
