using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using System.Text.Json.Serialization;
using Common;
using DMS.CORE.Entities.AD;

namespace DMS.BUSINESS.Dtos.AD
{
    public class UserDto : BaseAdDto, IMapFrom, IDto
    {
        [Key]
        [Description("PKID")]
        public string PKID { get; set; }

        [Description("STT")]
        public int OrdinalNumber { get; set; }

        [Description("Tên đầy đủ")]
        public string? FullName { get; set; }

        [Description("SĐT")]
        public string? PhoneNumber { get; set; }

        [Description("Email")]
        public string? Email { get; set; }

        [Description("Địa chỉ")]
        public string? Address { get; set; }

        public string? UrlImage { get; set; }
        public string? FaceId { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<TblUser, UserDto>().ReverseMap();
        }
    }
    public class UserCreateDto : BaseAdDto, IMapFrom, IDto
    {
        [Key]
        public string? PKID { get; set; }
        public string? FullName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string? ImageBase64 { get; set; }
        public string? UrlImage { get; set; }
        public string? FaceId { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<TblUser, UserCreateDto>().ReverseMap();
        }
    }
}
