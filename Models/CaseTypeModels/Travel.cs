using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Resolve.Models
{
    public enum BudgetType
    {
        [Display(Name = "Clinic")]
        Clinic,
        [Display(Name = "GOF")]
        GOF,
        [Display(Name = "Gift")]
        Gift,
        [Display(Name = "Grant")]
        Grant,
        [Display(Name = "Multiple")]
        Multiple,
        [Display(Name = "Other")]
        Other
    }

    public enum BudgetPurpose
    {
        [Display(Name = "Administration")]
        Administration,
        [Display(Name = "Clinical")]
        Clinical,
        [Display(Name = "Research")]
        Research,
        [Display(Name = "Teaching")]
        Teaching,
        [Display(Name = "Multiple")]
        Multiple,
        [Display(Name = "Other")]
        Other
    }

    public class Travel

    {
        [Required, Key, ForeignKey("Case")]
        public int CaseID { get; set; }
        public Case Case { get; set; }

        [Display(Name = "Travel Start Date")]
        [DataType(DataType.Date)]
        public DateTime TravelStartDate { get; set; }

        [Display(Name = "Travel End Date")]
        [DataType(DataType.Date)]
        public DateTime? TravelEndDate { get; set; }

        public virtual Department Department { get; set; }

        public virtual BudgetType BudgetType { get; set; }

        public virtual BudgetPurpose BudgetPurpose { get; set; }

        public string EmployeeEID { get; set; }

        [Display(Name = "Employee Name"), Required]
        public string EmployeeName { get; set; }

        [Display(Name = "Budget Numbers")]
        public string BudgetNumbers { get; set; }

        public string Note { get; set; }

        public string Destination { get; set; }

        public string Reason { get; set; }

        public int Airfare { get; set; }

        public int Registration { get; set; }

        public int Transportation { get; set; }

        public int Meals { get; set; }

        public int Hotels { get; set; }

        public int Other1 { get; set; }

        public int Other2 { get; set; }

        public int Total { get; set; }
      
    }
}
