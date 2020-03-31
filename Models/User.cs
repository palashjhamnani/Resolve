using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Resolve.Models
{
    public class User
    {
        // UserID will be populated as UW NetID at the first authentication 
        public int UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailID { get; set; }
        public ICollection<Case> Cases { get; set; }
    }
}
