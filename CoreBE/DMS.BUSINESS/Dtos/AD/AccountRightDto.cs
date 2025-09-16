using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Common;
using DMS.CORE.Entities.AD;

namespace DMS.BUSINESS.Dtos.AD
{
    public class AccountRightDto : IMapFrom, IDto
    {
        public string UserName { get; set; }

        public string RightId { get; set; }

        public bool? IsAdded { get; set; }

        public bool? IsRemoved { get; set; }

        public virtual AccountDto Account { get; set; }

        public virtual RightDto Right { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<TblAdAccountRight, AccountRightDto>().ReverseMap();
        }
    }

    public class AccountRightLoginDto : IMapFrom, IDto
    {
        public string UserName { get; set; }

        public string RightId { get; set; }

        public bool? IsAdded { get; set; }

        public bool? IsRemoved { get; set; }

        public virtual RightDto Right { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<TblAdAccountRight, AccountRightLoginDto>().ReverseMap();
        }
    }

    public class TblAccountRightCreateDto : IMapFrom, IDto
    {
        [Key]
        public string RightId { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<TblAdAccountRight, TblAccountRightCreateDto>().ReverseMap();
        }
    }

    public class TblAccountRightUpdateDto : IMapFrom, IDto
    {
        [Key]
        public string RightId { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<AccountRightLoginDto, TblAccountRightUpdateDto>().ReverseMap();
        }
    }
}
