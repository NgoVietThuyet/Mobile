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
    [Table("T_MT_MEETING")]
    public class TblMtMeeting : BaseEntity
    {
        [Key]
        [Column("ID")]
        public string? Id { get; set; }

        [Column("CODE")]
        public string? Code { get; set; }

        [Column("NAME")]
        public string? Name { get; set; }

        [Column("ROOM_ID")]
        public string? RoomId { get; set; }

        [Column("HOST_USERNAME")]
        public string? HostUsername { get; set; }

        [Column("START_DATE")]
        public DateTime? StartDate { get; set; }

        [Column("END_DATE")]
        public DateTime? EndDate { get; set; }

        [Column("ADDRESS")]
        public string? Address { get; set; }

        [Column("MODE")]
        public string? Mode { get; set; }

        [Column("STATUS")]
        public string? Status { get; set; }

        [Column("NOTE")]
        public string? Note { get; set; }

    }
}
