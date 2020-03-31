using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Resolve.Models
{
    public class SampleCaseType
    {
        
        [Key]
        public int CaseID { get; set; }
        public Case Case { get; set; }

        [Display(Name = "Case Description"), Required]
        public string CaseDescription { get; set; }

    }
}
