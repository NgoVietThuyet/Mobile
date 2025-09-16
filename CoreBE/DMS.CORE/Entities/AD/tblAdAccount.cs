using DMS.CORE.Common;
using DMS.CORE.Entities.MD;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DMS.CORE.Entities.AD
{
    [Table("T_AD_ACCOUNT")]
    public class TblAdAccount : SoftDeleteEntity
    {
        [Key]
        [Column("USER_NAME", TypeName = "VARCHAR(50)")]
        public string UserName { get; set; }

        [Column("FULL_NAME", TypeName = "NVARCHAR(255)")]
        public string FullName { get; set; }

        [Column("USER_ID", TypeName = "VARCHAR(100)")]
        public string? UserId { get; set; }

        [Column("PASSWORD", TypeName = "VARCHAR(50)")]
        public string Password { get; set; }

        [Column("PHONE_NUMBER", TypeName = "VARCHAR(10)")]
        public string? PhoneNumber { get; set; }

        [Column("EMAIL", TypeName = "VARCHAR(255)")]
        public string? Email { get; set; }

        [Column("ADDRESS", TypeName = "NVARCHAR(255)")]
        public string? Address { get; set; }

        [Column("ACCOUNT_TYPE", TypeName = "VARCHAR(10)")]
        public string? AccountType { get; set; }

        [Column("ORG_CODE", TypeName = "VARCHAR(50)")]
        public string? OrganizeCode { get; set; }

        [Column("URL_IMAGE", TypeName = "VARCHAR(100)")]
        public string? UrlImage { get; set; }

        [Column("FACE_ID", TypeName = "VARCHAR(100)")]
        public string? FaceId { get; set; }


        public virtual ICollection<TblAdAccount_AccountGroup> Account_AccountGroups { get; set; }

        public virtual ICollection<TblAdAccountRight> AccountRights { get; set; }

        public virtual ICollection<TblAdAccountRefreshToken> RefreshTokens { get; set; }


    }
}
