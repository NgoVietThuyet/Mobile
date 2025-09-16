using System.ComponentModel.DataAnnotations;
using DMS.CORE.Entities.AD;
using AutoMapper;
using Common;

namespace DMS.BUSINESS.Dtos.AD
{
    public class MenuDto : BaseMdDto,IMapFrom, IDto
    {
        [Key]
        public string Id { get; set; }

        public string Name { get; set; }

        public string? PId { get; set; }

        public int? OrderNumber { get; set; }

        public string? Url { get; set; }

        public string? Icon { get; set; }

        public string? Title { get; set; }

        public string? Key { get; set; }

        public bool? Expanded { get; set; } = true;

        public bool? IsLeaf { get; set; } = false;

        public List<MenuRightDto>? RightReferences { get; set; }

        public List<MenuDto>? Children { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<TblAdMenu, MenuDto>().ReverseMap();
        }
    }

    public class MenuUpdateDto : BaseMdDto, IMapFrom, IDto
    {
        [Key]
        public string Id { get; set; }

        public string Name { get; set; }

        public string? PId { get; set; }

        public int? OrderNumber { get; set; }

        public string? Url { get; set; }

        public string? Icon { get; set; }

        public string? Title { get; set; }

        public string? Key { get; set; }

        public bool? Expanded { get; set; } = true;

        public bool? IsLeaf { get; set; } = false;

        public List<MenuRightUpdateMenuDto>? RightReferences { get; set; }

        public List<MenuDto>? Children { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<TblAdMenu, MenuUpdateDto>().ReverseMap();
        }
    }

    public class MenuDetailDto : BaseMdDto, IMapFrom, IDto
    {
        [Key]
        public string Id { get; set; }

        public string Name { get; set; }

        public string? PId { get; set; }

        public int? OrderNumber { get; set; }

        public string? Url { get; set; }

        public string? Icon { get; set; }

        public string? Title { get; set; }

        public string? Key { get; set; }

        public bool? Expanded { get; set; } = true;

        public bool? IsLeaf { get; set; } = false;

        public RightDto TreeRight { get; set; }

        public List<MenuDto>? Children { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<TblAdMenu, MenuDetailDto>().ReverseMap();
        }
    }
}
