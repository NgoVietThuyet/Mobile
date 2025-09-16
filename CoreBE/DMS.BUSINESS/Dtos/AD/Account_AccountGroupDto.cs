using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using AutoMapper;
using Common;
using DMS.CORE.Entities.AD;

namespace DMS.BUSINESS.Dtos.AD
{
    public class Account_AccountGroupDto : IMapFrom, IDto
    {
        [Key]
        public string UserName { get; set; }

        [Key]
        public Guid GroupId { get; set; }

        public virtual AccountDto Account { get; set; }

        [JsonIgnore]
        public virtual AccountGroupDto AccountGroup { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<TblAdAccount_AccountGroup, Account_AccountGroupDto>().ReverseMap();
        }
    }

    public class TblAccount_AccountGroupCreateGroupDto : IMapFrom, IDto
    {
        public Guid GroupId { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<TblAdAccount_AccountGroup, TblAccount_AccountGroupCreateGroupDto>().ReverseMap();
        }
    }

    public class TblAccount_AccountGroupUpdateGroupDto : IMapFrom, IDto
    {
        public Guid GroupId { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Account_AccountGroupDto, TblAccount_AccountGroupUpdateGroupDto>().ReverseMap();
        }
    }

    public class TblAccount_AccountGroupCreateAccountDto : IMapFrom, IDto
    {
        public string UserName { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<TblAdAccount_AccountGroup, TblAccount_AccountGroupCreateAccountDto>().ReverseMap();
        }
    }

    public class TblAccount_AccountGroupLiteAccountDto : IMapFrom, IDto
    {
        [Key]
        public string UserName { get; set; }

        [Key]
        public Guid GroupId { get; set; }

        public virtual AccountPortableDto Account { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<TblAdAccount_AccountGroup, TblAccount_AccountGroupLiteAccountDto>().ReverseMap();
        }
    }

    public class TblAccount_AccountGroupLiteGroupDto : IMapFrom, IDto
    {
        [Key]
        public Guid GroupId { get; set; }

        public virtual AccountGroupLiteDto AccountGroup { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<TblAdAccount_AccountGroup, TblAccount_AccountGroupLiteGroupDto>().ReverseMap();
        }
    }
}
