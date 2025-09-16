using System.ComponentModel.DataAnnotations;
using DMS.CORE.Entities.AD;
using AutoMapper;
using Common;
using System.ComponentModel;

namespace DMS.BUSINESS.Dtos.AD
{
    public class ConfigTemplateDto : BaseMdDto, IMapFrom, IDto
    {

        [Key]
        [Description("code")]
        public string? Code { get; set; }

        [Description("Tên")]
        public string Name { get; set; }

        [Description("Source code")]
        public string? HtmlSource { get; set; }

        [Description("Tiêu đề")]
        public string? Title { get; set; }

        [Description("Kiểu tin nhắn")]
        public string? Type { get; set; }

        [Description("Trạng thái")]
        public string State { get => this.IsActive == true ? "Đang hoạt động" : "Khóa"; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<TblAdConfigTemplate, ConfigTemplateDto>().ReverseMap();
        }
    }
}
