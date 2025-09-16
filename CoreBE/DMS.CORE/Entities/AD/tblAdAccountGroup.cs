using DMS.CORE.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DMS.CORE.Entities.AD
{
    [Table("T_AD_ACCOUNTGROUP")]
    public class TblAdAccountGroup : SoftDeleteEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID")]
        public Guid Id { get; set; }

        [Column("NAME",TypeName = "NVARCHAR(50)")]
        public string Name { get; set; }

        [Column("NOTES",TypeName = "NVARCHAR(255)")]
        public string? Notes { get; set; }

        [Column("ROLE_CODE",TypeName = "VARCHAR(255)")]
        public string? RoleCode { get; set; }

        public virtual List<TblAdAccount_AccountGroup> Account_AccountGroups { get; set; }

        public virtual List<TblAdAccountGroupRight> ListAccountGroupRight { get; set; }
    }
}
