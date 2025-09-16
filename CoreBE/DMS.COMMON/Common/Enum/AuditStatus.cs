using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.COMMON.Common.Enum
{
    public enum AuditStatus
    {
        [EnumValue("01")]
        [EnumName("Khởi tạo")]
        KHOI_TAO,

        [EnumValue("02")]
        [EnumName("Chờ xác nhận")]
        CHO_XAC_NHAN,

        [EnumValue("03")]
        [EnumName("Đã phê duyệt")]
        DA_PHE_DUYET,

        [EnumValue("04")]
        [EnumName("Từ chối")]
        TU_CHOI       
    }

}
