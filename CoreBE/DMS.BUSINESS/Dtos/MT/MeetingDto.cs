using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Common;
using DMS.CORE.Entities.MD;
using DMS.CORE.Entities.MT;

namespace DMS.BUSINESS.Dtos.MT
{
    public class MeetingDto : BaseMdDto, IMapFrom, IDto
    {
        [Key]
        public string Id { get; set; }
        public string? Code { get; set; }
        public string Name { get; set; }
        public string? RoomId { get; set; }
        public string? HostUsername { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? StartDate { get; set; }
        public string? Address { get; set; }
        public string? Mode { get; set; }
        public string? Status { get; set; }
        public string? Note { get; set; }
        //public IList<TblMtMeetingMember> LstUserMember { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<TblMtMeeting, MeetingDto>().ReverseMap();
        }

    }
}
