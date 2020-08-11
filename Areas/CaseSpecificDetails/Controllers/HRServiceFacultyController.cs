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

                    EmployeeName = hrFaculty.EmployeeName,
                    FacRequestType = hrFaculty.FacRequestType,
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
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            HRServiceFaculty editCase = _context.HRServiceFaculty.Find(id);
            if (editCase == null)
            {
                return NotFound();
            }
            return View(editCase);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CaseID,EmployeeName,FacRequestType,EffectiveStartDate,EffectiveEndDate,TerminationReason,Offboarding,Note,ClosePosition,LeaveWA,Salary,Amount,SupOrg,Department,CurrentFTE,ProposedFTE,JobTitle,EmployeeEID,BudgetNumbers")] HRServiceFaculty hrFaculty)

        {
            IQueryable<HRServiceFaculty> beforeCases = _context.HRServiceFaculty.Where(c => c.CaseID == id).AsNoTracking<HRServiceFaculty>();
            HRServiceFaculty beforeCase = beforeCases.FirstOrDefault();
            if (beforeCase == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                /** First check fields to see if values have changed and if so add to audit log  **/
               /** Also clear any fields that were hidden when and not automatically reset in the form **/

                string strAudit = "Case Edited. Values updated (old,new). ";
               
                string bcRequest = beforeCase.FacRequestType.ToString();
                string currRequest = hrFaculty.FacRequestType.ToString();              
                string bcTerm = beforeCase.TerminationReason.ToString();
                string currTerm = hrFaculty.TerminationReason.ToString();
                string bcOff = beforeCase.Offboarding.ToString();
                string currOff = hrFaculty.Offboarding.ToString();
                string bcClose = beforeCase.ClosePosition.ToString();
                string currClose = hrFaculty.ClosePosition.ToString();
                string bcLeave = beforeCase.LeaveWA.ToString();
                string currLeave = hrFaculty.LeaveWA.ToString();
                string bcAmount = "";
                string currAmount = "";
                string bcCurrFTE = "";
                string currCurrFTE = "";
                string bcPropFTE = "";
                string currPropFTE = "";
                string bcSup = beforeCase.SupOrg.ToString();
                string currSup = hrFaculty.SupOrg.ToString();
                if (!String.IsNullOrEmpty(beforeCase.Amount))
                {
                    bcAmount = beforeCase.Amount;
                }
                if (!String.IsNullOrEmpty(hrFaculty.Amount))
                {
                    currAmount = hrFaculty.Amount;
                }
                if (!String.IsNullOrEmpty(beforeCase.CurrentFTE))
                {
                    bcCurrFTE = beforeCase.CurrentFTE;
                }
                if (!String.IsNullOrEmpty(beforeCase.ProposedFTE))
                {
                    bcPropFTE = beforeCase.ProposedFTE;
                }
                if (!String.IsNullOrEmpty(hrFaculty.CurrentFTE))
                {
                    currCurrFTE = hrFaculty.CurrentFTE;
                }
                if (!String.IsNullOrEmpty(hrFaculty.ProposedFTE))
                {
                    currPropFTE = hrFaculty.ProposedFTE;
                }


                if (beforeCase.EmployeeName != hrFaculty.EmployeeName)
                {
                    strAudit += "Employee: (" + beforeCase.EmployeeName + "," + hrFaculty.EmployeeName + ") ";
                }

                if (bcRequest != currRequest)
                {
                    strAudit += "RequestType: (" + beforeCase.FacRequestType.ToString() + "," + hrFaculty.FacRequestType.ToString() + ") ";
                }
                if (currRequest != "Termination")
                {
                    hrFaculty.TerminationReason = null;
                    currTerm = "";
                    hrFaculty.Offboarding = false;
                    currOff = "False"; 
                    hrFaculty.ClosePosition = false;
                    currClose = "False";
                    hrFaculty.LeaveWA = false;
                    currLeave = "False";
                }
               
               
                if (currRequest != "FTE")
                {
                    hrFaculty.CurrentFTE = "";
                    hrFaculty.ProposedFTE = "";
                    currCurrFTE = "";
                    currPropFTE = "";
                }
                if (currRequest != "Move")
                {
                    hrFaculty.SupOrg = null;
                    currSup = "";
                }
               
                if (bcAmount != currAmount)
                {
                    strAudit += "Amount: (" + bcAmount + "," + currAmount + ") ";
                }
               
                if (bcTerm != currTerm)
                {
                    strAudit += "TerminationReason: (" + bcTerm + "," + currTerm + ") ";
                }
                if (bcOff != currOff)
                {
                    strAudit += "Offboarding: (" + bcOff + "," + currOff + ") ";
                }
                if (bcLeave != currLeave)
                {
                    strAudit += "LeaveWA: (" + bcLeave + "," + currLeave + ") ";
                }
                if (bcClose != currClose)
                {
                    strAudit += "ClosePosition: (" + bcClose + "," + currClose + ") ";
                }
                if (bcCurrFTE != currCurrFTE)
                {
                    strAudit += "CurrentFTE: (" + bcCurrFTE + "," + currCurrFTE + ") ";
                }
                if (bcPropFTE != currPropFTE)
                {
                    strAudit += "PurposedFTE: (" + bcPropFTE + "," + currPropFTE + ") ";
                }
                if (bcSup != currSup)
                {
                    strAudit += "SupOrg: (" + bcSup+ "," + currSup + ") ";
                }
                if (beforeCase.EffectiveStartDate.ToShortDateString() != hrFaculty.EffectiveStartDate.ToShortDateString())
                {
                    strAudit += "StartDate: (" + beforeCase.EffectiveStartDate.ToShortDateString() + "," + hrFaculty.EffectiveStartDate.ToShortDateString() + ") ";
                }
                DateTime beforeEffectDate = beforeCase.EffectiveEndDate.GetValueOrDefault();
                DateTime curEffectDate = hrFaculty.EffectiveEndDate.GetValueOrDefault();
                if (beforeEffectDate.ToShortDateString() != curEffectDate.ToShortDateString())
                {
                    strAudit += "EndDate: (" + beforeEffectDate.ToShortDateString() + "," + curEffectDate.ToShortDateString() + ") ";
                }

                if (beforeCase.Department.ToString() != hrFaculty.Department.ToString())
                {
                    strAudit += "Department: (" + beforeCase.Department.ToString() + "," + hrFaculty.Department.ToString() + ") ";
                }

                if (!String.IsNullOrEmpty(beforeCase.Salary) && !String.IsNullOrEmpty(hrFaculty.Salary))
                {
                    if (beforeCase.Salary.ToString() != hrFaculty.Salary.ToString())
                    {
                        strAudit += "Salary: (" + beforeCase.Salary.ToString() + "," + hrFaculty.Salary.ToString() + ") ";
                    }
                }
                if (beforeCase.EmployeeEID.ToString() != hrFaculty.EmployeeEID.ToString())
                {
                    strAudit += "EmployeeEID: (" + beforeCase.EmployeeEID.ToString() + "," + hrFaculty.EmployeeEID.ToString() + ") ";
                }
                if (beforeCase.BudgetNumbers.ToString() != hrFaculty.BudgetNumbers.ToString())
                {
                    strAudit += "Budget#: (" + beforeCase.BudgetNumbers.ToString() + "," + hrFaculty.BudgetNumbers.ToString() + ") ";
                }
                if (beforeCase.JobTitle.ToString() != hrFaculty.JobTitle.ToString())
                {
                    strAudit += "JobTitle: (" + beforeCase.JobTitle.ToString() + "," + hrFaculty.JobTitle.ToString() + ") ";
                }

                var audit = new CaseAudit { AuditLog = strAudit, CaseID = id, LocalUserID = User.Identity.Name };
                _context.Add(audit);
                _context.Entry(hrFaculty).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                var cid = id;
                return RedirectToAction("Details", "Cases", new { id = cid, area = "" });
                //return RedirectToAction("Index", "Home");
            }
            return View(hrFaculty);
        }

    }
}