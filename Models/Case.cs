using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Resolve.Models
{
    public class Case
    {
        public int CaseID { get; set; }
        public string CaseCID { get; set; }

        // Case Created by
        [Display(Name = "Case Created By")]
        public string LocalUserID { get; set; }
        public LocalUser LocalUser { get; set; }

        // Case can be on behalf of someone who doesn't yet exist in User table, below is a flag      
        public bool OnBehalfOf { get; set; }

        [Display(Name = "Case Status")]
        public string CaseStatus { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CaseCreationTimestamp { get; set; }

        public int CaseTypeID { get; set; }
        public CaseType CaseType { get; set; }

        public int? Processed { get; set; }
        // Below are the properties of a Case made accessible

        public ICollection<CaseComment> CaseComments { get; set; }
        public ICollection<OnBehalf> OnBehalves { get; set; }
        public ICollection<CaseAudit> CaseAudits { get; set; }
        public ICollection<Approver> Approvers { get; set; }
        public ICollection<GroupAssignment> GroupAssignments { get; set; }
        public ICollection<CaseAttachment> CaseAttachments { get; set; }

        // Add a line for every new Case Type added to the application

        public ICollection<SampleCaseType> SampleCaseType { get; set; }
        public ICollection<Sample2> Sample2 { get; set; }
        public ICollection<SAR4> SAR4 { get; set; }
        public ICollection<HRServiceGradStudent> HRServiceGradStudent { get; set; }
        public ICollection<HRServiceStaff> HRServiceStaff { get; set; }
        public ICollection<HRServiceFaculty> HRServiceFaculty { get; set; }

    }  

}
