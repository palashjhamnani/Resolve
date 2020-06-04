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
    public class SampleCaseTypeController : Controller
    {
        
        private readonly ResolveCaseContext _context;

        public SampleCaseTypeController(ResolveCaseContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        // GET: Cases/CreateCaseTypeData
        public IActionResult Create(int id)
        {
            return View();
        }
        // POST: Cases/CreateCaseTypeData
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int id, SampleCaseType samplecasetype)
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
                return RedirectToAction("Details", "Cases", new { id = cid, area = "" });
            }
            return View(samplecasetype);
        }

    }
}