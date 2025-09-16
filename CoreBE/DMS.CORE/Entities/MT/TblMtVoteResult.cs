using DMS.CORE.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.CORE.Entities.MT
{
    [Table("T_MT_VOTE_RESULT")]
    public class TblMtVoteResult : BaseEntity
    {
        [Key]
        [Column("ID")]
        public string? Id { get; set; }

        [Column("VOTE_ID")]
        public string? VoteId { get; set; }

        [Column("USER_ID", TypeName = "NVARCHAR(MAX)")]
        public string? UserId { get; set; }

        [Column("MEETING_ID", TypeName = "NVARCHAR(MAX)")]
        public string MeetingId { get; set; }

        [Column("SEGGEST")]
        public string? Seggest { get; set; }

        [Column("RESULT", TypeName = "NVARCHAR(1)")]
        public string? Result { get; set; }
    }
}
