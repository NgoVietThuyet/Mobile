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
    [Table("T_MT_MEETING_MEMBER_FILE")]
    public class TblMtMeetingMemberFile : BaseEntity
    {
        [Key]
        [Column("ID")]
        public string? Id { get; set; }

        [Column("USER_NAME")]
        public string? UserName { get; set; }

        [Column("USER_ID")]
        public string? UserId { get; set; }

        [Column("FILE_ID")]
        public string? FileId { get; set; }

        [Column("MEETING_ID")]
        public string? MeetingId { get; set; }

    }
}
