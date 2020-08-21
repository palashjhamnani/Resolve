using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Resolve.Data;
using Resolve.Models;

namespace Resolve.Areas.CaseSpecificDetails.Controllers
{
    [Area("CaseSpecificDetails")]
    //[Route(nameof(CaseTypes) + "/[controller]")]
    public class HiringAffiliateFacultyController : Controller
    {

        private readonly ResolveCaseContext _context;

        public HiringAffiliateFacultyController(ResolveCaseContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create(int id)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int id, HiringAffiliateFaculty hrAffFaculty)
        {
            if (ModelState.IsValid)
            {
                HiringAffiliateFaculty newCase = new HiringAffiliateFaculty
                {
                    CaseID = id,
                    FacAffiliateTitle = hrAffFaculty.FacAffiliateTitle,
                    Name = hrAffFaculty.Name,
                    HireDate = hrAffFaculty.HireDate,                 
                    Department = hrAffFaculty.Department,                  
                    Note = hrAffFaculty.Note
                };
                _context.Add(newCase);
                await _context.SaveChangesAsync();
                var cid = id;
                return RedirectToAction("Details", "Cases", new { id = cid, area = "" });
                //return RedirectToAction("Index", "Home");
            }
            return View(hrAffFaculty);
        }
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            HiringAffiliateFaculty editCase = _context.HiringAffiliateFaculty.Find(id);
            if (editCase == null)
            {
                return NotFound();
            }
            return View(editCase);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CaseID,Name,FacAffiliateTitle,HireDate,Department,Note")] HiringAffiliateFaculty hrAffFaculty)

        {
            IQueryable<HiringAffiliateFaculty> beforeCases = _context.HiringAffiliateFaculty.Where(c => c.CaseID == id).AsNoTracking<HiringAffiliateFaculty>();
            HiringAffiliateFaculty beforeCase = beforeCases.FirstOrDefault();
            if (beforeCase == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                /** First check fields to see if values have changed and if so add to audit log  **/
               /** Also clear any fields that were hidden when and not automatically reset in the form **/

                string strAudit = "Case Details Edited. Values updated (old,new). ";
               
                string bcRequest = beforeCase.FacAffiliateTitle.ToString();
                string currRequest = hrAffFaculty.FacAffiliateTitle.ToString();              
                


                if (beforeCase.Department.ToString() != hrAffFaculty.Department.ToString())
                {
                    strAudit += "Department: (" + beforeCase.Department.ToString() + "," + hrAffFaculty.Department.ToString() + ") ";
                }

               

                var audit = new CaseAudit { AuditLog = strAudit, CaseID = id, LocalUserID = User.Identity.Name };
                _context.Add(audit);
                _context.Entry(hrAffFaculty).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                var cid = id;
                return RedirectToAction("Details", "Cases", new { id = cid, area = "" });
                //return RedirectToAction("Index", "Home");
            }
            return View(hrAffFaculty);
        }

    }
}