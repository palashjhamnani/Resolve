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
    public class CaseSpecificDetailsController : Controller
    {
        private readonly ResolveCaseContext _context;

        public CaseSpecificDetailsController(ResolveCaseContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        // Enter data for a specific case type
        // GET: Cases/CreateCaseTypeData
        public IActionResult SampleCaseType(int id)
        {
            return View();
        }
        // POST: Cases/CreateCaseTypeData
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SampleCaseType(int id, SampleCaseType samplecasetype)
        {
            if (ModelState.IsValid)
            {
                SampleCaseType newCase = new SampleCaseType
                {
                    CaseID = id,
                    CaseDescription = samplecasetype.CaseDescription
                };
                _context.Add(newCase);
                await _context.SaveChangesAsync();
                var cid = id;
                return RedirectToAction("Details", "Cases", new { id = cid });
                //return RedirectToAction("Index", "Home");
            }
            return View(samplecasetype);
        }





        public IActionResult Sample2(int id)
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Sample2(int id, Sample2 sample2)
        {
            if (ModelState.IsValid)
            {
                Sample2 newCase = new Sample2
                {
                    CaseID = id,
                    SampleDescription = sample2.SampleDescription
                };
                _context.Add(newCase);
                await _context.SaveChangesAsync();
                var cid = id;
                return RedirectToAction("Details", "Cases", new { id = cid });
                //return RedirectToAction("Index", "Home");
            }
            return View(sample2);
        }



    }
}