

using DMS.BUSINESS.Dtos.MT;
using DMS.CORE;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DMS.BUSINESS.Services.MT
{
    public class VoteSchedulerService(IServiceProvider serviceProvider, AppDbContext dbContext) : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider = serviceProvider;
        private readonly AppDbContext _dbContext = dbContext;
        private readonly Timer _timer;
        private readonly Dictionary<string, List<Timer>> _voteTimers = [];

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await CheckAndScheduleVotes();
                    await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken); // Kiểm tra mỗi phút
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }

        private async Task CheckAndScheduleVotes()
        {
            using var scope = _serviceProvider.CreateScope();
            var voteService = scope.ServiceProvider.GetRequiredService<IVoteService>();
            var notificationService = scope.ServiceProvider.GetRequiredService<IVoteNotificationService>();

            var activeVotes = await GetActiveVotes();

            foreach (var vote in activeVotes)
            {
                ScheduleVoteNotifications(vote, notificationService);
            }
        }

        private async Task<List<VoteDto>> GetActiveVotes()
        {
            var now = DateTime.Now;

            var upcomingVotes = await _dbContext.TblMtVotes
            .Where(v => v.Status == "APPROVED" &&
                       v.StartTime.HasValue &&
                       v.StartTime.Value > now &&
                       v.StartTime.Value <= now.AddMinutes(3))
            .Select(v => new VoteDto
            {
                Id = v.Id,
                Title = v.Title,
                Status = v.Status,
                StartTime = v.StartTime,
                EndTime = v.EndTime,
                Description = v.Description
            })
            .OrderBy(v => v.StartTime)
            .ToListAsync();

            return upcomingVotes;
        }


        private void ScheduleVoteNotifications(VoteDto vote, IVoteNotificationService notificationService)
        {
            if (!vote.StartTime.HasValue || !vote.EndTime.HasValue) return;

            var now = DateTime.Now;
            var startTime = vote.StartTime.Value;
            var endTime = vote.EndTime.Value;

            // Nếu vote đã có timers, bỏ qua
            if (_voteTimers.ContainsKey(vote.Id)) return;

            var timers = new List<Timer>();

            // 1. Thông báo khi vote bắt đầu
            if (startTime > now)
            {
                var startDelay = startTime - now;
                var startTimer = new Timer(async _ => await notificationService.NotifyVoteStarted(vote.MeetingId, vote.Id),
                    null, startDelay, Timeout.InfiniteTimeSpan);
                timers.Add(startTimer);
            }


            // 5. Thông báo kết thúc
            var endDelay = endTime - now;
            var endTimer = new Timer(async _ =>
            {
                await notificationService.NotifyVoteEnded(vote.MeetingId, vote.Id);
                // Cleanup timers
                if (_voteTimers.ContainsKey(vote.Id))
                {
                    foreach (var timer in _voteTimers[vote.Id])
                    {
                        timer?.Dispose();
                    }
                    _voteTimers.Remove(vote.Id);
                }
            }, null, endDelay, Timeout.InfiniteTimeSpan);
            timers.Add(endTimer);

            _voteTimers[vote.Id] = timers;
        }

    }
}