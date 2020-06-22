using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Resolve.Models
{
    public class EmailPreference
    {
        [Required, Key, ForeignKey("LocalUser")]
        public string LocalUserID { get; set; }
        public LocalUser LocalUser { get; set; }       

        public bool CaseCreation { get; set; }
        public bool CaseAssignment { get; set; }
        public bool CommentCreation { get; set; }
        public bool AttachmentCreation { get; set; }
        public bool CaseProcessed { get; set; }

    }
}
