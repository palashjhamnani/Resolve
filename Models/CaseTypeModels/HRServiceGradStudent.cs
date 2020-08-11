using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Resolve.Models
{
    public enum GradRequestType
    {
        [Display(Name = "Distribution Change")]
        Distribution,
        [Display(Name = "Extend End Date")]
        Extend,
        [Display(Name = "New Hire")]
        Newhire,
        [Display(Name = "Rehire")]
        Rehire,
        [Display(Name = "Termination")]
        Termination,

    }

    public enum GradJobProfile
    {
        [Display(Name = "Graduate Fellow/Trainee Stipend w/o Benefits")]
        FellowStudentNoBenefits,
        [Display(Name = "Graduate Fellow Stipend w / Benefits(Historical 0863 job code)")]
        FellowStudentBenefits,
        [Display(Name = "Graduate Trainee Stipend (NE UAW ASE)")]
        GradTraineeStipend,
        [Display(Name = "Graduate Trainee Stipend w/Benefits (Historical 0864 job code)")]
        GradTraineeBenefits
    }

    public enum Department
    {
        [Display(Name = "Endodontics")]
        Endodontics,
        [Display(Name = "Office of Research")]
        Research,
        [Display(Name = "Oral and Maxillofacial Surgery")]
        OMS,
        [Display(Name = "Oral Health Sciences")]
        OHS,
        [Display(Name = "Oral Medicine")]
        OMed,
        [Display(Name = "Orthodontics")]
        Ortho,
        [Display(Name = "Pediatric Dentistry")]
        Pedo,
        [Display(Name = "Periodontics")]
        Perio,
        [Display(Name = "Restorative Dentistry")]
        Restore
    }
    public class HRServiceGradStudent
    {
        [Required, Key, ForeignKey("Case")]
        public int CaseID { get; set; }
        public Case Case { get; set; }

        [Display(Name = "Effective Start Date")]
        [DataType(DataType.Date)]
        public DateTime EffectiveStartDate { get; set; }

        [Display(Name = "Effective End Date")]
        [DataType(DataType.Date)]
        public DateTime? EffectiveEndDate { get; set; }

        [Display(Name = "Request Type"), Required]
        public GradRequestType GradRequestType { get; set; }

        [Display(Name = "Job Profile"), Required]
        public virtual GradJobProfile GradJobProfile { get; set; }

        public virtual Department Department { get; set; }

        [Display(Name = "Student Name"), Required]
        public string StudentName { get; set; }

        [Display(Name = "Step/Stipend/Allowance")]
        public string StepStipendAllowance { get; set; }

        public string BudgetNumbers { get; set; }

        [MaxLength(1024)]
        public string Note { get; set; }
    }
}
