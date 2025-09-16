using Microsoft.AspNetCore.SignalR;

namespace DMS.API.Hubs
{
    public class MeetingVoteHub : Hub
    {
        // Gửi tin nhắn đến tất cả clients
        public async Task SendVote(string user, string votes)
        {
            await Clients.All.SendAsync("ReceiveVote", user, votes);
        }

        // Gửi tin nhắn đến một nhóm cụ thể
        public async Task SendVoteoGroup(string group, string user, string message)
        {
            await Clients.Group(group).SendAsync("ReceiveVote", user, message);
        }

        // Join vào một nhóm
        public async Task JoinGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            await Clients.Group(groupName).SendAsync("UserJoined", Context.ConnectionId);
        }

        // Leave khỏi nhóm
        public async Task LeaveGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
            await Clients.Group(groupName).SendAsync("UserLeft", Context.ConnectionId);
        }

        // Xử lý khi client kết nối
        public override async Task OnConnectedAsync()
        {
            Console.WriteLine($"Client connected: {Context.ConnectionId}");
            await Clients.All.SendAsync("UserConnected", Context.ConnectionId);
            await base.OnConnectedAsync();
        }

        // Xử lý khi client ngắt kết nối
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            Console.WriteLine($"Client disconnected: {Context.ConnectionId}");
            await Clients.All.SendAsync("UserDisconnected", Context.ConnectionId);
            await base.OnDisconnectedAsync(exception);
        }
    }
}