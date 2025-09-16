using DMS.CORE.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.CORE.Entities.MD
{
    [Table("T_MD_ACCOUNT_TYPE")]
    public class TblMdAccountType : SoftDeleteEntity
    {
        [Key]
        [Column("ID")]
        public string Id { get; set; }
        [Column("NAME", TypeName = "NVARCHAR(255)")]
        public string Name { get; set; }
    }
}
