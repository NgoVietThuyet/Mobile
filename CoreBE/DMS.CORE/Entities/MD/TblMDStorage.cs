using DMS.CORE.Common;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace DMS.CORE.Entities.MD
{
    [Table("T_MD_STORAGE")]
    public class TblMdStorage : BaseEntity
    {
        [Key]
        [Column("ID")]
        public string ID { get; set; }

        [Column("NAME")]
        public string Name { get; set; }

        [Column("CODE")]
        public string Code { get; set; }
        
    }
}
