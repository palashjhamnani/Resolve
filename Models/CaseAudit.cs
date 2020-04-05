using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Resolve.Models
{
    public class CaseAudit
    {
        public int CaseAuditID { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime AuditTimestamp { get; set; }

        [Display(Name = "Audit Log"), Required]
        public string AuditLog { get; set; }

        public int CaseID { get; set; }
        public Case Case { get; set; }

        public string LocalUserID { get; set; }
        public LocalUser LocalUser { get; set; }
    }
}
