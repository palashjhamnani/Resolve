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

        // Case Created by
        [Display(Name = "Case Created By")]
        public int UserID { get; set; }

        [Display(Name = "Case Created By")]
        public User User { get; set; }

        // Case can be on behalf of someone who doesn't yet exist in User table, below is a flag      
        public int OnBehalfOf { get; set; }

        [Display(Name = "Case Status")]
        public string CaseStatus { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CaseCreationTimestamp { get; set; }

        public int CaseTypeID { get; set; }
        public CaseType CaseType { get; set; }

        public ICollection<CaseComment> CaseComments { get; set; }
        public ICollection<CaseAudit> CaseAudits { get; set; }
        public ICollection<Approver> Approvers { get; set; }

    }  

}
