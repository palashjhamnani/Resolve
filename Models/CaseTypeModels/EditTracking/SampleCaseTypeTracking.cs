using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Resolve.Models
{
    public class SampleCaseTypeTracking
    {
        public int SampleCaseTypeTrackingID { get; set; }
        public string Status { get; set; }
        public int CaseAuditID { get; set; }
        public CaseAudit CaseAudit { get; set; }
        public int CaseID { get; set; }
        public Case Case { get; set; }

        [Display(Name = "Case Description"), Required]
        public string CaseDescription { get; set; }
        public string EmployeeName { get; set; }
        public int Salary { get; set; }
    }
}
