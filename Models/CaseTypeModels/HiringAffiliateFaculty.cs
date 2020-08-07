using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Resolve.Models
{
    public enum FacAffiliateTitle
    {
        [Display(Name = "Affiliate Instructor")]
        AffiliateInstructor,
        [Display(Name = "Affiliate Assistant Professor")]
        AffiliateAssistProf,
        [Display(Name = "Affiliate Associate Professor")]
        AffiliateAssocProf,
        [Display(Name = "Affiliate Professor")]
        AffiliateProf,
        [Display(Name = "Affiliate in Medical/Dental Practice")]
        AffiliateMedDental
    }

    public class HiringAffiliateFaculty
    {
        [Required, Key, ForeignKey("Case")]
        public int CaseID { get; set; }
        public Case Case { get; set; }

        [Display(Name = "Proposed Hire Date")]
        [DataType(DataType.Date)]
        public DateTime HireDate { get; set; }

        public virtual Department Department { get; set; }

        public string Name { get; set; }

        public string Note { get; set; }

        [Display(Name = "Job Title")]
        public virtual FacAffiliateTitle FacAffiliateTitle { get; set; }
      
    }
}
