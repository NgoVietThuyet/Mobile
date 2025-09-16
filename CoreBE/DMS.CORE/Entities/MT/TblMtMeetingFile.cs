using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DMS.CORE.Common;
using Microsoft.AspNetCore.Http;

namespace DMS.CORE.Entities.MT
{
    [Table("T_MT_MEETING_FILE")]
    public class TblMtMeetingFile : BaseEntity
    {
        [Key]
        [Column("ID")]
        public string? Id { get; set; }

        [Column("MEETING_ID")]
        public string? MeetingId { get; set; }

        [Column("USER_ID")]
        public string? UserId { get; set; }

        [Column("USER_NAME")]
        public string? UserName { get; set; }

        [Column("NAME")]
        public string? Name { get; set; }

        [Column("IS_SHARED_ALL")]
        public bool? IsSharedAll { get; set; }

        [Column("FILE_NAME")]
        public string? FileName { get; set; }

        [Column("FILE_TYPE")]
        public string? FileType { get; set; }

        [Column("FILE_PATH")]
        public string? FilePath { get; set; }
    }
}
