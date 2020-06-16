using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Resolve.Data;
using Resolve.Models;
using Resolve.ViewModels;
using Microsoft.AspNetCore.Hosting;
using static Microsoft.AspNetCore.Hosting.IWebHostEnvironment;
using Microsoft.AspNetCore.Http;

namespace Resolve.Controllers
{
    public class CaseAttachmentsController : Controller
    {
        private readonly ResolveCaseContext _context;
        private readonly IHostingEnvironment hostingEnvironment;

        public CaseAttachmentsController(ResolveCaseContext context, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            this.hostingEnvironment = hostingEnvironment;
        }

        // GET: CaseAttachments
        public async Task<IActionResult> Index()
        {
            var resolveCaseContext = _context.CaseAttachment.Include(c => c.Case).Include(c => c.LocalUser);
            return View(await resolveCaseContext.ToListAsync());
        }

        // GET: CaseAttachments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var caseAttachment = await _context.CaseAttachment
                .Include(c => c.Case)
                .Include(c => c.LocalUser)
                .FirstOrDefaultAsync(m => m.CaseAttachmentID == id);
            if (caseAttachment == null)
            {
                return NotFound();
            }

            return View(caseAttachment);
        }

        // GET: CaseAttachments/Create
        public IActionResult Create(int? id)
        {
            //ViewData["CaseID"] = new SelectList(_context.Case, "CaseID", "CaseID");
            //ViewData["LocalUserID"] = new SelectList(_context.LocalUser, "LocalUserID", "LocalUserID");
            return View();
        }

        // POST: CaseAttachments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int id, CaseAttachmentCreateViewModel model)
        {
            if (id == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                string uniqueFileName = null;
                if (model.Attachments != null && model.Attachments.Count > 0)
                {
                    string uFolder = Path.Combine(hostingEnvironment.WebRootPath, "Attachments");
                    //if (!Directory.Exists(uFolder))
                    //{
                    //    Directory.CreateDirectory(uFolder);
                    //}

                    foreach (IFormFile Attachment in model.Attachments)
                    {
                        string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "Attachments");
                        uniqueFileName = Guid.NewGuid().ToString() + "_" + Attachment.FileName;
                        //uniqueFileName = Attachment.FileName;
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                        // Use CopyTo() method provided by IFormFile interface to
                        // copy the file to wwwroot/images folder
                        Attachment.CopyTo(new FileStream(filePath, FileMode.Create));
                        CaseAttachment newAttachment = new CaseAttachment
                        {
                            LocalUserID = User.Identity.Name,
                            CaseID = id,
                            // Store the file name in PhotoPath property of the employee object
                            // which gets saved to the Employees database table
                            FilePath = uniqueFileName,
                            FileName = Attachment.FileName
                        };
                        _context.Add(newAttachment);

                    }
                    
                    await _context.SaveChangesAsync();
                }
                int cid = id;
                return RedirectToAction("Details", "Cases", new { id = cid });
            }
            ViewData["CaseID"] = new SelectList(_context.Case, "CaseID", "CaseID", model.CaseID);
            //ViewData["LocalUserID"] = new SelectList(_context.LocalUser, "LocalUserID", "LocalUserID", caseAttachment.LocalUserID);
            return View();
        }

        // GET: CaseAttachments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var caseAttachment = await _context.CaseAttachment.FindAsync(id);
            if (caseAttachment == null)
            {
                return NotFound();
            }
            ViewData["CaseID"] = new SelectList(_context.Case, "CaseID", "CaseID", caseAttachment.CaseID);
            ViewData["LocalUserID"] = new SelectList(_context.LocalUser, "LocalUserID", "LocalUserID", caseAttachment.LocalUserID);
            return View(caseAttachment);
        }

        // POST: CaseAttachments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CaseAttachmentID,CaseID,LocalUserID,FilePath,AttachmentTimestamp")] CaseAttachment caseAttachment)
        {
            if (id != caseAttachment.CaseAttachmentID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(caseAttachment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CaseAttachmentExists(caseAttachment.CaseAttachmentID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CaseID"] = new SelectList(_context.Case, "CaseID", "CaseID", caseAttachment.CaseID);
            ViewData["LocalUserID"] = new SelectList(_context.LocalUser, "LocalUserID", "LocalUserID", caseAttachment.LocalUserID);
            return View(caseAttachment);
        }
        // GET: CaseAttachments/Download/5
        public async Task<IActionResult> Download(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var caseAttachment = await _context.CaseAttachment.FindAsync(id);
            if (caseAttachment == null)
            {
                return NotFound();
            }
            string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "Attachments");

            string filePath = Path.Combine(uploadsFolder, caseAttachment.FilePath);
            var memory = new MemoryStream();
            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, GetContentType(filePath), Path.GetFileName(filePath));
        }
        private string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }

        private Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", "application/vnd.openxmlformats officedocument.spreadsheetml.sheet"},
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".csv", "text/csv"}
            };
        }
        // GET: CaseAttachments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var caseAttachment = await _context.CaseAttachment
                .Include(c => c.Case)
                .Include(c => c.LocalUser)
                .FirstOrDefaultAsync(m => m.CaseAttachmentID == id);
            if (caseAttachment == null)
            {
                return NotFound();
            }

            return View(caseAttachment);
        }

        // POST: CaseAttachments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var caseAttachment = await _context.CaseAttachment.FindAsync(id);
            _context.CaseAttachment.Remove(caseAttachment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CaseAttachmentExists(int id)
        {
            return _context.CaseAttachment.Any(e => e.CaseAttachmentID == id);
        }

        public FileResult Download(int id)
        {
            var caseAttachment = _context.CaseAttachment
                .FirstOrDefault(m => m.CaseAttachmentID == id);
            string filename = caseAttachment.FilePath;
            var filepath = $"wwwroot/Attachments/{filename}";
            byte[] fileBytes = System.IO.File.ReadAllBytes(filepath);
            return File(fileBytes, "application/x-msdownload", filename);
        }

        [HttpPost, ActionName("QuickDelete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> QuickDelete()
        {
            var s_cid = HttpContext.Request.Form["CID"];
            int cid = Convert.ToInt32(s_cid);
            var s_caid = HttpContext.Request.Form["CAID"];
            int caid = Convert.ToInt32(s_caid);
            var caseAttachment = await _context.CaseAttachment.FindAsync(caid);
            var filename = caseAttachment.FilePath;
            //string uFolder = Path.Combine(hostingEnvironment.WebRootPath, "Attachments");
            //string fullPath = Path.Combine(uFolder, filename).Replace(@"\\", @"\\\\");
            var fullPath = $"wwwroot/Attachments/{filename}";
            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
                _context.CaseAttachment.Remove(caseAttachment);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Cases", new { id = cid, approved = 3 });
            }
            return RedirectToAction("Details", "Cases", new { id = cid, approved = 0});
        }
    }
}