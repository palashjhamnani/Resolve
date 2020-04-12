using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Resolve.Models
{
    public class UserGroup
    {
        public string LocalUserID { get; set; }
        public LocalUser LocalUser { get; set; }
        public string LocalGroupID { get; set; }
        public LocalGroup LocalGroup { get; set; }
    }
}
