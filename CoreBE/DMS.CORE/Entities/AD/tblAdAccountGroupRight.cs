using DMS.CORE.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DMS.CORE.Entities.AD
{
    [Table("T_AD_ACCOUNTGROUP_RIGHT")]
    public class TblAdAccountGroupRight : SoftDeleteEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID")]
        public Guid Id { get; set; }

        [Column("GROUP_ID")]
        public Guid GroupId { get; set; }

        [Column("RIGHT_ID")]
        public string RightId { get; set; }

        public virtual TblAdAccountGroup AccountGroup { get; set; }

        public virtual TblAdRight Right { get; set; }
    }
}
