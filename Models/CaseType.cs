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
        // Create a model in CaseTypeModels folder with this name
        [Display(Name = "Case Type Entity Name")]
        public string CaseTypeTitle { get; set; }
        //public Type Ctype { get; set; }

        [Display(Name = "Description")]
        public string LongDescription { get; set; }

        [Display(Name = "Default Group")]
        public string LocalGroupID { get; set; }
        public LocalGroup LocalGroup { get; set; }

        public ICollection<Case> Cases { get; set; }
    }
}
