using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Resolve.Models
{
    public enum FacRequestType
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
        [Display(Name = "Paid to Unpaid")]
        Unpaid,
        [Display(Name = "Termination")]
        Termination,
        [Display(Name = "Other")]
        Other
    }

    public enum TerminationReason
    {
        [Display(Name = "Death")]
        Death,
        [Display(Name = "Layoff - Funding")]
        LayoffFund,
        [Display(Name = "Layoff - Lack of Work")]
        LayoffWork,
        [Display(Name = "Layoff - Reorganization")]
        LayoffReorg,
        [Display(Name = "Retirement")]
        Retirement,
        [Display(Name = "Separation - Better Job Opportunities")]
        SeparationBetterJob,
        [Display(Name = "Separation - Commute")]
        SeparationCommute,
        [Display(Name = "Separation - Denied Tenure / Promotion")]
        SeparationNoTenure,
        [Display(Name = "Separation - Educational Pursuit")]
        SeparationEducation,
        [Display(Name = "Separation - End of Student Status")]
        SeparationEndStudent,
        [Display(Name = "Separation - Fixed Term Job Ended")]
        SeparationFixedTerm,
        [Display(Name = "Separation - General Resignation")]
        SeparationResignation,
        [Display(Name = "Separation - Job Dissatisfaction")]
        SeparationDissat,
        [Display(Name = "Separation - Medical Disability")]
        SepartionMedDis,

        [Display(Name = "Separation - Not Reappointed/Renewed")]
        SeparationNoRenew,
        [Display(Name = "Separation - Personal Reasons")]
        SeparationPersonal,
        [Display(Name = "Separation - Probationary Rejection")]
        SeparationProbation,
        [Display(Name = "Separation - Relocation")]
        SeparationReloc,
        [Display(Name = "Separation - Resigned-Accepted non-Academic Position")]
        SeparationResAca,
        [Display(Name = "Separation - Resign in Lieu of Dismissal")]
        SepartionDismiss,
        [Display(Name = "Separation - Return to Rehire List")]
        SeparationRehire,
        [Display(Name = "Separation - Separated for Cause")]
        SeparationCause,
        [Display(Name = "Separation - Transfer Within UW Medicine")]
        SeparationTransfer


    }
    public enum FacAllowanceChange
    {
        [Display(Name = "Administrative Suppliment")]
        AdminSupp,
        [Display(Name = "Endowed Supplement")]
        EndowedSupp,
        [Display(Name = "Lump Sum Moving Allowance")]
        LumpSumMoving,
        [Display(Name = "Lump Sum Relocation Payment")]
        LumpSumReloc,
        [Display(Name = "Other")]
        Other
    }
    public class HRServiceFaculty

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
        public DateTime? EffectiveEndDate { get; set; }

        [Display(Name = "Request Type"), Required]
        public FacRequestType FacRequestType { get; set; }

        public virtual SupOrg? SupOrg { get; set; }

        public virtual Department Department { get; set; }

        [Display(Name = "Termination Reason")]
        public virtual TerminationReason? TerminationReason { get; set; }

        [Display(Name = "Allowance Change")]
        public virtual FacAllowanceChange? FacAllowanceChange { get; set; }

        public string EmployeeEID { get; set; }

        [Display(Name = "Employee Name"), Required]
        public string EmployeeName { get; set; }

        [Display(Name = "Job Title")]
        public string JobTitle { get; set; }

        [Display(Name = "Current FTE")]
        public string CurrentFTE { get; set; }

        [Display(Name = "Proposed FTE")]
        public string ProposedFTE { get; set; }

        [Display(Name = "Budget Numbers"), Required]
        public string BudgetNumbers { get; set; }

        public string Note { get; set; }

        [Display(Name = "Offboarding?")]
        public bool Offboarding { get; set; }

        [Display(Name = "Close Position?")]
        public bool ClosePosition { get; set; }

        [Display(Name = "Leave WA?")]
        public bool LeaveWA { get; set; }

        [Display(Name = "Salary/Pay Rate")]
        public string Salary { get; set; }

        [Display(Name = "Amount/Percent/Step Increase")]
        public string Amount { get; set; }
    }
}
