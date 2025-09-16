using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using DMS.CORE.Entities.AD;
using AutoMapper;
using DMS.BUSINESS.Common.Enum;
using Common;

namespace DMS.BUSINESS.Dtos.AD
{
    public class AccountGroupDto : BaseAdDto, IMapFrom, IDto
    {
        [Key]
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Notes { get; set; }

        public int TotalAccount => Account_AccountGroups?.Count ?? 0;


        public List<TblAccount_AccountGroupLiteAccountDto> Account_AccountGroups { get; set; }

        public List<AccountGroupRightDto> ListAccountGroupRight { get; set; }

        public RightDto TreeRight { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<TblAdAccountGroup, AccountGroupDto>().ReverseMap();
        }
    }

    public class AccountGroupUpdateDto : BaseAdDto, IMapFrom, IDto
    {
        [Key]
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string? Notes { get; set; }


        public List<AccountGroupRightCreateDto> ListAccountGroupRight { get; set; }

        public List<TblAccount_AccountGroupCreateAccountDto> Account_AccountGroups { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<TblAdAccountGroup, AccountGroupUpdateDto>().ReverseMap();
        }
    }

    public class AccountGroupCreateDto : BaseAdDto, IMapFrom, IDto
    {
        public string Name { get; set; }

        public string Notes { get; set; }


        public void Mapping(Profile profile)
        {
            profile.CreateMap<TblAdAccountGroup, AccountGroupCreateDto>().ReverseMap();
        }
    }

    public class TblAccountGroupExportDto : BaseAdDto
    {
        [Description("STT")]
        public int OrdinalNumber { get; set; }

        [Description("Tên nhóm")]
        public string Name { get; set; }

        [Description("Ghi chú")]
        public string Notes { get; set; }

        [Description("Trạng thái")]
        public string State { get => IsActive == true ? "Đang hoạt động" : "Khóa"; }
    }

    public class AccountGroupLiteDto :BaseAdDto, IMapFrom, IDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Notes { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<TblAdAccountGroup, AccountGroupLiteDto>().ReverseMap();
        }
    }
}
