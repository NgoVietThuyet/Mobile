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
    [Table("T_MT_MEETING_MEMBER")]
    public class TblMtMeetingMember : BaseEntity
    {
        [Key]
        [Column("ID")]
        public string? Id { get; set; }

        [Column("GUEST_NAME")]
        public string? GuestName { get; set; }

        [Column("MEETING_ID")]
        public string? MeetingId { get; set; }

        [Column("USER_NAME")]
        public string? UserName { get; set; }

        [Column("USER_ID")]
        public string? UserId { get; set; }

        [Column("TYPE")]
        public string? Type { get; set; }

    }
}
