using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Resolve.Models
{
    public class CaseTypeGroup
    {
        public int CaseTypeID { get; set; }
        public CaseType CaseType { get; set; }
        // Case to be approved by
        public string LocalGroupID { get; set; }
        public LocalGroup LocalGroup { get; set; }
        // Order is required only if CaseType.Hierarchical_Approval == true
        public int? Order { get; set; }
    }

}
