using AutoMapper;
using Common;
using DMS.CORE.Entities.AD;

namespace DMS.BUSINESS.Dtos.Common
{
    public class UpdaterDto : IMapFrom, IDto
    {
        public string FullName { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<TblAdAccount, UpdaterDto>().ReverseMap();
        }
    }
}
