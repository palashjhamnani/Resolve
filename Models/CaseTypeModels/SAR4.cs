using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Resolve.Models
{
    public class SAR4
    {
        [Required, Key, ForeignKey("Case")]
        public int CaseID { get; set; }
        public Case Case { get; set; }

        [Display(Name = "Absence Description"), Required]
        public string AbsenceDescription { get; set; }

        [Display(Name = "Request Start Date")]
        [DataType(DataType.Date)]
        public DateTime RequestStartDate { get; set; }

        [Display(Name = "Request End Date")]
        [DataType(DataType.Date)]
        public DateTime RequestEndDate { get; set; }

        [Display(Name = "Make-up Plan")]
        public string MakeupPlan { get; set; }

        [Display(Name = "Absences Requested")]
        public int AbsenceRequested { get; set; }

        public string Student { get; set; }

        public int GradYear { get; set; }

        public string Quarter { get; set; }

        public string AbsenceReason { get; set; }


    }
}

