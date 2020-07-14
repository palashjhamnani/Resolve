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
    public class HRServiceStaffController : Controller
    {

        private readonly ResolveCaseContext _context;

        public HRServiceStaffController(ResolveCaseContext context)
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
        public async Task<IActionResult> Create(int id, HRServiceStaff hrStaff)
        {
            if (ModelState.IsValid)
            {
                HRServiceStaff newCase = new HRServiceStaff
                {
                    CaseID = id,
                    Description = hrStaff.Description,
                    EmployeeName = hrStaff.EmployeeName,
                    RequestType = hrStaff.RequestType,
                    BasePayChange = hrStaff.BasePayChange,
                    AllowanceChange = hrStaff.AllowanceChange,
                    TerminationReason = hrStaff.TerminationReason,
                    Offboarding = hrStaff.Offboarding,
                    LeaveWA = hrStaff.LeaveWA,
                    ClosePosition = hrStaff.ClosePosition,
                    Amount = hrStaff.Amount,
                    WorkerType = hrStaff.WorkerType,
                    EffectiveStartDate = hrStaff.EffectiveStartDate,
                    EffectiveEndDate = hrStaff.EffectiveEndDate,
                    SupOrg = hrStaff.SupOrg,
                    EmployeeEID = hrStaff.EmployeeEID,
                    Note = hrStaff.Note,
                    BudgetNumbers = hrStaff.BudgetNumbers
                };
                _context.Add(newCase);
                await _context.SaveChangesAsync();
                var cid = id;
                return RedirectToAction("Details", "Cases", new { id = cid, area = "" });
                //return RedirectToAction("Index", "Home");
            }
            return View(hrStaff);
        }
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            HRServiceStaff editCase = _context.HRServiceStaff.Find(id);
            if (editCase == null)
            {
                return NotFound();
            }
            return View(editCase);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CaseID,Description,EmployeeName,RequestType,BasePayChange,AllowanceChange,EffectiveStartDate,EffectiveEndDate,TerminationReason,Offboarding,Note,ClosePosition,LeaveWA,WorkerType,Amount,SupOrg,EmployeeEID,BudgetNumbers")] HRServiceStaff hrStaff)

        {
            string strAudit = "Case Edited. Values updated: ";
                       
            IQueryable<HRServiceStaff> beforeCases = _context.HRServiceStaff.Where(c => c.CaseID == id).AsNoTracking<HRServiceStaff>();
            HRServiceStaff beforeCase = beforeCases.FirstOrDefault();
            if (beforeCase == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                if (beforeCase.EmployeeName != hrStaff.EmployeeName)
                {
                    strAudit += " " + beforeCase.EmployeeName + ":" + hrStaff.EmployeeName;
                }

                if (beforeCase.RequestType.ToString() != hrStaff.RequestType.ToString())
                {
                    strAudit += " " + beforeCase.RequestType.ToString() + ":" + hrStaff.RequestType.ToString();
                }
                var audit = new CaseAudit { AuditLog = strAudit, CaseID = id, LocalUserID = User.Identity.Name };
                _context.Add(audit);
                _context.Entry(hrStaff).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                var cid = id;
                return RedirectToAction("Details", "Cases", new { id = cid, area = "" });
                //return RedirectToAction("Index", "Home");
            }
            return View(hrStaff);
        }
    }
}