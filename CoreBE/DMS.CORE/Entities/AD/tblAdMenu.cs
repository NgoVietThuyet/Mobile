using DMS.CORE.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DMS.CORE.Entities.AD
{
    [Table("T_AD_MENU")]
    public class TblAdMenu : SoftDeleteEntity
    {
        [Key]
        [Column("ID",TypeName = "VARCHAR(50)")]
        public string Id { get; set; }

        [Column("NAME",TypeName = "NVARCHAR(255)")]
        public string Name { get; set; }

        [Column("P_ID",TypeName = "VARCHAR(50)")]
        public string? PId { get; set; }

        [Column("ORDER_NUMBER")]
        public int OrderNumber { get; set; }

        [Column("URL",TypeName = "VARCHAR(255)")]
        public string? Url { get; set; }

        [Column("ICON",TypeName = "VARCHAR(255)")]
        public string? Icon { get; set; }

        public List<TblAdMenuRight> RightReferences { get; set; }
    }
}
