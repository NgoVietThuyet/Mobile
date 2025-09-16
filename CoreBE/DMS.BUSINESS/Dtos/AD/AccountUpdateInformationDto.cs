using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Common;
using DMS.CORE.Entities.AD;

namespace DMS.BUSINESS.Dtos.AD
{
    public class AccountUpdateInformationDto : IMapFrom, IDto
    {
        [Key]
        public string UserName { get; set; }

        public string FullName { get; set; }

        public string? PhoneNumber { get; set; }

        public string? Email { get; set; }

        public string? Address { get; set; }

        public string? AccountType { get; set; }

        public int? PartnerId { get; set; }

        public int? DriverId { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<AccountUpdateInformationDto, TblAdAccount>()
                    .ForMember(x => x.Account_AccountGroups, y => y.Ignore())
                    .ForMember(x => x.AccountRights, y => y.Ignore())
                    .ForMember(x => x.RefreshTokens, y => y.Ignore())
                    .ReverseMap();
        }
    }
}
