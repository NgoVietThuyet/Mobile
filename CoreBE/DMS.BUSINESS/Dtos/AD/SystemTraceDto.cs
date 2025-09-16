using AutoMapper;
using Common;
using DMS.CORE.Entities.AD;
using System.ComponentModel.DataAnnotations;

namespace DMS.BUSINESS.Dtos.AD
{
    public class SystemTraceDto : BaseDto, IMapFrom, IDto
    {
        [Key]
        public string Code { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        public string Address { get; set; }

        public int Interval { get; set; }

        public string? Note { get; set; }

        public bool? Status { get; set; }

        public string? Log { get; set; }

        public DateTime? LastCheckTime { get; set; }

        public DateTime? NextCheckTime { get => LastCheckTime?.AddMinutes(Interval); }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<TblAdSystemTrace, SystemTraceDto>().ReverseMap();
        }
    }

    public class SystemTraceCreateUpdateDto : IMapFrom, IDto
    {
        [Key]
        public string Code { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        public string Address { get; set; }

        public int Interval { get; set; }

        public string? Note { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<TblAdSystemTrace, SystemTraceCreateUpdateDto>().ReverseMap();
        }
    }

    public class SystemTraceResponseDto
    {
        public string Code { get; set; }

        public bool? Status { get; set; }

        public string Message { get; set; }

        public DateTime CheckTime { get; set; }
    }
}
