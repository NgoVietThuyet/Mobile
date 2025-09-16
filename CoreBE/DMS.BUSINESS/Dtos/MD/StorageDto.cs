using AutoMapper;
using Common;
using DMS.CORE.Entities.MD;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DMS.BUSINESS.Dtos.MD
{
    public class StorageDto : BaseMdDto, IMapFrom, IDto
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
      
        public void Mapping(Profile profile)
        {
            profile.CreateMap<TblMdStorage, StorageDto>().ReverseMap();
        }
    }
}
