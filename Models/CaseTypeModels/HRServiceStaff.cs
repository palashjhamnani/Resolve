using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Resolve.Models
{
    public enum RequestType
    {
        [Display(Name = "Compensation Base Pay Change")]
        Base,
        [Display(Name = "Compensation Allowance Change")]
        Allowance,
        [Display(Name = "Distribution Change")]
        Distribution,
        [Display(Name = "Employment End Date Change")]
        Enddate,
        [Display(Name = "FTE Change")]
        FTE,
        [Display(Name = "Move Worker")]
        Move,
        [Display(Name = "Termination")]
        Termination,
        [Display(Name = "Other")]
        Other
    }
    public enum BasePayChange
    {
        [Display(Name = "Ingrade – Change in Responsibilities")]
        IngradeChange,
        [Display(Name = "Ingrade – Competitive Offer(non-UW)")]
        IngradeCompete,
        [Display(Name = "Ingrade – Market/Retention")]
        IngradeRetain,
        [Display(Name = "Ingrade – Merit/Increased Functioning")]
        IngradeMerit,
        [Display(Name = "Ingrade – Preemptive Offer(non-UW)")]
        IngradePrempt,
        [Display(Name = "Internal Equity")]
        InternalEquity,
        [Display(Name = "Salary / Hourly Rate Reduction")]
        HourlyReduction,
        [Display(Name = "Step Adjustment – Career Enhancement/Growth Program(CEGP)")]
        StepCareer,
        [Display(Name = "Step Adjustment – Contractual Change")]
        StepContract,
        [Display(Name = "Step Adjustment – Other")]
        StepOther,
        [Display(Name = "Step Adjustment –Recruitment / Retention Adjustment")]
        StepRecruit,
        [Display(Name = "Maintain Progression Start Date")]
        MaintainProgress
    }
    public enum AllowanceChange
    {
        [Display(Name = "Administrative Supplement")]
        AdministrativeSupplement,
        [Display(Name = "Alaska RN Assignment Pay")]
        AlaskaRN,
        [Display(Name = "Car Allowance")]
        CarAllowance,
        [Display(Name = "Chief Resident Supplement")]
        ChiefResident,
        [Display(Name = "Car Allowance")]
        DentistryPractice,
        [Display(Name = " Employment Contract Allowance")]
        EmploymentContract,
        [Display(Name = "Endowed Supplement")]
        EndowedSupplement,
        [Display(Name = "Excess Compensation")]
        ExcessCompensation,
        [Display(Name = "Extension Lecture Summer Quarter Premium")]
        ExtensionPremium,
        [Display(Name = "International Location Allowance")]
        InternationalAllowance,
        [Display(Name = "K9 Officer Premium")]
        K9Premium,
        [Display(Name = "Language Premium")]
        LanguagePremium,
        [Display(Name = "Mobile Service Agreement")]
        MobileService,
        [Display(Name = "Teaching Assistant Summer Quarter Premium")]
        TeachingAssistant,
        [Display(Name = "Temporary Pay Increase")]
        TemporaryIncrease,
        [Display(Name = "Temporary Pay Supplement")]
        TemporarySupplement,
        [Display(Name = "Temporary Recruitment and Retention Increase CNU")]
        TemporaryRetention,
        [Display(Name = "Temporary Salary Increase")]
        TemporarySalary,
        [Display(Name = "UWPMA/Teamsters117 Longevity Pay")]
        LongevityPay
    }

    public enum WorkerType
    {
        [Display(Name = "Student")]
        Student,
        [Display(Name = "Classified")]
        Classified,
        [Display(Name = "Professional")]
        Professional
    }

    public enum SupOrg
    {
        Sup1, Sup2, Sup3, Sup4
    }


    public class HRServiceStaff
    {
        
        [Required, Key, ForeignKey("Case")]
        public int CaseID { get; set; }
        public Case Case { get; set; }

        [Display(Name = "Description"), Required]
        public string Description { get; set; }

        [Display(Name = "Effective Start Date")]
        [DataType(DataType.Date)]
        public DateTime EffectiveStartDate { get; set; }

        [Display(Name = "Effective End/Termination Date")]
        [DataType(DataType.Date)]
        public DateTime EffectiveEndDate { get; set; }

        public virtual RequestType RequestType { get; set; }

        public virtual WorkerType WorkerType{ get; set; }

        public virtual BasePayChange BasePayChange { get; set; }

        public virtual AllowanceChange AllowanceChange { get; set; }

        public virtual TerminationReason TerminationReason { get; set; }

        public virtual SupOrg SupOrg { get; set; }

        public string EmployeeEID { get; set; }

        public string EmployeeName { get; set; }

        public string BudgetNumbers { get; set; }

        public string Note { get; set; }

        public bool Offboarding { get; set; }

        public bool ClosePosition { get; set; }

        public bool LeaveWA { get; set; }
    }
}
