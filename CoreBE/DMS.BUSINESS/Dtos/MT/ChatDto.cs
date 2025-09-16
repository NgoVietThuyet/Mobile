using System;
using AutoMapper;
using Common;
using DMS.CORE.Entities.MT;

namespace DMS.BUSINESS.Dtos.MT
{
    public class ChatDto : BaseMdDto, IMapFrom, IDto
    {
        public string Id { get; set; }
        public string MeetingId { get; set; }
        public string SenderId { get; set; }
        public string? SenderUsername { get; set; }
        public string? SenderName { get; set; }
        /// <summary>
        /// P = Public, D = Direct
        /// </summary>
        public string MessageType { get; set; }
        public string? ReceiverId { get; set; }
        public string? ReceiverUsername { get; set; }
        public string? ReceiverName { get; set; }
        public string Content { get; set; }
        /// <summary>
        /// TEXT, IMAGE, FILE, EMOJI
        /// </summary>
        public string ContentType { get; set; } = "TEXT";
        public string? FilePath { get; set; }
        public string? FileName { get; set; }
        public long? FileSize { get; set; }
        public string? ReplyToMessageId { get; set; }
        public bool IsEdited { get; set; } = false;
        public bool IsDeleted { get; set; } = false;
        public DateTime SentTime { get; set; } = DateTime.Now;

        public void Mapping(Profile profile)
        {
            profile.CreateMap<TblMtChat, ChatDto>().ReverseMap();
        }
    }

    public class ChatCreateDto : IMapFrom, IDto
    {
        public string MeetingId { get; set; }
        public string MessageType { get; set; }
        public string? ReceiverId { get; set; }
        public string? SenderId { get; set; }
        public string Content { get; set; }
        public string ContentType { get; set; } = "TEXT";
        public string? FilePath { get; set; }
        public string? FileName { get; set; }
        public long? FileSize { get; set; }
        public string? ReplyToMessageId { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<ChatCreateDto, TblMtChat>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.SenderId, opt => opt.Ignore()) // Set in service
                .ForMember(dest => dest.SenderUsername, opt => opt.Ignore()) // Set in service
                .ForMember(dest => dest.SenderName, opt => opt.Ignore()) // Set in service
                .ForMember(dest => dest.ReceiverUsername, opt => opt.Ignore()) // Set in service
                .ForMember(dest => dest.ReceiverName, opt => opt.Ignore()) // Set in service
                .ForMember(dest => dest.IsEdited, opt => opt.MapFrom(src => false))
                .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => false))
                .ForMember(dest => dest.SentTime, opt => opt.MapFrom(src => DateTime.Now))
                .ReverseMap();

            profile.CreateMap<ChatCreateDto, ChatDto>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.SenderId, opt => opt.Ignore())
                .ForMember(dest => dest.SenderUsername, opt => opt.Ignore())
                .ForMember(dest => dest.SenderName, opt => opt.Ignore())
                .ForMember(dest => dest.ReceiverUsername, opt => opt.Ignore())
                .ForMember(dest => dest.ReceiverName, opt => opt.Ignore())
                .ForMember(dest => dest.IsEdited, opt => opt.MapFrom(src => false))
                .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => false))
                .ForMember(dest => dest.SentTime, opt => opt.MapFrom(src => DateTime.Now));
        }
    }
}