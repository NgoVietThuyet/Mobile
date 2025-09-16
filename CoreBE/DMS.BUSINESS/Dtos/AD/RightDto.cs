using System.ComponentModel.DataAnnotations;
using DMS.CORE.Entities.AD;
using AutoMapper;
using Common;

namespace DMS.BUSINESS.Dtos.AD
{
    public class RightDto : BaseMdDto, IMapFrom, IDto
    {
        [Key]
        public string Id { get; set; }

        public string Name { get; set; }

        public string? PId { get; set; }

        public int? OrderNumber { get; set; }

        public bool IsChecked { get; set; }

        public string? Title { get; set; }

        public string? Key { get; set; }

        public bool? Expanded { get; set; } = true;

        public bool? IsLeaf { get; set; } = false;

        public List<RightDto>? Children { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<TblAdRight, RightDto>().ReverseMap();
        }
    }

    public class RightViewModel
    {
        public Guid GroupId { get; set; }

        public List<RightDto> ListRight { get; set; }

        public RightViewModel()
        {
            ListRight = [];
        }
    }
}
