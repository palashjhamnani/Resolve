using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Resolve.Models
{
    public class CaseType
    {
        public int CaseTypeID { get; set; }

        // No Blank Spaces. Create a model in ~/Models/CaseTypeModels/ folder with this name        
        [Display(Name = "Case Type Entity Name")]       
        public string CaseTypeTitle { get; set; }

        [Display(Name = "Description")]
        public string? LongDescription { get; set; }
        public string LocalGroupID { get; set; }
        public LocalGroup LocalGroup { get; set; }
        public int? GroupNumber { get; set; }
        public ICollection<Case> Cases { get; set; }
        public ICollection<CaseTypeGroup> CaseTypeGroups { get; set; }
    }
}
