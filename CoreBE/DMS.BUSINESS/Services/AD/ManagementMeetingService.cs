using Microsoft.EntityFrameworkCore;
using DMS.BUSINESS.Common;
using DMS.BUSINESS.Dtos.AD;
using DMS.BUSINESS.Filter.AD;
using DMS.CORE;
using DMS.CORE.Entities.AD;
using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using DMS.BUSINESS.Services.HUB;
using Common;
using Common.Util;
using DMS.BUSINESS.Common.Enum;
using Org.BouncyCastle.Tsp;
using System.Net.Http.Headers;
using DMS.CORE.Entities.MT;
using DMS.BUSINESS.Dtos.MT;
using DMS.BUSINESS.Models;
using Microsoft.AspNetCore.Http;
using DocumentFormat.OpenXml.Office2010.Excel;
using Aspose.Words.XAttr;
namespace DMS.BUSINESS.Services.AD
{
    public interface IManagementMeetingService : IGenericService<TblMtMeeting, MeetingDto>
    {
        Task<PagedResponseDto> Search(BaseFilter filter);
        Task<List<TblMtMeeting>> GetAllMeeting();
        Task InsertMeeting(MeetingModels data);
        Task<MeetingModels> GetDataMeeting(string id);
        Task UpdateMeeting(MeetingModels data);
    }
    public class ManagementMeetingService(AppDbContext dbContext, IMapper mapper, IHubContext<RefreshServiceHub> hubContext) : GenericService<TblMtMeeting, MeetingDto>(dbContext, mapper), IManagementMeetingService
    {

        public async Task<PagedResponseDto> Search(BaseFilter filter)
        {
            try
            {
                var query = _dbContext.TblMtMeeting
                .AsQueryable();

                if (!string.IsNullOrWhiteSpace(filter.KeyWord))
                {
                    query = query.Where(x =>
                        x.Name.Contains(filter.KeyWord)
                    );
                }

                if (filter.IsActive.HasValue)
                {
                    query = query.Where(x => x.IsActive == filter.IsActive);
                }


                return await Paging(query, filter);
            }
            catch (Exception ex)
            {
                Status = false;
                Exception = ex;
                return null;
            }
        }
        public async Task<List<TblMtMeeting>> GetAllMeeting()
        {
            try
            {
                return _dbContext.TblMtMeeting.OrderBy(x => x.CreateDate).ToList();
            }
            catch (Exception ex)
            {
                this.Status = false;
                return null;
            }
        }

        public async Task InsertMeeting(MeetingModels data)
        {
            try
            {
                var id = Guid.NewGuid().ToString();

                data.MeetInfor.Id = id;
                _dbContext.TblMtMeeting.Add(data.MeetInfor);

                await AddFiles(data.Files, id);
                await AddMember(data.Members, "MEMBER", id);
                await AddMember(data.Secretaries, "SECRETARY", id);
                await AddMember([data.HostMeeting], "HOST", id);
                await AddSeatAssignments(data.SeatAssignments, id);

                foreach(var v in data.Votes)
                {
                    v.Id = Guid.NewGuid().ToString();
                    v.MeetingId = id;
                }
                _dbContext.TblMtVotes.AddRange(data.Votes);

                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                this.Status = false;

            }
        }


        public async Task UpdateMeeting(MeetingModels data)
        {
            try
            {
                var lstRemove = _dbContext.TblMtMeetingMember.Where(f => f.MeetingId == data.MeetInfor.Id).ToList() ?? new List<TblMtMeetingMember>();
                var lstRemoveSeat = _dbContext.TblMtSeatAssignments.Where(f => f.MeetingId == data.MeetInfor.Id).ToList() ?? new List<TblMtSeatAssignments>();
                var lstRemoveMemberFile = _dbContext.TblMtMeetingMemberFile.Where(f => f.MeetingId == data.MeetInfor.Id).ToList() ?? new List<TblMtMeetingMemberFile>();
                _dbContext.TblMtMeetingMember.RemoveRange(lstRemove);
                _dbContext.TblMtSeatAssignments.RemoveRange(lstRemoveSeat);
                _dbContext.TblMtMeetingMemberFile.RemoveRange(lstRemoveMemberFile);

                await AddMember(data.Members, "MEMBER", data.MeetInfor.Id);
                await AddMember(data.Secretaries, "SECRETARY", data.MeetInfor.Id);
                await AddMember([data.HostMeeting], "HOST", data.MeetInfor.Id);
                await AddSeatAssignments(data.SeatAssignments, data.MeetInfor.Id);
                await UpdateVotes(data.MeetInfor.Id, data.Votes);

                await AddFiles(data.Files, data.MeetInfor.Id);

                _dbContext.TblMtMeeting.Update(data.MeetInfor);

                await _dbContext.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                this.Status = false;
            }
        }

        public async Task UpdateVotes(string meetingId, List<TblMtVotes> votes)
        {
            try
            {
                foreach (var v in votes)
                {
                    if (v.Id == "-")
                    {
                        v.Id = Guid.NewGuid().ToString();
                        v.MeetingId = meetingId;
                        await _dbContext.TblMtVotes.AddAsync(v);
                    }
                    else
                    {
                        _dbContext.TblMtVotes.Update(v);
                    }
                }
                var lstRemove = votes.Where(x => x.IsActive == false).ToList();
                 _dbContext.TblMtVotes.RemoveRange(lstRemove);

            }
            catch (Exception ex) 
            {
            }
        }

        public async Task<MeetingModels> GetDataMeeting(string id)
        {
            try
            {
                var lstfile = _dbContext.TblMtMeetingFile
                       .Where(x => x.MeetingId == id)
                       .ToList();
                var lstMember = await _dbContext.TblMtMeetingMember
                       .Where(x => x.MeetingId == id)
                       .ToListAsync();
                var lstMemberFile = _dbContext.TblMtMeetingMemberFile.Where(x => x.MeetingId == id).ToList();
                var lstFiles = new List<FileModels>();
                foreach (var file in lstfile)
                {
                    lstFiles.Add(new FileModels
                    {
                        File = file,
                        MemberFile = lstMemberFile.Where(x => x.FileId == file.Id).Select(x => x.UserId).ToList(),
                    });
                }
                return new MeetingModels()
                {
                    MeetInfor = await _dbContext.TblMtMeeting.FirstOrDefaultAsync(x => x.Id == id),
                    memberMeeting = lstMember,
                    Secretaries = lstMember.Where(x => x.Type == "SECRETARY").Select(x => x.UserId).ToList(),
                    HostMeeting = lstMember.FirstOrDefault(x => x.Type == "HOST")?.UserId ?? "",
                    Members = lstMember.Where(x => x.Type == "MEMBER").Select(x => x.UserId).ToList(),
                    SeatAssignments = _dbContext.TblMtSeatAssignments.Where(x => x.MeetingId == id).ToList(),
                    Votes = _dbContext.TblMtVotes.Where(x => x.MeetingId == id).ToList(),
                    Files = lstFiles
                };
            }
            catch (Exception ex)
            {
                this.Status = false;
                return null;
            }
        }

        public async Task AddFiles(List<FileModels> Files, string meetId)
        {
            try
            {
                var lstFileRemove = _dbContext.TblMtMeetingFile.Where(f => f.MeetingId == meetId).ToList() ?? new List<TblMtMeetingFile>();
                _dbContext.TblMtMeetingFile.RemoveRange(lstFileRemove);

                var lstFile = new List<TblMtMeetingFile>();
                var lstMemberFile = new List<TblMtMeetingMemberFile>();

                foreach (var item in Files)
                {
                    var id = Guid.NewGuid().ToString();

                    item.File.Id = id;
                    item.File.MeetingId = meetId;

                    lstMemberFile.AddRange(item.MemberFile.Select(x => new TblMtMeetingMemberFile
                    {
                        Id = Guid.NewGuid().ToString(),
                        MeetingId = meetId,
                        FileId = id,
                        UserId = x,
                        UserName = x,
                    }).ToList());
                    lstFile.Add(item.File);
                }
                await _dbContext.TblMtMeetingFile.AddRangeAsync(lstFile);
                await _dbContext.TblMtMeetingMemberFile.AddRangeAsync(lstMemberFile);
            }
            catch (Exception ex)
            {
            }
        }

        public async Task AddMember(List<string> data, string type, string meetId)
        {
            try
            {
                var lstMember = new List<TblMtMeetingMember>();
                lstMember.AddRange(data.Select(x => new TblMtMeetingMember
                {
                    Id = Guid.NewGuid().ToString(),
                    MeetingId = meetId,
                    UserId = x,
                    Type = type,
                }).ToList());

                await _dbContext.TblMtMeetingMember.AddRangeAsync(lstMember);
            }
            catch (Exception ex)
            {
            }
        }

        public async Task AddSeatAssignments(List<TblMtSeatAssignments> data, string meetId)
        {
            try
            {
                foreach (var i in data)
                {
                    i.Id = Guid.NewGuid().ToString();
                    i.MeetingId = meetId;
                }

                await _dbContext.TblMtSeatAssignments.AddRangeAsync(data);
            }
            catch (Exception ex)
            {
            }
        }
    }
}
