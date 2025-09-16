using AutoMapper;
using Common;
using DMS.CORE.Entities.AD;
using System.ComponentModel.DataAnnotations;

namespace DMS.BUSINESS.Dtos.AD
{
    public class ActionLogDto : BaseAdDto, IMapFrom, IDto
    {
        [Key]
        public int Id { get; set; }

        public string? UserName { get; set; }

        public string ActionUrl { get; set; }

        public string RequestData { get; set; }

        public DateTime RequestTime { get; set; }

        public string ResponseData { get; set; }

        public DateTime ResponseTime { get; set; }

        public int StatusCode { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<TblActionLog, ActionLogDto>().ReverseMap();
        }
    }
}
