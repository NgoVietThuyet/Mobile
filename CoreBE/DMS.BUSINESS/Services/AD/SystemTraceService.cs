using AutoMapper;
using Common;
using DMS.BUSINESS.Common;
using DMS.BUSINESS.Common.Enum;
using DMS.BUSINESS.Dtos.AD;
using DMS.BUSINESS.Services.HUB;
using DMS.CORE;
using DMS.CORE.Entities.AD;
using Hangfire;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Net.NetworkInformation;

namespace DMS.BUSINESS.Services.AD
{
    public interface ISystemTraceService : IGenericService<TblAdSystemTrace, SystemTraceDto>
    {
        Task StartService();
    }
    public class SystemTraceService(AppDbContext dbContext, IMapper mapper, IHubContext<SystemTraceServiceHub> hubContext) : GenericService<TblAdSystemTrace, SystemTraceDto>(dbContext, mapper), ISystemTraceService
    {
        public readonly IHubContext<SystemTraceServiceHub> _hubContext = hubContext;

        public async Task StartService()
        {
            var systemTraces = await _dbContext.TblAdSystemTrace.ToListAsync();
            systemTraces.ForEach(trace =>
            {
                if (trace.Type == SystemTraceType.HTTP.ToString() || trace.Type == SystemTraceType.TCP_IP.ToString())
                {
                    RecurringJob.AddOrUpdate(trace.Code, () => SendPing(trace.Code, trace.Address), $"*/{trace.InterVal} * * * *");
                }
            });
        }

        public async Task SendPing(string code, string ipAddress)
        {
            try
            {
                var trace = await _dbContext.TblAdSystemTrace.FirstOrDefaultAsync(x => x.Code == code);
                Ping pingSender = new();
                SystemTraceResponseDto response;
                PingReply reply = await pingSender.SendPingAsync(ipAddress);

                if (reply.Status == IPStatus.Success)
                {
                    response = new SystemTraceResponseDto()
                    {
                        Code = code,
                        Status = true,
                        Message = $"Ping to {ipAddress} successful. Time: {reply.RoundtripTime}ms",
                        CheckTime = DateTime.Now,
                    };
                }
                else
                {
                    response = new SystemTraceResponseDto()
                    {
                        Code = code,
                        Status = false,
                        Message = $"Ping to {ipAddress} failed. Status: {reply.Status}",
                        CheckTime = DateTime.Now,
                    };
                }
                trace.Status = response.Status;
                trace.Log = response.Message;
                trace.LastCheckTime = DateTime.Now;

                _dbContext.Update(trace);
                await _dbContext.SaveChangesAsync();
                await _hubContext.Clients.All.SendAsync(SignalRMethod.SYSTEM_TRACE.ToString(), response);
            }
            catch (Exception ex)
            {
                var response = new SystemTraceResponseDto()
                {
                    Code = code,
                    Status = null,
                    Message = $"Ping to {ipAddress} failed. Exception: {ex.Message}",
                    CheckTime = DateTime.Now,
                };
                LoggerService.LogError(ex.Message);
                LoggerService.LogError(ex.StackTrace);
                await _hubContext.Clients.All.SendAsync(SignalRMethod.SYSTEM_TRACE.ToString(), response);
            }
        }

        public override async Task<SystemTraceDto> Add(IDto dto)
        {
            var result = await base.Add(dto);
            if (this.Status)
            {
                RecurringJob.AddOrUpdate(result.Code, () => SendPing(result.Code, result.Address), $"*/{result.Interval} * * * *");
                await SendPing(result.Code, result.Address);
            }
            return result;
        }

        public override async Task Delete(object code)
        {
            await base.Delete(code);
            if (this.Status)
            {
                RecurringJob.RemoveIfExists(code as string);
            }
        }

        public override async Task Update(IDto dto)
        {
            var model = dto as SystemTraceCreateUpdateDto;
            await base.Update(dto);
            if (this.Status)
            {
                RecurringJob.AddOrUpdate(model.Code, () => SendPing(model.Code, model.Address), $"*/{model.Interval} * * * *");
                await SendPing(model.Code, model.Address);
            }
        }
    }

}
