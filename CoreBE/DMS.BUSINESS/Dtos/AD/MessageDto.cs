using System.ComponentModel.DataAnnotations;
using DMS.CORE.Entities.AD;
using AutoMapper;
using Common;

namespace DMS.BUSINESS.Dtos.AD
{
    public class MessageDto : IMapFrom, IDto
    {

        [Key]
   
        public string Code { get; set; }

        public string Lang { get; set; }

        public string Value { get; set; }
      
        public void Mapping(Profile profile)
        {
            profile.CreateMap<TblAdMessage, MessageDto>().ReverseMap();
        }
    }
}
