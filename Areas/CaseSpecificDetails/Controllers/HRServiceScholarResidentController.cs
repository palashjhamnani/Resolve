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
    public class HRServiceScholarResidentController : Controller
    {

        private readonly ResolveCaseContext _context;

        public HRServiceScholarResidentController(ResolveCaseContext context)
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
        public async Task<IActionResult> Create(int id, HRServiceScholarResident hrScholar)
        {
            if (ModelState.IsValid)
            {
                HRServiceScholarResident newCase = new HRServiceScholarResident
                {
                    CaseID = id,                     
                    Name = hrScholar.Name,
                    ScholarRequestType = hrScholar.ScholarRequestType,
                    ScholarJobProfile = hrScholar.ScholarJobProfile,
                    ScholarCompAllowanceChange=hrScholar.ScholarCompAllowanceChange,
                    EffectiveStartDate = hrScholar.EffectiveStartDate,
                    EffectiveEndDate = hrScholar.EffectiveEndDate,
                    CurrentFTE=hrScholar.CurrentFTE,
                    ProposedFTE = hrScholar.ProposedFTE,
                    JobTitle = hrScholar.JobTitle,
                    PropTitle = hrScholar.PropTitle,
                    Department = hrScholar.Department,
                    StepStipendAllowance = hrScholar.StepStipendAllowance,
                    BudgetNumbers = hrScholar.BudgetNumbers,
                    Note = hrScholar.Note,
                };
                
                _context.Add(newCase);
                await _context.SaveChangesAsync();
                var cid = id;
                return RedirectToAction("Details", "Cases", new { id = cid, area = "" });
                //return RedirectToAction("Index", "Home");
            }
            return View(hrScholar);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            HRServiceScholarResident editCase = _context.HRServiceScholarResident.Find(id);
            if (editCase == null)
            {
                return NotFound();
            }
            return View(editCase);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CaseID,Name,ScholarRequestType,ScholarCompAllowanceChange,ScholarJobProfile,JobTitle,PropTitle,CurrentFTE,ProposedFTE,EffectiveStartDate,EffectiveEndDate,StepStipendAllowance,Department,Note,BudgetNumbers")] HRServiceScholarResident hrScholar)

        {
            /** First check important fields to see if values have changed and if so add to audit log **/
            string strAudit = "Case Edited. Values updated (old,new). ";

            IQueryable<HRServiceScholarResident> beforeCases = _context.HRServiceScholarResident.Where(c => c.CaseID == id).AsNoTracking<HRServiceScholarResident>();
            HRServiceScholarResident beforeCase = beforeCases.FirstOrDefault();
            if (beforeCase == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                if (beforeCase.Name != hrScholar.Name)
                {
                    strAudit += "Student: (" + beforeCase.Name + "," + hrScholar.Name + ") ";
                }

               
                if (beforeCase.EffectiveStartDate.ToShortDateString() != hrScholar.EffectiveStartDate.ToShortDateString())
                {
                    strAudit += "StartDate: (" + beforeCase.EffectiveStartDate.ToShortDateString() + "," + hrScholar.EffectiveStartDate.ToShortDateString() + ") ";
                }
                DateTime beforeEffectDate = beforeCase.EffectiveEndDate.GetValueOrDefault();
                DateTime curEffectDate = hrScholar.EffectiveEndDate.GetValueOrDefault();
                if (beforeEffectDate.ToShortDateString() != curEffectDate.ToShortDateString())
                {
                    strAudit += "EndDate: (" + beforeEffectDate.ToShortDateString() + "," + curEffectDate.ToShortDateString() + ") ";
                }
                /*
                if (beforeCase.Department.ToString() != hrGradStudent.Department.ToString())
                {
                    strAudit += "Department: (" + beforeCase.Department.ToString() + "," + hrGradStudent.Department.ToString() + ") ";
                }
                if (beforeCase.StepStipendAllowance.ToString() != hrGradStudent.StepStipendAllowance.ToString())
                {
                    strAudit += "Allowance: (" + beforeCase.StepStipendAllowance.ToString() + "," + hrGradStudent.StepStipendAllowance.ToString() + ") ";
                }
                if (beforeCase.BudgetNumbers.ToString() != hrGradStudent.BudgetNumbers.ToString())
                {
                    strAudit += "BudgetNumbers: (" + beforeCase.BudgetNumbers.ToString() + "," + hrGradStudent.BudgetNumbers.ToString() + ") ";
                }
                */

                var audit = new CaseAudit { AuditLog = strAudit, CaseID = id, LocalUserID = User.Identity.Name };
                _context.Add(audit);
                _context.Entry(hrScholar).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                var cid = id;
                return RedirectToAction("Details", "Cases", new { id = cid, area = "" });
                //return RedirectToAction("Index", "Home");
            }
            return View(hrScholar);
        }
    }
}