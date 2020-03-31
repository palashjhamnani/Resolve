using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Resolve.Models
{
    public class OnBehalf
    {
        public int CaseID { get; set; }
        public Case Case { get; set; }

        // Case to be approved by
        [Display(Name = "On behalf of")]
        public int UserID { get; set; }
        public User User { get; set; }
    }
}
