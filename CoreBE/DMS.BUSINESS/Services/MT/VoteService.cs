using AutoMapper;
using Common;
using DMS.BUSINESS.Common;
using DMS.BUSINESS.Dtos.MT;
using DMS.BUSINESS.Models;
using DMS.BUSINESS.Services.HUB;
using DMS.CORE;
using DMS.CORE.Entities.MT;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace DMS.BUSINESS.Services.MT
{
    public interface IVoteService : IGenericService<TblMtVotes, VoteDto>
    {
        Task CreateVoteMeeting(VotesModels votesModels);

        Task<bool> UpdateVoteMeeting(VotesModels data);

        Task<List<VoteDto>> GetListVoteByMeetingId(string meetingId, string userId);

        Task<List<VoteDto>> GetListVoteByMeetingIdForAdmin(string meetingId);

        Task<List<VoteDto>> GetListVoteByMeetingIdForHost(string meetingId);

        Task<bool> DeleteVote(string voteId);

        Task<VoteDto> UpdateStatusVote(string voteId, string status);

        Task<VotesModels> GetDetailVotes(string voteId);

        Task<List<VotesQuestionsModels>> JoinVote(string voteId, string userId);

        Task<bool> SubmitVoteForm(ResultVoteRequest resultVoteRequest, string userId);

        Task<List<VotesQuestionsModels>> GetVoteQuestions(string voteId);

        Task GetStatisticVote(string voteId);
        Task<bool> CheckStatusVote(string voteId);

        Task<bool> StartVote(string voteId);
        Task<List<AnswerVoteDto>> GetAnswerVote(string voteId, string meetingId);
    }

    public class VoteService(AppDbContext dbContext, IMapper mapper) : GenericService<TblMtVotes, VoteDto>(dbContext, mapper), IVoteService
    {
        public async Task CreateVoteMeeting(VotesModels votesModels)
        {
            try
            {
                if (string.IsNullOrEmpty(votesModels.Votes.MeetingId))
                {
                    this.Status = false;
                    this.MessageObject.MessageDetail = "Có lỗi khi thêm phiếu biểu quyết. Vui lòng thử lại!";
                }
                else
                {
                    var id = Guid.NewGuid().ToString();
                    votesModels.Votes.Id = id;
                    _dbContext.TblMtVotes.Add(votesModels.Votes);

                    await AddQuestions(votesModels.VoteQuestions, id);
                    await _dbContext.SaveChangesAsync();
                }


            }
            catch
            {
                this.Status = false;
                this.MessageObject.MessageDetail = "Có lỗi khi thêm phiếu biểu quyết. Vui lòng thử lại!";
            }
        }

        public async Task AddQuestions(List<VotesQuestionsModels> questions, string votesId)
        {
            try
            {
                foreach (var question in questions)
                {
                    if (string.IsNullOrEmpty(question.Config.QuestionText))
                    {
                        throw new Exception("Nội dụng câu hỏi không được để trống");
                    }

                    if (question.Config.VoteType != "SHORT_ANSWER" && question.VoteOptions.Count == 0)
                    {
                        throw new Exception("Cần thêm option cho câu hỏi");
                    }

                    if (string.IsNullOrEmpty(question.Config.Id))
                    {
                        var id = Guid.NewGuid().ToString();
                        question.Config.Id = id;
                        question.Config.VoteId = votesId;
                        await AddOptionsVote(question.VoteOptions, id);
                        await _dbContext.TblMtVoteQuestion.AddAsync(question.Config);
                    }
                    else
                    {
                        var qt = await _dbContext.TblMtVoteQuestion.FindAsync(question.Config.Id);
                        if (qt != null)
                        {
                            qt.QuestionText = question.Config.QuestionText;
                            qt.QuestionOrder = question.Config.QuestionOrder;
                            qt.VoteType = question.Config.VoteType;
                            qt.AllowMultipleChoice = question.Config.AllowMultipleChoice;
                            qt.UpdateDate = DateTime.Now;

                            _dbContext.TblMtVoteQuestion.Update(qt);

                            await DeleteOptionsAsync(question.IdsOptionDelete);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi thêm câu hỏi", ex);
            }
        }

        public async Task AddOptionsVote(List<TblMtVoteOption> options, string questionId)
        {
            try
            {
                foreach (var option in options)
                {
                    if (string.IsNullOrEmpty(option.OptionText))
                    {
                        throw new Exception("Nội dụng câu trả lời không được để trống");
                    }

                    if (string.IsNullOrEmpty(option.Id))
                    {
                        var id = Guid.NewGuid().ToString();
                        option.Id = id;
                        option.QuestionId = questionId;
                        await _dbContext.TblMtVoteOption.AddAsync(option);
                    }
                    else
                    {
                        var qt = await _dbContext.TblMtVoteOption.FindAsync(option.Id);

                        if (qt != null)
                        {
                            qt.OptionText = option.OptionText;
                            qt.IsOtherOption = option.IsOtherOption;
                            qt.OtherText = option.OtherText;
                            qt.IsOtherOption = option.IsOtherOption;
                            qt.UpdateDate = DateTime.Now;

                            _dbContext.TblMtVoteOption.Update(qt);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi thêm options cho câu hỏi", ex);
            }
        }

        public async Task<bool> DeleteOptionsAsync(List<string> ids)
        {
            try
            {
                if (ids == null || ids.Count == 0)
                    return false;

                var optionsToDelete = await _dbContext.TblMtVoteOption
                    .Where(q => ids.Contains(q.Id))
                    .ToListAsync();

                if (optionsToDelete.Count == 0)
                    return false;

                _dbContext.TblMtVoteOption.RemoveRange(optionsToDelete);

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi khi xoá option.", ex);
            }
        }

        public async Task<bool> DeleteVote(string voteId)
        {
            try
            {
                var vote = await _dbContext.TblMtVotes.FindAsync(voteId);

                if (vote == null)
                {
                    this.Status = false;
                    this.MessageObject.MessageDetail = "Không tồn tại biểu quyết";
                    return false;
                }

                _dbContext.TblMtVotes.Remove(vote);
                await _dbContext.SaveChangesAsync();

                return true;
            }
            catch
            {
                this.Status = false;
                this.MessageObject.MessageDetail = "Có lỗi khi xoá vote";
                return false;
            }
        }

        public async Task<VotesModels> GetDetailVotes(string voteId)
        {
            try
            {
                var votes = await _dbContext.TblMtVotes
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == voteId);

                if (votes == null)
                {
                    this.Status = false;
                    this.MessageObject.MessageDetail = "Không tìm thấy biểu quyết nào.";
                    return null;
                }

                var voteResult = _dbContext.TblMtVoteReport.FirstOrDefault(x => x.VoteId == voteId);

                var questions = await _dbContext.TblMtVoteQuestion
                    .AsNoTracking()
                    .Where(x => x.VoteId == voteId)
                    .ToListAsync();

                var questionIds = questions.Select(q => q.Id).ToList();

                var options = await _dbContext.TblMtVoteOption
                    .AsNoTracking()
                    .Where(o => questionIds.Contains(o.QuestionId))
                    .ToListAsync();



                var resultQuestions = questions.Select(q =>
                {
                    var qOptions = options.Where(o => o.QuestionId == q.Id).ToList();
                    return new VotesQuestionsModels
                    {
                        Config = q,
                        VoteOptions = qOptions
                    };
                }).ToList();


                return new VotesModels
                {
                    Votes = votes,
                    VoteQuestions = resultQuestions,
                    VoteReport = voteResult,
                    VoteResult = _dbContext.TblMtVoteResult.Where(x => x.VoteId == voteId).ToList(),
                };
            }
            catch
            {
                this.Status = false;
                this.MessageObject.MessageDetail = "Có lỗi khi lấy chi tiết biểu quyết";
                return null;
            }
        }

        public async Task<List<VoteDto>> GetListVoteByMeetingId(string meetingId, string userId)
        {
            try
            {
                // if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(meetingId))
                // {
                //     return [];
                // }

                // var userMt = await _dbContext.TblMtMeetingMember.FirstOrDefaultAsync(mb => mb.MeetingId == meetingId && mb.UserId == userId);

                // if (userMt == null)
                // {
                //     return [];
                // }

                // if (userMt.Type == "HOST")
                // {
                //     return await GetListVoteByMeetingIdForHost(meetingId);
                // }

                return await GetListVoteByMeetingIdForAdmin(meetingId);
            }
            catch
            {
                this.Status = false;
                this.MessageObject.MessageDetail = "Có lỗi khi lấy danh sách biểu quyết.";
                return null;
            }
        }

        public async Task<List<VoteDto>> GetListVoteByMeetingIdForAdmin(string meetingId)
        {
            try
            {
                var result = await _dbContext.TblMtVotes.Where(v => v.MeetingId == meetingId).Select(v => new VoteDto
                {
                    Id = v.Id,
                    MeetingId = v.MeetingId,
                    Title = v.Title,
                    Description = v.Description,
                    Status = v.Status,
                    StartTime = v.StartTime,
                    EndTime = v.EndTime,
                    IsAnonymous = v.IsAnonymous,
                }).ToListAsync();

                return result;
            }
            catch
            {
                this.Status = false;
                this.MessageObject.MessageDetail = "Có lỗi khi lấy danh sách biểu quyết.";
                return null;
            }
        }

        public async Task<List<VoteDto>> GetListVoteByMeetingIdForHost(string meetingId)
        {
            try
            {
                var result = await _dbContext.TblMtVotes.Where(v => v.MeetingId == meetingId && v.Status != "DRAFT").Select(v => new VoteDto
                {
                    Id = v.Id,
                    MeetingId = v.MeetingId,
                    Title = v.Title,
                    Description = v.Description,
                    Status = v.Status,
                    StartTime = v.StartTime,
                    EndTime = v.EndTime,
                    IsAnonymous = v.IsAnonymous,
                }).ToListAsync();

                return result;
            }
            catch
            {
                this.Status = false;
                this.MessageObject.MessageDetail = "Có lỗi khi lấy danh sách biểu quyết.";
                return null;
            }
        }

        public Task GetStatisticVote(string voteId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<VotesQuestionsModels>> GetVoteQuestions(string voteId)
        {
            try
            {
                var questions = await _dbContext.TblMtVoteQuestion
                            .Where(q => q.VoteId == voteId)
                            .ToListAsync();

                var questionIds = questions.Select(q => q.Id).ToList();

                var options = await _dbContext.TblMtVoteOption
                .Where(o => questionIds.Contains(o.QuestionId))
                .ToListAsync();

                var result = questions.Select(q => new VotesQuestionsModels
                {
                    Config = q,
                    VoteOptions = [.. options.Where(o => o.QuestionId == q.Id)]
                }).ToList();

                return result;
            }
            catch
            {
                this.Status = false;
                this.MessageObject.MessageDetail = "Xảy ra lỗi khi lấy danh sách câu hỏi";
                return null;
            }
        }

        public async Task<List<VotesQuestionsModels>> JoinVote(string voteId, string userId)
        {
            try
            {
                var voteExists = await _dbContext.TblMtVotes.FirstOrDefaultAsync(x => x.Id == voteId);
                if (voteExists == null)
                {
                    this.Status = false;
                    this.MessageObject.MessageDetail = "Không tồn tại cuộc bỏ phiếu này";
                    return null;
                }

                var questions = await GetVoteQuestions(voteId);

                var existing = await _dbContext.TblMtVoteParticipants
                    .FirstOrDefaultAsync(x => x.VoteId == voteId && x.UserId == userId);

                if (existing != null)
                {
                    if (existing.Status == "IN_PROGRESS")
                        return questions;


                    this.Status = false;
                    this.MessageObject.MessageDetail = "Bạn đã tham gia và nộp phiếu rồi.";
                    return null;
                }

                var id = Guid.NewGuid().ToString();
                var participant = new TblMtVoteParticipants
                {
                    Id = id,
                    VoteId = voteId,
                    UserId = userId,
                    Status = "IN_PROGRESS"
                };

                _dbContext.TblMtVoteParticipants.Add(participant);
                await _dbContext.SaveChangesAsync();
                return questions;
            }
            catch
            {
                this.Status = false;
                this.MessageObject.MessageDetail = "Có lỗi khi tham gia biểu quyết.";
                return null;
            }
        }

        public async Task<bool> SubmitVoteForm(ResultVoteRequest resultVoteRequest, string userId)
        {
            try
            {
                if (string.IsNullOrEmpty(resultVoteRequest.VoteId))
                {
                    this.Status = false;
                    this.MessageObject.MessageDetail = "Có lỗi khi biểu quyết";
                    return false;
                }

                var vote = await _dbContext.TblMtVotes.FindAsync(resultVoteRequest.VoteId);

                if (vote == null)
                {
                    this.Status = false;
                    this.MessageObject.MessageDetail = "Có lỗi khi biểu quyết";
                    return false;
                }

                if (vote.Status == "COMPLETED")
                {
                    this.Status = false;
                    this.MessageObject.MessageDetail = "Biểu quyết đã kết thúc";
                    return false;
                }

                var user = await _dbContext.TblMtVoteParticipants.FindAsync(userId);

                if (user == null)
                {
                    this.Status = false;
                    this.MessageObject.MessageDetail = "Có lỗi khi biểu quyết";
                    return false;
                }

                user.Status = "SUBMITTED";
                user.SubmittedAt = DateTime.Now;

                _dbContext.TblMtVoteParticipants.Update(user);

                var responses = new List<TblMtVoteResponse>();
                var voteId = resultVoteRequest.VoteId;


                foreach (var item in resultVoteRequest.Result)
                {
                    foreach (var option in item.OptionsSelected)
                    {

                        responses.Add(new TblMtVoteResponse
                        {
                            Id = Guid.NewGuid().ToString(),
                            UserId = userId,
                            QuestionId = item.QuestionId,
                            OptionId = option.Id,
                            ResponseText = option.Type == "other" ? item.ResponseText : null,
                        });

                    }
                }

                await _dbContext.TblMtVoteResponse.AddRangeAsync(responses);
                await _dbContext.SaveChangesAsync();

                return true;
            }
            catch
            {

                this.Status = false;
                this.MessageObject.MessageDetail = "Có lỗi khi gửi biểu quyết";
                return false;
            }
        }

        public async Task<VoteDto> UpdateStatusVote(string voteId, string status)
        {
            try
            {
                var vote = await _dbContext.TblMtVotes.FindAsync(voteId);

                if (vote == null)
                {
                    this.Status = false;
                    this.MessageObject.MessageDetail = "Không tìm thấy thông tin biểu quyết";
                    return null;
                }

                vote.Status = status;
                _dbContext.TblMtVotes.Update(vote);

                var voteDto = _mapper.Map<TblMtVotes, VoteDto>(vote);

                return voteDto;
            }
            catch
            {
                this.Status = false;
                this.MessageObject.MessageDetail = "Có lỗi khi chuyển trạng thái biểu quyết";
                return null;
            }
        }

        public async Task<bool> UpdateVoteMeeting(VotesModels data)
        {
            try
            {
                if (string.IsNullOrEmpty(data.Votes.Id))
                {
                    this.Status = false;
                    this.MessageObject.MessageDetail = "Thiếu ID biểu quyết";
                    return false;
                }

                if (data.VoteQuestions == null || data.VoteQuestions.Count <= 0)
                {
                    this.Status = false;
                    this.MessageObject.MessageDetail = "Câu hỏi không được để trống!";
                    return false;
                }

                var vote = await _dbContext.TblMtVotes.FindAsync(data.Votes.Id);

                if (vote == null)
                {
                    this.Status = false;
                    this.MessageObject.MessageDetail = "Không tìm thấy thông tin biểu quyết";
                    return false;
                }

                data.Votes.UpdateDate = DateTime.Now;
                _dbContext.TblMtVotes.Update(data.Votes);

                await DeleteQuestionsAsync(data.IdsQuestionDelete);
                await AddQuestions(data.VoteQuestions, data.Votes.Id);

                await _dbContext.SaveChangesAsync();

                return true;
            }
            catch
            {
                this.Status = false;
                this.MessageObject.MessageDetail = "Đã có lỗi xảy ra khi cập nhật thông tin biểu quyết!";
                return false;
            }
        }

        public async Task<bool> DeleteQuestionsAsync(List<string> ids)
        {
            try
            {
                if (ids == null || ids.Count == 0)
                    return false;

                var questionsToDelete = await _dbContext.TblMtVoteQuestion
                    .Where(q => ids.Contains(q.Id))
                    .ToListAsync();

                if (questionsToDelete.Count == 0)
                    return false;

                _dbContext.TblMtVoteQuestion.RemoveRange(questionsToDelete);

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi khi xoá câu hỏi.", ex);
            }
        }

        public async Task<bool> CheckStatusVote(string voteId)
        {
            try
            {
                var vote = await _dbContext.TblMtVotes.FindAsync(voteId);

                if (vote == null) return false;

                if (vote.Status != "APPROVED") return false;

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> StartVote(string voteId)
        {
            try
            {
                var checkStatus = await CheckStatusVote(voteId);

                if (!checkStatus)
                {
                    this.Status = false;
                    this.MessageObject.MessageDetail = "Biểu quyết ở trạng thái không hợp lệ";

                    return false;
                }

                await UpdateStatusVote(voteId, "IN_PROGRESS");

                return true;
            }
            catch
            {
                this.Status = false;
                return false;
            }
        }

        public async Task<List<AnswerVoteDto>> GetAnswerVote(string voteId, string meetingId)
        {
            try
            {
                var questions = await (
                    from qt in _dbContext.TblMtVoteQuestion
                    where qt.VoteId == voteId
                    join rs in _dbContext.TblMtVoteResponse
                        on qt.Id equals rs.QuestionId into responses
                    select new AnswerVoteDto
                    {
                        QuestionId = qt.Id!,
                        QuestionText = qt.QuestionText!,
                        Answers = (
                            from r in responses
                            join mb in _dbContext.TblMtMeetingMember
                                on new { r.UserId, MeetingId = meetingId }
                                equals new { mb.UserId, mb.MeetingId }
                            select new AnswerDto
                            {
                                OptionId = r.OptionId!,
                                OptionText = r.ResponseText!,
                                User = new UserAnswerDto
                                {
                                    UserName = mb.UserName,
                                    UserId = mb.UserId
                                }
                            }
                        ).ToList()
                    }
                ).ToListAsync();

                return questions;
            }
            catch
            {
                this.Status = false;
                return null;
            }
        }
    }

}
