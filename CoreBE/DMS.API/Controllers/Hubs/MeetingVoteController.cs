using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using DMS.API.Hubs;

namespace DMS.API.Controllers.Hubs
{
    [Route("api/[controller]")]
    [ApiController]
    public class MeetingVoteController(IHubContext<MeetingVoteHub> _hubContext) : ControllerBase
    {
        private readonly IHubContext<MeetingVoteHub> hubContext = _hubContext;

        [HttpPost("send")]
        public async Task<IActionResult> SendVote([FromBody] VotesRequest request)
        {
            await hubContext.Clients.All.SendAsync("ReceiveVote", request.User, request.Votes);
            return Ok(new { success = true, message = "Votes sent successfully" });
        }

        [HttpPost("send-to-group")]
        public async Task<IActionResult> SendVoteToGroup([FromBody] GroupVotesRequest request)
        {
            await hubContext.Clients.Group(request.GroupName)
                .SendAsync("ReceiveMessage", request.User, request.Votes);
            return Ok(new { success = true, message = "Message sent to group successfully" });
        }

        [HttpPost("notification")]
        public async Task<IActionResult> SendNotification([FromBody] NotificationRequest request)
        {
            await hubContext.Clients.All.SendAsync("ReceiveNotificationVote", request.Title, request.Message);
            return Ok(new { success = true, message = "Notification sent successfully" });
        }

        [HttpGet("connections")]
        public IActionResult GetConnections()
        {
            // Trong thực tế, bạn sẽ cần lưu trữ connections trong database hoặc cache
            return Ok(new { message = "Use SignalR events to track connections" });
        }
    }

    // DTOs
    public class VotesRequest
    {
        public string User { get; set; } = string.Empty;
        public string Votes { get; set; } = string.Empty;
    }

    public class GroupVotesRequest : VotesRequest
    {
        public string GroupName { get; set; } = string.Empty;
    }

    public class NotificationRequest
    {
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }
}