using Microsoft.AspNetCore.SignalR;

namespace DMS.BUSINESS.Services.HUB
{
    public class VoteMeetingHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            var userId = Context.GetHttpContext()?.Request.Query["user_id"].ToString();
            var meetingId = Context.GetHttpContext()?.Request.Query["meeting_id"].ToString();

            if (!string.IsNullOrEmpty(userId))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, $"Vote_Meeting_{meetingId}");
                Context.Items["user_id"] = userId;
                Context.Items["meeting_id"] = meetingId;
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var userId = Context.Items["user_id"]?.ToString();
            var meetingId = Context.Items["meeting_id"]?.ToString();

            if (!string.IsNullOrEmpty(userId) && !string.IsNullOrEmpty(meetingId))
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"Vote_Meeting_{meetingId}");
            }

            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendVoteGroup(string meetingId, string voteId, string message)
        {
            await Clients.Group($"Vote_Meeting_{meetingId}_{voteId}").SendAsync("ReceiveVote", message);
        }
    }
}
