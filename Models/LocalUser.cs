using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Resolve.Models
{
    public class LocalUser
    {
        // LocalUserID will be populated as UW NetID at the first authentication
        // Assumption: this data about the user will never change
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string LocalUserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Required]
        public string EmailID { get; set; }
        public ICollection<Case> Cases { get; set; }
        public ICollection<Approver> CasesforApproval { get; set; }
        public ICollection<UserGroup> UserGroups { get; set; }
        public ICollection<OnBehalf> OnBehalves { get; set; }
        public EmailPreference EmailPreference { get; set; }
    }
}
