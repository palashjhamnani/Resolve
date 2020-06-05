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
    public class SAR4Controller : Controller
    {

        private readonly ResolveCaseContext _context;

        public SAR4Controller(ResolveCaseContext context)
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
        public async Task<IActionResult> Create(int id, SAR4 sar4)
        {
            if (ModelState.IsValid)
            {
                SAR4 newCase = new SAR4
                {
                    CaseID = id,
                    AbsenceDescription = sar4.AbsenceDescription,
                    Student = sar4.Student,
                    Quarter = sar4.Quarter,
                    RequestStartDate = sar4.RequestStartDate,
                    RequestEndDate = sar4.RequestEndDate,
                    AbsenceReason = sar4.AbsenceReason,
                    GradYear = sar4.GradYear
                };
                _context.Add(newCase);
                await _context.SaveChangesAsync();
                var cid = id;
                return RedirectToAction("Details", "Cases", new { id = cid, area = "" });
                //return RedirectToAction("Index", "Home");
            }
            return View(sar4);
        }



    }
}