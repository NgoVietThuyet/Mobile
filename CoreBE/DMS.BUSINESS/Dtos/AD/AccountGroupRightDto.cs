using AutoMapper;
using Common;
using DMS.CORE.Entities.AD;

namespace DMS.BUSINESS.Dtos.AD
{
    public class AccountGroupRightDto : IMapFrom, IDto
    {
        public Guid Id { get; set; }

        public Guid GroupId { get; set; }

        public string RightId { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<TblAdAccountGroupRight, AccountGroupRightDto>().ReverseMap();
        }
    }

    public class AccountGroupRightCreateDto : IMapFrom, IDto
    {
        public string RightId { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<TblAdAccountGroupRight, AccountGroupRightCreateDto>().ReverseMap();
        }
    }
}
