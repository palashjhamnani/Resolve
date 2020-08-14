using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Resolve.Models
{
    public class PerioLimitedCare
    {
        [Required, Key, ForeignKey("Case")]
        public int CaseID { get; set; }
        public Case Case { get; set; }

        [Display(Name = "Birth Date")]
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

        [Display(Name = "Student Name"), Required]
        public string StudentName { get; set; }

        [Display(Name = "Patient Name")]
        public string PatientName { get; set; }

        [Display(Name = "Patient Address")]
        public string PatientAddress { get; set; }

        [Display(Name = "Patient Phone")]
        public string PatientPhone { get; set; }

        [Display(Name = "Chief Complaint")]
        public string Complaint { get; set; }

        [Display(Name = "Treatment Plan")]
        public string TreatmentPlan { get; set; }

        [Display(Name = "Temp/Chart#")]
        public string TChart { get; set; }

        [Display(Name = "Restorative Exam")]
        public bool RestorativeExam { get; set; }

        [Display(Name = "Perio Exam")]
        public bool PerioExam { get; set; }

        [Display(Name = "4 BW x-rays")]
        public bool bwxrays { get; set; }

        [Display(Name = "PA x-rays")]
        public bool paxrays { get; set; }

        [Display(Name = "Prophy")]
        public bool Prophy { get; set; }

        [Display(Name = "Other")]
        public bool Other { get; set; }

        [Display(Name = "Other Explanation")]
        public string OtherProcedure { get; set; }
       
        public string Note { get; set; }

    }
}
