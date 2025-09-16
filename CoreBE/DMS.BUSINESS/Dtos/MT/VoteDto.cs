using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Common;
using DMS.CORE.Entities.MT;

namespace DMS.BUSINESS.Dtos.MT
{
    public class VoteDto : BaseMdDto, IMapFrom, IDto
    {
        [Key]
        public string? Id { get; set; }

        public string Title { get; set; }

        public string? Description { get; set; }

        public string? Status { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public bool? IsAnonymous { get; set; }

        public string MeetingId { get; set; }

        public DateTime? Duration { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<TblMtVotes, VoteDto>().ReverseMap();
        }
    }

    public class VoteSendRequest
    {
        public string MeetingId { get; set; }
        public string VoteId { get; set; }

        public string UserId { get; set; }
    }

    public class VoteStatusRequest
    {
        public string Status { get; set; }
    }

    public class AnswerVoteDto
    {
        public string QuestionId { get; set; }
        public string QuestionText { get; set; }
        public List<AnswerDto> Answers { get; set; }
    }

    public class AnswerDto
    {
        public string OptionId { get; set; }
        public string OptionText { get; set; }
        public UserAnswerDto User { get; set; }
    }

    public class UserAnswerDto
    {
        public string? UserName { get; set; }

        public string? UserId { get; set; }
    }
}