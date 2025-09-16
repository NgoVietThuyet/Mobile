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
    [Table("T_MT_VOTE_REPORT")]
    public class TblMtVoteReport : BaseEntity
    {
        [Key]
        [Column("ID")]
        public string? Id { get; set; }

        [Column("MEETING_ID")]
        public string? MeetingId { get; set; }

        [Column("VOTE_ID")]
        public string? VoteId { get; set; }

        [Column("K")]
        public decimal? K { get; set; }

        [Column("Y")]
        public decimal? Y { get; set; }

        [Column("N")]
        public decimal? N { get; set; }

    }
}
