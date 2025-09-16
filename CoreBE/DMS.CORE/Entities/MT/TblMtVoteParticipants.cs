using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DMS.CORE.Common;

namespace DMS.CORE.Entities.MT
{
    [Table("T_MT_VOTE_PARTICIPANTS")]
    public class TblMtVoteParticipants
    {
        [Key]
        [Column("ID")]
        public string Id { get; set; }

        [Column("VOTE_ID")]
        public string VoteId { get; set; }

        [Column("USER_ID")]
        public string? UserId { get; set; }

        [Column("STATUS")]
        public string? Status { get; set; }

        // [Column("PARTICIPANT_NAME")]
        // public string? ParticipantName { get; set; }

        // [Column("PARTICIPANT_EMAIL")]
        // public string? ParticipantEmail { get; set; }

        // [Column("IP_ADDRESS")]
        // public string? IpAddress { get; set; }

        // [Column("USER_AGENT")]
        // public string? UserAgent { get; set; }

        [Column("SUBMITTED_AT")]
        public DateTime? SubmittedAt { get; set; }
    }
}
