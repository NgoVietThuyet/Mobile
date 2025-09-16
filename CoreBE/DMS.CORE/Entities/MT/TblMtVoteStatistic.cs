using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DMS.CORE.Common;

namespace DMS.CORE.Entities.MT
{
    [Table("T_MT_VOTE_STATISTICS")]
    public class TblMtVoteStatistic
    {
        [Key]
        [Column("ID")]
        public string Id { get; set; }

        [Column("VOTE_ID")]
        public string VoteId { get; set; }

        [Column("TOTAL_PARTICIPANTS")]
        public int? TotalParticipants { get; set; }

        [Column("TOTAL_RESPONSES")]
        public int? TotalResponses { get; set; }

        [Column("COMPLETION_RATE")]
        public decimal? CompletionRate { get; set; }

        [Column("LAST_UPDATED")]
        public DateTime? LastUpdated { get; set; }
    }
}
