
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
    [Table("T_MT_SEAT_LAYOUT")]
    public class TblMtSeatLayout : BaseEntity
    {
        [Key]
        [Column("ID")]
        public string Id { get; set; }

        [Column("ROOM_ID")]
        public string? RoomId { get; set; }

        [Column("SEAT_NUMBER")]
        public decimal? SeatNumber { get; set; }

        [Column("SEAT_TYPE")]
        public string? SeatType { get; set; }


    }
}
