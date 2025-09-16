using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Common;
using DMS.CORE.Entities.CM;

namespace DMS.BUSINESS.Dtos.CM
{
    public class FileDto : IMapFrom, IDto
    {
        [Key]
        public string? Id { get; set; }
        public string? RefrenceFileId { get; set; }
        public string? RefrencePdfFileId { get; set; }
        public string? FileName { get; set; }
        public string? FileType { get; set; }
        public decimal? FileSize { get; set; }
        public string? FilePath { get; set; }
        public bool? IsAllowDelete { get; set; }
        public string? FileNameKhongDau { get; set; }
        public string? PathPdfFile { get; set; }
        public DateTime? CreateDate { get; set; }
        public string? CreateBy { get; set; }
        public bool? isDelete { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<TblCmFile, FileDto>().ReverseMap();
        }
    }

    public class Base64FileDto : IDto
    {
        public string Base64Content { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public long FileSize { get; set; }
        public string? OldFileId { get; set; }

    }
}
