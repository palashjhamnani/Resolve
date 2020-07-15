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
                    Description = hrGradStudent.Description,
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
        public async Task<IActionResult> Edit(int id, [Bind("CaseID,Description,StudentName,GradRequestType,GradJobProfile,EffectiveStartDate,EffectiveEndDate,StepStipendAllowance,Department,Note,BudgetNumbers")] HRServiceGradStudent hrGradStudent)

        {
            if (ModelState.IsValid)
            {
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