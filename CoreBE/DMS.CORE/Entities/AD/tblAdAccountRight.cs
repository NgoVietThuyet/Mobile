using DMS.CORE.Common;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace DMS.CORE.Entities.AD
{
    [Table("T_AD_ACCOUNT_RIGHT")]
    public class TblAdAccountRight : BaseEntity
    {
        [Column("USER_NAME")]
        public string UserName { get; set; }

        [Column("RIGHT_ID")]
        public string RightId { get; set; }

        [Column("IS_ADDED")]
        public bool? IsAdded { get; set; }

        [Column("IS_REMOVED")]
        public bool? IsRemoved { get; set; }

        public virtual TblAdAccount Account { get; set; }

        public virtual TblAdRight Right { get; set; }
    }
}
