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
    public class HRServiceGradStudentController : Controller
    {

        private readonly ResolveCaseContext _context;

        public HRServiceGradStudentController(ResolveCaseContext context)
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
        public async Task<IActionResult> Create(int id, HRServiceGradStudent hrGradStudent)
        {
            if (ModelState.IsValid)
            {
                HRServiceGradStudent newCase = new HRServiceGradStudent
                {
                    CaseID = id,
                   
                    StudentName = hrGradStudent.StudentName,
                    GradRequestType = hrGradStudent.GradRequestType,
                    GradJobProfile = hrGradStudent.GradJobProfile,
                    EffectiveStartDate = hrGradStudent.EffectiveStartDate,
                    EffectiveEndDate = hrGradStudent.EffectiveEndDate,
                    Department = hrGradStudent.Department,
                    StepStipendAllowance = hrGradStudent.StepStipendAllowance,
                    BudgetNumbers = hrGradStudent.BudgetNumbers,
                    Note = hrGradStudent.Note,
                };
                _context.Add(newCase);
                await _context.SaveChangesAsync();
                var cid = id;
                return RedirectToAction("Details", "Cases", new { id = cid, area = "" });
                //return RedirectToAction("Index", "Home");
            }
            return View(hrGradStudent);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            HRServiceGradStudent editCase = _context.HRServiceGradStudent.Find(id);
            if (editCase == null)
            {
                return NotFound();
            }
            return View(editCase);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CaseID,StudentName,GradRequestType,GradJobProfile,EffectiveStartDate,EffectiveEndDate,StepStipendAllowance,Department,Note,BudgetNumbers")] HRServiceGradStudent hrGradStudent)

        {
            /** First check important fields to see if values have changed and if so add to audit log **/
            string strAudit = "Case Details Edited. Values updated (old,new). ";

            IQueryable<HRServiceGradStudent> beforeCases = _context.HRServiceGradStudent.Where(c => c.CaseID == id).AsNoTracking<HRServiceGradStudent>();
            HRServiceGradStudent beforeCase = beforeCases.FirstOrDefault();
            if (beforeCase == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                if (beforeCase.StudentName != hrGradStudent.StudentName)
                {
                    strAudit += "Student: (" + beforeCase.StudentName + "," + hrGradStudent.StudentName + ") ";
                }

                if (beforeCase.GradRequestType.ToString() != hrGradStudent.GradRequestType.ToString())
                {
                    strAudit += "RequestType: (" + beforeCase.GradRequestType.ToString() + "," + hrGradStudent.GradRequestType.ToString() + ") ";
                }
                if (beforeCase.GradJobProfile.ToString() != hrGradStudent.GradJobProfile.ToString())
                {
                    strAudit += "JobProfile: (" + beforeCase.GradJobProfile.ToString() + "," + hrGradStudent.GradJobProfile.ToString() + ") ";
                }
                if (beforeCase.EffectiveStartDate.ToShortDateString() != hrGradStudent.EffectiveStartDate.ToShortDateString())
                {
                    strAudit += "StartDate: (" + beforeCase.EffectiveStartDate.ToShortDateString() + "," + hrGradStudent.EffectiveStartDate.ToShortDateString() + ") ";
                }
                DateTime beforeEffectDate = beforeCase.EffectiveEndDate.GetValueOrDefault();
                DateTime curEffectDate = hrGradStudent.EffectiveEndDate.GetValueOrDefault();
                if (beforeEffectDate.ToShortDateString() != curEffectDate.ToShortDateString())
                {
                    strAudit += "EndDate: (" + beforeEffectDate.ToShortDateString() + "," + curEffectDate.ToShortDateString() + ") ";
                }
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


                var audit = new CaseAudit { AuditLog = strAudit, CaseID = id, LocalUserID = User.Identity.Name };
                _context.Add(audit);
                _context.Entry(hrGradStudent).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                var cid = id;
                return RedirectToAction("Details", "Cases", new { id = cid, area = "" });
                //return RedirectToAction("Index", "Home");
            }
            return View(hrGradStudent);
        }
    }
}