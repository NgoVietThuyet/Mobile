using System.ComponentModel.DataAnnotations.Schema;

namespace DMS.CORE.Common
{
    public class BaseEntity : IBaseEntity
    {
        [Column("IS_ACTIVE")]
        public bool? IsActive { get; set; }

        [Column("CREATE_BY",TypeName = "VARCHAR(50)")]
        public string? CreateBy { get; set; }

        [Column("UPDATE_BY", TypeName = "VARCHAR(50)")]
        public string? UpdateBy { get; set; }

        [Column("CREATE_DATE")]
        public DateTime? CreateDate { get; set; }

        [Column("UPDATE_DATE")]
        public DateTime? UpdateDate { get; set; }
    }
}
