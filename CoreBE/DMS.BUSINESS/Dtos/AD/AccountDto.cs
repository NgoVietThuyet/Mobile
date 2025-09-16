using System.ComponentModel.DataAnnotations;
using DMS.CORE.Entities.AD;
using AutoMapper;
using System.Text.Json.Serialization;
using DMS.CORE.Entities.MD;
using System.ComponentModel;
using Common;

namespace DMS.BUSINESS.Dtos.AD
{
    public class AccountDto : BaseAdDto, IMapFrom, IDto
    {
        [JsonIgnore]
        [Description("STT")]
        public int OrdinalNumber { get; set; }

        [Key]
        [Description("Tên tài khoản")]
        public string? UserName { get; set; }

        [Description("Tên đầy đủ")]
        public string? FullName { get; set; }
        public string? UserId { get; set; }

        [Description("SĐT")]
        public string? PhoneNumber { get; set; }

        [Description("Email")]
        public string? Email { get; set; }

        [Description("Địa chỉ")]
        public string? Address { get; set; }

        [Description("Loại")]
        public string? AccountType { get; set; }
        public string? OrganizeCode { get; set; }
        public string? UrlImage { get; set; }
        public string? FaceId { get; set; }

        //public string? OrgCode { get; set; }



        [JsonIgnore]
        public virtual List<Account_AccountGroupDto> Account_AccountGroups { get; set; }

        public virtual List<AccountGroupDto> AccountGroups { get => Account_AccountGroups.Select(x => x.AccountGroup).ToList(); }

        public virtual List<AccountRightLoginDto> AccountRights { get; set; }



        public void Mapping(Profile profile)
        {
            profile.CreateMap<TblAdAccount, AccountDto>().ReverseMap();
        }
    }

    public class AccountLoginDto : BaseAdDto, IMapFrom, IDto
    {
        [Key]
        public string? UserName { get; set; }

        public string? FullName { get; set; }

        public string? PhoneNumber { get; set; }

        public string? Email { get; set; }
        public string? UserId { get; set; }

        public string? Address { get; set; }

        public string? AccountType { get; set; }

        public string? OrganizeCode { get; set; }
        public string? UrlImage { get; set; }

        public string? FaceId { get; set; }

        [JsonIgnore]
        public virtual List<Account_AccountGroupDto> Account_AccountGroups { get; set; }

        public virtual List<AccountGroupDto> AccountGroups { get => Account_AccountGroups.Select(x => x.AccountGroup).ToList(); }

        public virtual List<AccountRightLoginDto> AccountRights { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<TblAdAccount, AccountLoginDto>().ReverseMap();
        }
    }

    public class AccountCreateDto : BaseAdDto, IMapFrom, IDto
    {
        [Key]
        public string? UserName { get; set; }
        public string? FullName { get; set; }
        public string? UserId { get; set; }
        public string? Password { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string? AccountType { get; set; }
        public string? OrganizeCode { get; set; }
        public string? ImageBase64 { get; set; }
        public string? UrlImage { get; set; }
        public string? FaceId { get; set; }
        //public string? OrgCode { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<TblAdAccount, AccountCreateDto>().ReverseMap();
        }
    }

    public class AccountLiteDto : BaseAdDto, IMapFrom, IDto
    {
        [Key]
        public string? UserName { get; set; }

        public string? FullName { get; set; }

        public string? PhoneNumber { get; set; }

        public string? Email { get; set; }

        public string? Address { get; set; }

        public string? AccountType { get; set; }

        public string? OrganizeCode { get; set; }
        public string? UrlImage { get; set; }
        public string? FaceId { get; set; }

        public virtual List<Account_AccountGroupDto> Account_AccountGroups { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<TblAdAccount, AccountLiteDto>().ReverseMap();
            profile.CreateMap<AccountDto, AccountLiteDto>().ReverseMap();
        }
    }

    public class AccountPortableDto : BaseAdDto, IMapFrom, IDto
    {
        [Key]
        public string? UserName { get; set; }

        public string? FullName { get; set; }

        public string? PhoneNumber { get; set; }

        public string? Email { get; set; }

        public string? Address { get; set; }
        public string? OrganizeCode { get; set; }
        public string? UrlImage { get; set; }
        public string? FaceId { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<TblAdAccount, AccountPortableDto>().ReverseMap();
            profile.CreateMap<AccountDto, AccountPortableDto>().ReverseMap();
        }
    }

    public class AccountUpdateDto : BaseAdDto, IMapFrom, IDto
    {
        [Key]
        public string? UserName { get; set; }

        public string? FullName { get; set; }

        public string? PhoneNumber { get; set; }

        public string? Email { get; set; }

        public string? Address { get; set; }

        public string? AccountType { get; set; }

        public string? OrganizeCode { get; set; }
        public string? ImageBase64 { get; set; }
        public string? UrlImage { get; set; }
        public string? FaceId { get; set; }
        public virtual List<TblAccount_AccountGroupUpdateGroupDto> Account_AccountGroups { get; set; }

        public virtual List<TblAccountRightUpdateDto> AccountRights { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<AccountUpdateDto, AccountDto>().ReverseMap();
        }
    }

    public class AccountTreeRightDto : BaseAdDto, IMapFrom, IDto
    {
        [Key]
        public string? UserName { get; set; }

        public string? FullName { get; set; }

        public string? PhoneNumber { get; set; }

        public string? Email { get; set; }

        public string? Address { get; set; }

        public string? AccountType { get; set; }

        public string? OrganizeCode { get; set; }

        public string? UrlImage { get; set; }
        public string? FaceId { get; set; }
        public virtual List<TblAccount_AccountGroupLiteGroupDto> Account_AccountGroups { get; set; }

        public RightDto TreeRight { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<TblAdAccount, AccountTreeRightDto>().ReverseMap();
        }
    }
}
