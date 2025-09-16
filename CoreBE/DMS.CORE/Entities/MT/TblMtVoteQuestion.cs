using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DMS.CORE.Common;

namespace DMS.CORE.Entities.MT
{
    [Table("T_MT_VOTE_QUESTION")]
    public class TblMtVoteQuestion : BaseEntity
    {
        [Key]
        [Column("ID")]
        public string? Id { get; set; }

        [Column("VOTE_ID")]
        public string? VoteId { get; set; }

        [Column("QUESTION_TEXT")]
        public string? QuestionText { get; set; }

        [Column("QUESTION_ORDER")]
        public int? QuestionOrder { get; set; }

        [Column("IS_REQUIRED")]
        public bool? IsRequired { get; set; }

        [Column("VOTE_TYPE")]
        public string? VoteType { get; set; }

        [Column("ALLOW_MULTIPLE_CHOICE")]
        public bool? AllowMultipleChoice { get; set; }

        [Column("SHORT_ANSWER")]
        public string? ShortAnswer { get; set; }
    }
}
