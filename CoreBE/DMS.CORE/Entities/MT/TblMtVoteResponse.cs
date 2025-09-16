using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DMS.CORE.Common;

namespace DMS.CORE.Entities.MT
{
    [Table("T_MT_VOTE_RESPONSES")]
    public class TblMtVoteResponse
    {
        [Key]
        [Column("ID")]
        public string Id { get; set; }

        [Column("USER_ID")]
        public string UserId { get; set; }

        [Column("QUESTION_ID")]
        public string QuestionId { get; set; }

        [Column("OPTION_ID")]
        public string? OptionId { get; set; }

        [Column("RESPONSE_TEXT")]
        public string? ResponseText { get; set; }

        [Column("CREATE_DATE")]
        public DateTime? CreateDate { get; set; }

        // [Column("RATING_VALUE")]
        // public int? RatingValue { get; set; }
    }
}
