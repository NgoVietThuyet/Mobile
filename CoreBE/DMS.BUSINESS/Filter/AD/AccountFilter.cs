using Common;

namespace DMS.BUSINESS.Filter.AD
{
    public class AccountFilter : BaseFilter
    {
        public Guid? GroupId { get; set; }

        public string? RoleCode { get; set; }

        public string? AccountType { get; set; }

        public string? OrganizeCode { get; set; }
    }

    public class AccountFilterLite
    {
        public Guid? GroupId { get; set; }

        public string? RoleCode { get; set; }

        public string? KeyWord { get; set; }

        public string[]? ExceptRoles { get; set; }

        public bool? IsActive { get; set; }

        public string? AccountType { get; set; }
    }
}
