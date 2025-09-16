using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DMS.CORE.Common;

namespace DMS.CORE.Entities.MT
{
    [Table("T_MT_VOTES")]
    public class TblMtVotes : BaseEntity
    {
        [Key]
        [Column("ID")]
        public string? Id { get; set; }

        [Column("TITLE")]
        public string Title { get; set; }

        [Column("DESCRIPTION")]
        public string? Description { get; set; }

        [Column("STATUS")]
        public string? Status { get; set; }

        [Column("VOTING_TIME")]
        public decimal? VotingTime { get; set; }

        [Column("START_TIME")]
        public DateTime? StartTime { get; set; }

        [Column("END_TIME")]
        public DateTime? EndTime { get; set; }

        [Column("IS_ANONYMOUS")]
        public bool? IsAnonymous { get; set; }

        [Column("MEETING_ID")]
        public string? MeetingId { get; set; }

        [Column("DURATION")]
        public DateTime? Duration { get; set; }

        [Column("FILE_NAME")]
        public string? FileName { get; set; }

        [Column("FILE_TYPE")]
        public string? FileType { get; set; }

        [Column("FILE_PATH")]
        public string? FilePath { get; set; }
    }
}
