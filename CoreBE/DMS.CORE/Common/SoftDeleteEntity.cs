using System.ComponentModel.DataAnnotations.Schema;

namespace DMS.CORE.Common
{
    public class SoftDeleteEntity : BaseEntity, ISoftDeleteEntity
    {
        [Column("IS_DELETED")]
        public bool? IsDeleted { get; set; }

        [Column("DELETE_DATE")]
        public DateTime? DeleteDate { get; set; }

        [Column("DELETE_BY", TypeName = "VARCHAR2(50)")]
        public string? DeleteBy { get; set; }
    }
}
