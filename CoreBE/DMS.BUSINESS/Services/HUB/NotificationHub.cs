using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace DMS.BUSINESS.Services.HUB
{
    public class NotificationHub : Hub
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly List<Claim> _Claims;

        /// Khi client kết nối thành công
        public override async Task OnConnectedAsync()
        {
            var userId = Context.GetHttpContext()?.Request.Query["user_id"].ToString();
            var meetingId = Context.GetHttpContext()?.Request.Query["meeting_id"].ToString();
            Console.WriteLine($"[SignalR] New connection: ConnectionId={Context.ConnectionId}, user_name={userId}");
            if (!string.IsNullOrEmpty(userId))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, meetingId);
                Context.Items["user_id"] = userId;
                Context.Items["meeting_id"] = meetingId;
            }
            await base.OnConnectedAsync();
        }

        /// Khi client ngắt kết nối
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var userId = Context.Items["user_id"]?.ToString();
            var meetingId = Context.Items["meeting_id"]?.ToString();

            Console.WriteLine($"[SignalR] Disconnected: ConnectionId={Context.ConnectionId}, userName={userId}, meetingId={meetingId}");
            if (!string.IsNullOrEmpty(userId) && !string.IsNullOrEmpty(meetingId))
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, meetingId);
            }
            await base.OnDisconnectedAsync(exception);
        }

        /// Gửi thông báo đến một user cụ thể (qua group)
        public async Task SendNotification(string userName, string meetingId, string message)
        {
            Console.WriteLine($"SendNotification to userId={userName}, meetingId={meetingId}, message={message}");
            await Clients.Group($"User_{userName}_Meeting_{meetingId}").SendAsync("ReceiveNotification", message);
        }
    }
}
