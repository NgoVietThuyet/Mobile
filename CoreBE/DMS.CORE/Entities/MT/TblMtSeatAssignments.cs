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
    [Table("T_MT_SEAT_ASSIGNMENTS")]
    public class TblMtSeatAssignments : BaseEntity
    {
        [Key]
        [Column("ID")]
        public string? Id { get; set; }

        [Column("MEETING_ID")]
        public string? MeetingId { get; set; }

        [Column("USER_ID")]
        public string UserId { get; set; }

        [Column("ROOM_ID")]
        public string? RoomId { get; set; }

        [Column("SEAT_ID")]
        public string? SeatId { get; set; }

        [Column("NOTE")]
        public string? Note { get; set; }

    }
}
