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
    public class PerioLimitedCareController : Controller
    {

        private readonly ResolveCaseContext _context;

        public PerioLimitedCareController(ResolveCaseContext context)
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
        public async Task<IActionResult> Create(int id, [Bind("StudentName,PatientName,PatientAddress,PatientPhone,BirthDate,Complaint,TreatmentPlan,bwxrays,paxrays,RestorativeExam,PerioExam,Prophy,Other,TChart,Note")] PerioLimitedCare perioLimitedCare)
        {
            if (ModelState.IsValid)
            {
                perioLimitedCare.CaseID = id;
                _context.Add(perioLimitedCare);
                await _context.SaveChangesAsync();
                var cid = id;
                return RedirectToAction("Details", "Cases", new { id = cid, area = "" });
                //return RedirectToAction("Index", "Home");
            }
            return View(perioLimitedCare);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            PerioLimitedCare editCase = _context.PerioLimitedCare.Find(id);
            if (editCase == null)
            {
                return NotFound();
            }
            return View(editCase);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CaseID,StudentName,PatientName,PatientAddress,PatientPhone,BirthDate,Complaint,TreatmentPlan,bwxrays,paxrays,RestorativeExam,PerioExam,Prophy,Other,TChart.Note")] PerioLimitedCare perioLimitedCare)

        {
            /** First check important fields to see if values have changed and if so add to audit log **/
            string strAudit = "Case Edited. Values updated (old,new). ";

            IQueryable<PerioLimitedCare> beforeCases = _context.PerioLimitedCare.Where(c => c.CaseID == id).AsNoTracking<PerioLimitedCare>();
            PerioLimitedCare beforeCase = beforeCases.FirstOrDefault();
            if (beforeCase == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                if (beforeCase.StudentName != perioLimitedCare.StudentName)
                {
                    strAudit += "Student: (" + beforeCase.StudentName + "," + perioLimitedCare.StudentName + ") ";
                }

                if (beforeCase.PatientName != perioLimitedCare.PatientName)
                {
                    strAudit += "Patient: (" + beforeCase.PatientName + "," + perioLimitedCare.PatientName + ") ";
                }
                if (beforeCase.PatientAddress != perioLimitedCare.PatientAddress)
                {
                    strAudit += "PatientAddress: (" + beforeCase.PatientAddress + "," + perioLimitedCare.PatientAddress + ") ";
                }
                if (beforeCase.PatientPhone != perioLimitedCare.PatientPhone)
                {
                    strAudit += "PatientPhone: (" + beforeCase.PatientPhone + "," + perioLimitedCare.PatientPhone + ") ";
                }
                if (beforeCase.BirthDate.ToShortDateString() != perioLimitedCare.BirthDate.ToShortDateString())
                {
                    strAudit += "BirthDate: (" + beforeCase.BirthDate.ToShortDateString() + "," + perioLimitedCare.BirthDate.ToShortDateString() + ") ";
                }
                if (beforeCase.Complaint != perioLimitedCare.Complaint)
                {
                    strAudit += "Complaint: (" + beforeCase.Complaint + "," + perioLimitedCare.Complaint + ") ";
                }
                if (beforeCase.TreatmentPlan != perioLimitedCare.TreatmentPlan)
                {
                    strAudit += "TreatmentPlan: (" + beforeCase.TreatmentPlan + "," + perioLimitedCare.TreatmentPlan + ") ";
                }
                if (beforeCase.TChart != perioLimitedCare.TChart)
                {
                    strAudit += "TChart: (" + beforeCase.TChart + "," + perioLimitedCare.TChart + ") ";
                }
                string bcbwxrays = beforeCase.bwxrays.ToString();
                string bcpaxrays = beforeCase.paxrays.ToString();
                string bcPerioExam = beforeCase.PerioExam.ToString();
                string bcRestoreExam = beforeCase.RestorativeExam.ToString();
                string bcProphy = beforeCase.Prophy.ToString();
                string bcOther = beforeCase.Other.ToString();
                string currProphy = perioLimitedCare.Prophy.ToString();
                string currOther = perioLimitedCare.Other.ToString();
                string currRestoreExam = perioLimitedCare.RestorativeExam.ToString();
                string curbwxrays = perioLimitedCare.bwxrays.ToString();
                string currpaxrays = perioLimitedCare.paxrays.ToString();
                string currPerioExam = perioLimitedCare.PerioExam.ToString();

                if (bcbwxrays != curbwxrays)
                {
                    strAudit += "BW x-rays: (" + bcbwxrays + "," + curbwxrays + ") ";
                }
                if (bcpaxrays != currpaxrays)
                {
                    strAudit += "PA x-rays: (" + bcpaxrays + "," + currpaxrays + ") ";
                }
                if (bcPerioExam != currPerioExam)
                {
                    strAudit += "PerioExam: (" + bcPerioExam + "," + currPerioExam + ") ";
                }
                if (bcRestoreExam != currRestoreExam)
                {
                    strAudit += "RestoreExam: (" + bcRestoreExam + "," + currRestoreExam + ") ";
                }
                if (bcProphy != currProphy)
                {
                    strAudit += "Prophy: (" + bcProphy + "," + currProphy + ") ";
                }
                if (bcOther != currOther)
                {
                    strAudit += "Other: (" + bcOther + "," + currOther + ") ";
                }

                var audit = new CaseAudit { AuditLog = strAudit, CaseID = id, LocalUserID = User.Identity.Name };
                _context.Add(audit);
                _context.Entry(perioLimitedCare).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                var cid = id;
                return RedirectToAction("Details", "Cases", new { id = cid, area = "" });
                //return RedirectToAction("Index", "Home");
            }
            return View(perioLimitedCare);
        }

    }
}