using DMS.CORE.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DMS.CORE.Entities.AD
{
    [Table("T_AD_MESSAGE")]
    public class TblAdMessage : SoftDeleteEntity
    {
        [Key]
        [Column("CODE",TypeName = "VARCHAR(10)")]
        public string Code { get; set; }

        [Column("LANG",TypeName = "VARCHAR(10)")]
        public string Lang { get; set; }

        [Column("VALUE",TypeName = "NVARCHAR(255)")]
        public string Value { get; set; }
    }
}
