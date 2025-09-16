using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DMS.CORE.Common;

namespace DMS.CORE.Entities.AD
{
    [Table("T_AD_USER")]

    public class TblUser : BaseEntity
    {
        [Key]
        [Column("PKID", TypeName = "VARCHAR(50)")]
        public string PKID { get; set; }

        [Column("FULL_NAME", TypeName = "NVARCHAR(255)")]
        public string FullName { get; set; }

        [Column("PHONE_NUMBER", TypeName = "VARCHAR(10)")]
        public string? PhoneNumber { get; set; }

        [Column("EMAIL", TypeName = "VARCHAR(255)")]
        public string? Email { get; set; }

        [Column("ADDRESS", TypeName = "NVARCHAR(255)")]
        public string? Address { get; set; }

        [Column("URL_IMAGE", TypeName = "VARCHAR(100)")]
        public string? UrlImage { get; set; }

        [Column("FACE_ID", TypeName = "VARCHAR(100)")]
        public string? FaceId { get; set; }

    }
}
