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
    public class ReportsController : Controller
    {
        private readonly ResolveCaseContext _context;

        public ReportsController(ResolveCaseContext context)
        {
            _context = context;
        }

        // GET: LocalGroups
        public async Task<IActionResult> Index()
        {
            List<int> status = new List<int>();
            var pending = _context.Case.Where(p => p.CaseStatus == "Pending").Count();
            var approved = _context.Case.Where(p => p.CaseStatus == "Approved").Count();
            var rejected = _context.Case.Where(p => p.CaseStatus == "Rejected").Count();
            var cancelled = _context.Case.Where(p => p.CaseStatus == "Cancelled").Count();
            status.Add(pending);
            status.Add(approved);
            status.Add(rejected);
            status.Add(cancelled);
            ViewData["case_status"] = status;
            var case_type_list = await _context.CaseType
                .Include(e => e.Cases)
                .ToListAsync();
            return View(case_type_list);
        }


    }
}
