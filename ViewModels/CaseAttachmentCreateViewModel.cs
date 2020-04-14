using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Resolve.Models;

namespace Resolve.ViewModels
{
    public class CaseAttachmentCreateViewModel
    {
        public int CaseID { get; set; }
        public Case Case { get; set; }
        public IFormFile Attachment { get; set; }
    }
}
