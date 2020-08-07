using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Resolve.Models
{
    public enum ScholarRequestType
    {
        [Display(Name = "Conpensation Allowance Change")]
        Compensation,
        [Display(Name = "Distribution Change")]
        Distribution,
        [Display(Name = "Extend End Date")]
        Extend,
        [Display(Name = "FTE Change")]
        FTE,
        [Display(Name = "New Hire")]
        Newhire,
        [Display(Name = "Promotion")]
        Promotion,
        [Display(Name = "Rehire")]
        Rehire,
        [Display(Name = "Termination")]
        Termination,
    }

    public enum ScholarJobProfile
    {
        [Display(Name = "Resident")]
        Residents,
        [Display(Name = "Chief Resident")]
        ChiefResident,
        [Display(Name = "Postdoctoral Scholar")]
        PostScholar,
        [Display(Name = "Postdoctoral Scholar Fellow")]
        PostFellow,
        [Display(Name = "Interim Postdoctoral Scholar")]
        InterimPostFellow
    }
    public enum ScholarCompAllowanceChange
    {
        [Display(Name = "Chief Resident Supplement")]
        ChiefSup,
        [Display(Name = "Other")]
        Other
    }


    public class HRServiceScholarResident
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
        public ScholarRequestType ScholarRequestType { get; set; }

        [Display(Name = "Job Profile"), Required]
        public virtual ScholarJobProfile ScholarJobProfile { get; set; }

        [Display(Name = "Compensation Allowance Change")]
        public virtual ScholarCompAllowanceChange ScholarCompAllowanceChange { get; set; }

        public virtual Department Department { get; set; }

        [Display(Name = "Current Job Title")]
        public string JobTitle { get; set; }

        [Display(Name = "Proposed Job Title")]
        public string PropTitle { get; set; }

        [Display(Name = "Current FTE")]
        public string CurrentFTE { get; set; }

        [Display(Name = "Proposed FTE")]
        public string ProposedFTE { get; set; }

        [Display(Name = "Name"), Required]
        public string Name { get; set; }

        [Display(Name = "Step/Stipend/Allowance")]
        public string StepStipendAllowance { get; set; }

        public string BudgetNumbers { get; set; }

        [MaxLength(1024)]
        public string Note { get; set; }
    }
}
