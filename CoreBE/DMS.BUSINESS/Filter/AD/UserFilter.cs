using Common;

namespace DMS.BUSINESS.Filter.AD
{
    public class UserFilter : BaseFilter
    {

    
    }

    public class UserFilterLite
    {
        public string? KeyWord { get; set; }

        public string[]? ExceptRoles { get; set; }

        public bool? IsActive { get; set; }

    }
}
