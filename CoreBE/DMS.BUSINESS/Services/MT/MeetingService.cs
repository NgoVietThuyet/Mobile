using AutoMapper;
using Common;
using DMS.BUSINESS.Common;
using DMS.BUSINESS.Dtos.CM;
using DMS.BUSINESS.Dtos.MD;
using DMS.BUSINESS.Dtos.MT;
using DMS.BUSINESS.Models;
using DMS.BUSINESS.Services.CM;
using DMS.CORE;
using DMS.CORE.Entities.MD;
using DMS.CORE.Entities.MT;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Minio;
using Minio.DataModel.Args;
using Minio.Exceptions;
using NPOI.SS.Formula.Functions;
using Org.BouncyCastle.Crypto.Agreement.JPake;
using System.Reflection;

namespace DMS.BUSINESS.Services.MT
{
    public interface IMeetingService : IGenericService<TblMtMeeting, MeetingDto>
    {
        Task<List<TblMtMeetingFile>> GetPersonalDocuments(string username, string meetingId);
        Task<List<TblMtMeetingFile>> UploadFilePersonalDocuments(List<IFormFile> files, string username, string meetingId);
        Task<(byte[], string, string)> GetDocument(string filePath);
        Task<MeetingCliModel> GetDetaiMeeting(string meetId, string userId);
        Task PutFileObject(string fileId, Stream stream, long contentLength, string userName = null, string documentId = null, string phieuId = null, string fileIdFound = null);
        Task<TblMtVoteReport> InsertResultVote(TblMtVoteResult result);
        Task<TblMtMeetingFile> PutFileObjectDraw(string filePath, Stream stream, long contentLength, string MettingId, string UserName, string fileName, string fileIdFound = null);
        Task<TblMtMeetingFile> CreateFileFromTemplate(CreateFileFromTemplateRequest request);
        Task<List<TblMtMeeting>> GetMeetingsByUser(string userId);
        Task<TblMtMeetingFile> ShareFileAllMeet(CreateFileFromTemplateRequest request);
        
        Task<TblMtVotes> EndVote(string voteId);
        Task<TblMtVotes> GetVoteDetail(string voteId);
        Task<TblMtMeeting> StartMeeting(string meetingId);
        Task<TblMtMeeting> EndMeeting(string meetingId);
    }

    public class MeetingService : GenericService<TblMtMeeting, MeetingDto>, IMeetingService
    {
        private readonly IConfiguration _configuration;
        private readonly IMinioService _minioService;
        private readonly IMinioClient _minioClient;

        public MeetingService(AppDbContext dbContext, IMapper mapper, IConfiguration configuration, IMinioClient minioClient, IMinioService minioService)
            : base(dbContext, mapper)
        {
            _configuration = configuration;
            _minioService = minioService;
            _minioClient = minioClient;
        }

        public override async Task<PagedResponseDto> Search(BaseFilter filter)
        {
            try
            {
                var query = _dbContext.TblMtMeeting.AsQueryable();
                if (!string.IsNullOrWhiteSpace(filter.KeyWord))
                {
                    query = query.Where(x => x.Name.Contains(filter.KeyWord));
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

        public async Task<List<TblMtMeetingFile>> UploadFilePersonalDocuments(List<IFormFile> files, string username, string meetingId)
        {
            try
            {
                var data = new List<TblMtMeetingFile>();

                var bucket = _configuration["Minio:BucketName"];
                bool found = await _minioClient.BucketExistsAsync(new BucketExistsArgs().WithBucket(bucket));
                if (!found)
                {
                    await _minioClient.MakeBucketAsync(new MakeBucketArgs().WithBucket(bucket));
                }

                foreach (var file in files)
                {
                    if (file == null || file.Length == 0)
                    {
                        continue;
                    }

                    var fileId = Guid.NewGuid().ToString();
                    var now = DateTime.Now;
                    var folderPath = $"{now:yyyy_MM_dd}";
                    var ext = Path.GetExtension(file.FileName);
                    var objectName = $"{folderPath}_{fileId}{ext}";
                    var contentType = file.ContentType;

                    using (var stream = file.OpenReadStream())
                    {
                        var putObjectArgs = new PutObjectArgs()
                            .WithBucket(bucket)
                            .WithObject(objectName)
                            .WithStreamData(stream)
                            .WithObjectSize(stream.Length)
                            .WithContentType(contentType);

                        await _minioClient.PutObjectAsync(putObjectArgs);
                    }

                    data.Add(new TblMtMeetingFile
                    {
                        Id = fileId,
                        MeetingId = meetingId,
                        UserName = username,
                        UserId = username,
                        Name = file.FileName,
                        FileName = file.FileName,
                        FileType = contentType,
                        FilePath = objectName,
                        IsSharedAll = false,
                    });
                }

                await _dbContext.TblMtMeetingFile.AddRangeAsync(data);
                await _dbContext.SaveChangesAsync();

                return data;
            }
            catch (Exception ex)
            {
                this.Status = false;
                this.Exception = ex;
                return null;
            }
        }

        public async Task<(byte[], string, string)> GetDocument(string filePath)
        {
            try
            {
                var bucketName = _configuration["Minio:BucketName"];
                var objectName = filePath;

                var statObjectArgs = new StatObjectArgs()
                    .WithBucket(bucketName)
                    .WithObject(objectName);
                var stat = await _minioClient.StatObjectAsync(statObjectArgs);

                var memoryStream = new MemoryStream();
                var getObjectArgs = new GetObjectArgs()
                    .WithBucket(bucketName)
                    .WithObject(objectName)
                    .WithCallbackStream(stream => stream.CopyTo(memoryStream));

                await _minioClient.GetObjectAsync(getObjectArgs);

                memoryStream.Position = 0;

                var fileDb = _dbContext.TblMtMeetingFile.FirstOrDefault(x => x.FilePath == filePath);

                return (memoryStream.ToArray(), fileDb == null ? stat.ObjectName : fileDb.FileName, stat.ContentType);
            }
            catch (ObjectNotFoundException)
            {
                Status = false;
                Exception = new ArgumentException("File không tồn tại trên MinIO");
                return (null, null, null);
            }
            catch (Exception ex)
            {
                Status = false;
                Exception = ex;
                return (null, null, null);
            }
        }

        public async Task<TblMtMeetingFile> PutFileObjectDraw(string filePath, Stream stream, long contentLength, string MettingId, string UserName, string fileName, string fileIdFound = null)
        {
            try
            {
                var now = DateTime.Now.ToString("HHmmss-ddMMyyyy");
                var Fileid = Guid.NewGuid().ToString();
                var bucketName = _configuration["Minio:BucketName"];
                var objectName = fileName != "whiteboard" ? filePath : ("draw-" + now + "-" + MettingId);
                // Kiểm tra bucket tồn tại
                bool found = await _minioClient.BucketExistsAsync(new BucketExistsArgs().WithBucket(bucketName));
                if (!found)
                {
                    await _minioClient.MakeBucketAsync(new MakeBucketArgs().WithBucket(bucketName));
                }
                var putObjectArgs = new PutObjectArgs()
                        .WithBucket(bucketName)
                        .WithObject(objectName)
                        .WithStreamData(stream)
                        .WithObjectSize(contentLength)
                        .WithContentType("application/octet-stream");
                await _minioClient.PutObjectAsync(putObjectArgs);

                if (fileName == "whiteboard")
                {
                    var MeetingFile = new TblMtMeetingFile();
                    MeetingFile.Id = Fileid;
                    MeetingFile.MeetingId = MettingId;
                    MeetingFile.Name = "draw-" + now + "-" + MettingId;
                    MeetingFile.FileName = "draw-" + now;
                    MeetingFile.FilePath = "draw-" + now + "-" + MettingId;
                    MeetingFile.FileType = "draw";
                    MeetingFile.UserId = UserName;

                    await _dbContext.TblMtMeetingFile.AddAsync(MeetingFile);
                    await _dbContext.SaveChangesAsync();
                    return MeetingFile;
                }
                return null;
            }
            catch (Exception ex)
            {
                Status = false;
                Exception = ex;
                return null;
            }
        }
        public async Task PutFileObject(string fileId, Stream stream, long contentLength, string userName = null, string documentId = null, string phieuId = null, string fileIdFound = null)

        {
            try
            {
                var bucketName = _configuration["Minio:BucketName"];
                var objectName = fileId;
                // Kiểm tra bucket tồn tại
                bool found = await _minioClient.BucketExistsAsync(new BucketExistsArgs().WithBucket(bucketName));
                if (!found)
                {
                    await _minioClient.MakeBucketAsync(new MakeBucketArgs().WithBucket(bucketName));
                }
                var putObjectArgs = new PutObjectArgs()
                        .WithBucket(bucketName)
                        .WithObject(objectName)
                        .WithStreamData(stream)
                        .WithObjectSize(contentLength)
                        .WithContentType("application/octet-stream");
                await _minioClient.PutObjectAsync(putObjectArgs);



            }
            catch (Exception ex)
            {
                Status = false;
                Exception = ex;
            }
        }

        public async Task<List<TblMtMeetingFile>> GetPersonalDocuments(string username, string meetingId)
        {
            return await _dbContext.TblMtMeetingFile.Where(x => x.UserId == username && x.MeetingId == meetingId).OrderByDescending(x => x.CreateDate).ToListAsync();
        }


        public async Task<MeetingCliModel> GetDetaiMeeting(string meetId, string userId)
        {
            try
            {
                var meetingInfo = await _dbContext.TblMtMeeting.FirstOrDefaultAsync(x => x.Id == meetId);
                if (meetingInfo == null)
                {
                    this.Status = false;
                    return null;
                }
                var lstMember = _dbContext.TblMtMeetingMember.Where(x => x.MeetingId == meetId).ToList();
                if (lstMember.FirstOrDefault(x => x.UserId == userId) == null)
                {
                    this.Status = false;
                    return null;
                }
                var lstUser = _dbContext.TblAdUser.OrderBy(x => x.PKID).ToList();
                var lstVoice = _dbContext.TblCmFile.Where(x => x.RefrenceFileId == meetId).OrderBy(x => x.CreateDate).ToList();
                var lstFile = _dbContext.TblMtMeetingFile.Where(x => x.MeetingId == meetId).ToList();
                var memberFile = _dbContext.TblMtMeetingMemberFile.Where(x => x.UserId == userId && x.MeetingId == meetId).ToList();
                var personalFile = lstFile.Where(file => memberFile.Any(mf => mf.FileId == file.Id)).ToList();
                personalFile.AddRange(lstFile.Where(x => x.UserId == userId).ToList());
                

                var MemberMeet = lstMember.Select(x =>
                {
                    var matchedUser = lstUser.FirstOrDefault(u => u.PKID == x.UserId);
                    return new MemberMeeting
                    {
                        UserId = x.UserId,
                        UserName = matchedUser != null ? matchedUser.FullName : x.GuestName,
                        MeetingId = x.MeetingId,
                        IsCameraOff = false,
                        IsMicMuted = true,
                        Type = x.Type,
                    };
                }).ToList();

                foreach (var i in lstVoice.Where(x => x.FileType == "webm").ToList())
                {
                    i.FilePath = await _minioService.GetPresignedUrl(i.FilePath);
                }

                var result = new MeetingCliModel()
                {
                    MeetInfor = meetingInfo,
                    PersonalDocuments = personalFile,
                    MeetingDocuments = lstFile.Where(x => x.IsSharedAll == true && x.FileType != "draw").ToList(),
                    SeatAssignments = _dbContext.TblMtSeatAssignments.Where(x => x.MeetingId == meetId).ToList(),
                    MeetingDraw = lstFile.Where(x => x.FileType == "draw").ToList(),
                    Voices = lstVoice.Where(x => x.FileType != "docx").ToList(),
                    MeetingVotes = _dbContext.TblMtVotes.Where(x => x.MeetingId == meetId).ToList(),
                    Summary = _dbContext.TblCmFile.OrderByDescending(x => x.CreateDate).FirstOrDefault(x => x.FileType == "docx" && x.RefrenceFileId == meetId),
                    MemberMeeting = MemberMeet
                };

                return result;
            }
            catch (Exception ex)
            {
                this.Status = false;
                return null;
            }
        }


        public async Task<TblMtMeetingFile> CreateFileFromTemplate(CreateFileFromTemplateRequest request)
        {
            try
            {
                var (fileBytes, fileName, contentType) = await GetDocument(request.FilePath);

                var newFile = await _minioService.UploadFileFromBytes(fileBytes, $"{request.NewFileName}{Path.GetExtension(fileName)}", contentType);

                newFile.UserId = request.UserId;
                newFile.IsSharedAll = true;
                newFile.MeetingId = request.MeetId;

                _dbContext.TblMtMeetingFile.Add(newFile);
                _dbContext.SaveChanges();

                return newFile;
            }
            catch (Exception ex)
            {
                this.Status = false;
                return null;
            }
        }


        public async Task<TblMtMeetingFile> ShareFileAllMeet(CreateFileFromTemplateRequest request)
        {
            try
            {
                var (fileBytes, fileName, contentType) = await GetDocument(request.FilePath);

                var newFile = await _minioService.UploadFileFromBytes(fileBytes, $"{request.NewFileName}{Path.GetExtension(fileName)}", contentType);

                newFile.UserId = null;
                newFile.MeetingId = request.MeetId;
                newFile.IsSharedAll = true;

                _dbContext.TblMtMeetingFile.Add(newFile);
                _dbContext.SaveChanges();

                return newFile;
            }
            catch (Exception ex)
            {
                this.Status = false;
                return null;
            }
        }

        public async Task<List<TblMtMeeting>> GetMeetingsByUser(string userId)
        {
            try
            {
                var meetings = await _dbContext.TblMtMeeting
                    .Where(meeting => _dbContext.TblMtMeetingMember
                        .Any(member => member.UserId == userId && member.MeetingId == meeting.Id))
                    .ToListAsync();

                this.Status = true;
                return meetings;
            }
            catch (Exception ex)
            {
                this.Status = false;
                return null;
            }
        }

        //Task IMeetingService.PutFileObjectDraw(string fileId, Stream stream, long contentLength, string MettingId, string UserName, string FileName, string fileIdFound)
        //{
        //    return PutFileObjectDraw(fileId, stream, contentLength, MettingId, UserName, FileName, fileIdFound);
        //}

        
        public async Task<TblMtVotes> GetVoteDetail(string voteId)
        {
            try
            {
                var data = await _dbContext.TblMtVotes.FirstOrDefaultAsync(x => x.Id == voteId);
                data.Status = "START";
                _dbContext.TblMtVotes.Update(data);
                _dbContext.SaveChanges();

                return data;
            }
            catch (Exception ex)
            {
                this.Status = false;
                return null;
            }
        }
        public async Task<TblMtVoteReport> InsertResultVote(TblMtVoteResult result)
        {
            try
            {
                var lstVote = _dbContext.TblMtVoteResult
                                        .Where(x => x.VoteId == result.VoteId)
                                        .ToList();

                var vote = lstVote.FirstOrDefault(x => x.UserId == result.UserId);

                var report = _dbContext.TblMtVoteReport
                                       .FirstOrDefault(x => x.VoteId == result.VoteId);

                if (vote == null)
                {
                    vote = new TblMtVoteResult
                    {
                        Id = Guid.NewGuid().ToString(),
                        UserId = result.UserId,
                        MeetingId = result.MeetingId,
                        Result = result.Result,
                        VoteId = result.VoteId
                    };

                    _dbContext.TblMtVoteResult.Add(vote); 
                    lstVote.Add(vote);                   
                }
                else
                {
                    vote.Result = result.Result;
                }

                if (report == null)
                {
                    report = new TblMtVoteReport
                    {
                        Id = Guid.NewGuid().ToString(),
                        VoteId = result.VoteId,
                        MeetingId = result.MeetingId,
                        Y = 0,
                        N = 0,
                        K = 0,
                        IsActive = true
                    };
                    _dbContext.TblMtVoteReport.Add(report);
                }

                report.Y = lstVote.Count(x => x.Result == "Y");
                report.N = lstVote.Count(x => x.Result == "N");
                report.K = lstVote.Count(x => x.Result == "K");

                await _dbContext.SaveChangesAsync();
                return report;
            }
            catch (Exception ex)
            {
                this.Status = false;
                // log ex ở đây
                return null;
            }
        }

        public async Task<TblMtVotes> EndVote(string voteId)
        {
            try
            {
                var vote = _dbContext.TblMtVotes.FirstOrDefault(x => x.Id == voteId);
                vote.Status = "END";
                await _dbContext.SaveChangesAsync();
                return vote;
            }
            catch (Exception ex)
            {
                this.Status = false;
                return null;
            }
        }

        public async Task<TblMtMeeting> StartMeeting(string meetingId)
        {
            try
            {
                var meeting = _dbContext.TblMtMeeting.FirstOrDefault(x => x.Id == meetingId);
                meeting.Status = "START";
                await _dbContext.SaveChangesAsync();
                return meeting;
            }
            catch (Exception ex)
            {
                this.Status = false;
                return null;
            }
        }

        public async Task<TblMtMeeting> EndMeeting(string meetingId)
        {
            try
            {
                var meeting = _dbContext.TblMtMeeting.FirstOrDefault(x => x.Id == meetingId);
                meeting.Status = "END";
                await _dbContext.SaveChangesAsync();
                return meeting;
            }
            catch (Exception ex)
            {
                this.Status = false;
                return null;
            }
        }
    }

}
