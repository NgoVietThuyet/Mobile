using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DMS.CORE.Common;

namespace DMS.CORE.Entities.CM
{
    [Table("T_CM_FILE")]
    public class TblCmFile : SoftDeleteEntity
    {
        [Key]
        [Column("ID")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string? Id { get; set; }

        [Column("REFRENCE_FILE_ID")]
        public string? RefrenceFileId { get; set; }

        [Column("FILE_NAME")]
        public string? FileName { get; set; }

        [Column("FILE_TYPE")]
        public string? FileType { get; set; }

        [Column("FILE_SIZE")]
        public decimal? FileSize { get; set; }

        [Column("PATH")]
        public string? FilePath { get; set; }

        [Column("FILE_NAME_KHONG_DAU")]
        public string? FileNameKhongDau { get; set; }

        [Column("IS_ALLOW_DELETE")]
        public bool? IsAllowDelete { get; set; }
    }
}
