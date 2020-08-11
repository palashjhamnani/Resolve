using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Resolve.Models
{
    // This class should have been called User Preferences
    public class EmailPreference
    {
        [Required, Key, ForeignKey("LocalUser")]
        public string LocalUserID { get; set; }
        public LocalUser LocalUser { get; set; }       

        public bool CaseCreation { get; set; }
        public bool GroupAssignment { get; set; }
        public bool CaseAssignment { get; set; }
        public bool CommentCreation { get; set; }
        public bool AttachmentCreation { get; set; }
        public bool CaseProcessed { get; set; }
        // Preferences for home page styling
        public bool CasesCreatedByUser { get; set; }
        public bool CasesAssignedToUser { get; set; }
        public bool CasesAssignedToUsersGroups { get; set; }

    }
}
