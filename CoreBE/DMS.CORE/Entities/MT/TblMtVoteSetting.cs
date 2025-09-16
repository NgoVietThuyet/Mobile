using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DMS.CORE.Common;

namespace DMS.CORE.Entities.MT
{
    [Table("T_MT_VOTE_SETTINGS")]
    public class TblMtVoteSetting : BaseEntity
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }

        [Column("VOTE_ID")]
        public int VoteId { get; set; }

        [Column("SETTING_KEY")]
        public string? SettingKey { get; set; }

        [Column("SETTING_VALUE")]
        public string? SettingValue { get; set; }
    }
}
