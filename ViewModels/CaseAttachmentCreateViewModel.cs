using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Resolve.Models;
using System.ComponentModel.DataAnnotations;

namespace Resolve.ViewModels
{
    public class CaseAttachmentCreateViewModel
    {
        public int CaseID { get; set; }
        public Case Case { get; set; }
        [Required]
        public List<IFormFile> Attachments { get; set; }
    }
}
