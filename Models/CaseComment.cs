using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Resolve.Models
{
    public class CaseComment
    {
        public int CaseCommentID { get; set; }

        [Required]
        public string Comment { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CommentTimestamp { get; set; }

        public int CaseID { get; set; }
        public Case Case { get; set; }
        public int UserID { get; set; }
        public User User { get; set; }
    }
}
