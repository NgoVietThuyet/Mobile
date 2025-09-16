using System.ComponentModel.DataAnnotations.Schema;

namespace DMS.CORE.Entities.AD
{
    [Table("T_AD_MENU_RIGHT")]
    public class TblAdMenuRight
    {
        public string MenuId { get; set; }

        [ForeignKey("MenuId")]
        public virtual TblAdMenu Menu { get; set; }

        public string RightId { get; set; }

        [ForeignKey("RightId")]
        public virtual TblAdRight Right { get; set; }
    }
}
