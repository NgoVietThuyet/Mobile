using DMS.CORE.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DMS.CORE.Entities.AD
{
    [Table("T_AD_APP_VERSION")]
    public class TblAdAppVersion : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("VERSION_CODE")]
        public int VersionCode { get; set; }

        [Column("VERSION_NAME",TypeName = "VARCHAR(50)")]
        public string VersionName { get; set; }

        [Column("IS_REQUIRED_UPDATE")]
        public bool IsRequiredUpdate { get; set; }
    }
}
