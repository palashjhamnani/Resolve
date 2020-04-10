using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Resolve.Models
{
    public class GroupAssignment
    {
        public int CaseID { get; set; }
        public Case Case { get; set; }       
        public string LocalGroupID { get; set; }
        public LocalGroup LocalGroup { get; set; }
    }
}
