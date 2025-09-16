using DMS.BUSINESS.Services.HUB;
using DMS.CORE;
using Microsoft.AspNetCore.SignalR;

namespace DMS.BUSINESS.Services.MT
{
    public interface IVoteNotificationService
    {
        Task NotifyVoteStarted(string meetingId, string voteId);
        Task NotifyVoteEnded(string meetingId, string voteId);
    }

    public class VoteNotificationService(AppDbContext dbContext, IHubContext<VoteMeetingHub> hubContext) : IVoteNotificationService
    {
        private readonly AppDbContext _dbContext = dbContext;
        private readonly IHubContext<VoteMeetingHub> _hubContext = hubContext;

        public async Task NotifyVoteEnded(string meetingId, string voteId)
        {
            await _hubContext.Clients.Group($"Vote_Meeting_{meetingId}_{voteId}")
            .SendAsync("VoteEnded", new { VoteId = voteId, message = "Kết thúc biểu quyết" });
        }

        public async Task NotifyVoteStarted(string meetingId, string voteId)
        {
            await _hubContext.Clients.Group($"Vote_Meeting_{meetingId}_{voteId}")
            .SendAsync("ReceiveVote", new { VoteId = voteId, Time = DateTime.Now });
        }
    }
}