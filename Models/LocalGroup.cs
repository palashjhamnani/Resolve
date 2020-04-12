using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Resolve.Models
{
    public class LocalGroup
    {
        // This will be Azure AD ID of the group object from Azure Portal
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None), Required]
        public string LocalGroupID { get; set; }    
        [Required]
        public string GroupName { get; set; }

        [Display(Name = "Description")]
        public string GroupDescription { get; set; }

        [Display(Name = "Default Approver")]
        public string LocalUserID { get; set; }
        public LocalUser LocalUser { get; set; }
        public ICollection<GroupAssignment> GroupCases { get; set; }

    }
}
