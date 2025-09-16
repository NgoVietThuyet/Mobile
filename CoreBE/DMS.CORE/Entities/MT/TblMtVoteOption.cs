using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DMS.CORE.Common;

namespace DMS.CORE.Entities.MT
{
    [Table("T_MT_VOTE_OPTION")]
    public class TblMtVoteOption : BaseEntity
    {
        [Key]
        [Column("ID")]
        public string? Id { get; set; }

        [Column("QUESTION_ID")]
        public string? QuestionId { get; set; }


        [Column("OPTION_TEXT", TypeName = "NVARCHAR(MAX)")]
        public string? OptionText { get; set; }

        [Column("IS_OTHER_OPTION")]
        public bool? IsOtherOption { get; set; } = false;

        [Column("OTHER_TEXT", TypeName = "NVARCHAR(255)")]
        public string? OtherText { get; set; }
    }
}
