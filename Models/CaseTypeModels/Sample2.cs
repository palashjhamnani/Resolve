using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Resolve.Models
{
    public class Sample2
    {

        [Required, Key, ForeignKey("Case")]
        public int CaseID { get; set; }
        public Case Case { get; set; }

        [Display(Name = "Sample Field"), Required]
        public string SampleDescription { get; set; }

    }
}
