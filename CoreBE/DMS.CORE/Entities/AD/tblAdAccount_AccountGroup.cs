using DMS.CORE.Common;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DMS.CORE.Entities.AD
{
    [Table("T_AD_ACCOUNT_ACCOUNTGROUP")]
    public class TblAdAccount_AccountGroup : BaseEntity
    {
        [Key]
        [Column("USER_NAME")]
        public string UserName { get; set; }

        [Key]
        [Column("GROUP_ID")]
        public Guid GroupId { get; set; }

        
        public virtual TblAdAccount Account { get; set; }

        public virtual TblAdAccountGroup AccountGroup { get; set; }
    }
}
