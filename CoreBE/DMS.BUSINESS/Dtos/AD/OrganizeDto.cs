using AutoMapper;
using Common;
using DMS.CORE.Entities.AD;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.BUSINESS.Dtos.AD
{
    public class OrganizeDto :  BaseMdDto,IMapFrom, IDto
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }   
        public string PId { get; set; }
        public int?  OrderNumber { get; set; }

        public bool? IsChecked { get; set; }
        public bool? Checked { get; set; }

        public string? Title { get; set; }

        public string? Key { get; set; }
        public bool Expanded { get; set; } = true;

        public bool? IsPending { get; set; }

        //  public bool? Expanded { get; set; } = true;

        //  public bool? IsLeaf { get; set; } = false;
        public List<OrganizeDto>? Children { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<tblAdOrganize, OrganizeDto>().ReverseMap();
        }
    }
    public class OrganizeViewModel
    {
        public Guid GroupId { get; set; }

        public List<OrganizeDto> ListOrganize { get; set; }

        public OrganizeViewModel()
        {
            ListOrganize = new List<OrganizeDto>();
        }
    }
   
}
