using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DMS.CORE.Common;

namespace DMS.CORE.Entities.MT
{
    [Table("T_MT_MEETING_RESOURCE")]
    public class TblMtMeetingResource : BaseEntity
    {
        [Key]
        [Column("ID")]
        public string Id { get; set; }

        [Column("MEET_ID")]
        public string MeetId { get; set; }

        [Column("USER_NAME")]
        public string? Username { get; set; }

    }
}
