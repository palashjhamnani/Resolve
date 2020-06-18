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
    public class HRServiceFacultyController : Controller
    {

        private readonly ResolveCaseContext _context;

        public HRServiceFacultyController(ResolveCaseContext context)
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
        public async Task<IActionResult> Create(int id, HRServiceFaculty hrFaculty)
        {
            if (ModelState.IsValid)
            {
                HRServiceFaculty newCase = new HRServiceFaculty
                {
                    CaseID = id,
                    Description = hrFaculty.Description,
                    EmployeeName = hrFaculty.EmployeeName,
                    FacRequestType = hrFaculty.FacRequestType,
                    FacAllowanceChange = hrFaculty.FacAllowanceChange,
                    EffectiveStartDate = hrFaculty.EffectiveStartDate,
                    EffectiveEndDate = hrFaculty.EffectiveEndDate,
                    SupOrg = hrFaculty.SupOrg,
                    Department = hrFaculty.Department,
                    Salary = hrFaculty.Salary,
                    Amount = hrFaculty.Amount,
                    CurrentFTE = hrFaculty.CurrentFTE,
                    ProposedFTE = hrFaculty.ProposedFTE,
                    TerminationReason = hrFaculty.TerminationReason,
                    Offboarding = hrFaculty.Offboarding,
                    ClosePosition = hrFaculty.ClosePosition,
                    LeaveWA = hrFaculty.LeaveWA,
                    EmployeeEID = hrFaculty.EmployeeEID,
                    BudgetNumbers = hrFaculty.BudgetNumbers,
                    JobTitle = hrFaculty.JobTitle,
                    Note = hrFaculty.Note
                };
                _context.Add(newCase);
                await _context.SaveChangesAsync();
                var cid = id;
                return RedirectToAction("Details", "Cases", new { id = cid, area = "" });
                //return RedirectToAction("Index", "Home");
            }
            return View(hrFaculty);
        }

    }
}