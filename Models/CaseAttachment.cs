using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Resolve.Models
{
    public class CaseAttachment
    {
        public int CaseAttachmentID { get; set; }
        // Attachment created for
        public int CaseID { get; set; }
        public Case Case { get; set; }

        // Attachment created by
        [Display(Name = "Attachment By")]
        public string LocalUserID { get; set; }
        public LocalUser LocalUser { get; set; }
        public string FilePath { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime AttachmentTimestamp { get; set; }
    }
}
