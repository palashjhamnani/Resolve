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
    public class EmailPreferencesController : Controller
    {
        private readonly ResolveCaseContext _context;

        public EmailPreferencesController(ResolveCaseContext context)
        {
            _context = context;
        }


        // GET: EmailPreferences/Edit/{UserID}
        public async Task<IActionResult> Preferences(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pref = await _context.EmailPreference.FindAsync(id);
            if (pref == null)
            {
                return NotFound();
            }            
            return View(pref);
        }

        // POST: EmailPreferences/Edit/{UserID}
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Preferences(string id, [Bind("LocalUserID,CaseCreation,CaseAssignment,CommentCreation,AttachmentCreation,CaseProcessed")] EmailPreference pref)
        {
            //pref.LocalUserID = User.Identity.Name;
            if (id != pref.LocalUserID)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                Console.WriteLine("valid");
                try
                {                    
                    _context.Update(pref);
                    await _context.SaveChangesAsync();                    
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmailPreferenceExists(pref.LocalUserID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", "LocalUsers", new { id = User.Identity.Name });
            }
            //ViewData["LocalUserID"] = new SelectList(_context.LocalUser, "LocalUserID", "LocalUserID", localGroup.LocalUserID);
            Console.WriteLine("invalid");
            return View(pref);
        }

        private bool EmailPreferenceExists(string id)
        {
            return _context.EmailPreference.Any(e => e.LocalUserID == id);
        }
    }
}
