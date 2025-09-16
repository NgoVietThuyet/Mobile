using AutoMapper;
using Common;
using DMS.BUSINESS.Common;
using DMS.BUSINESS.Dtos.MT;
using DMS.BUSINESS.Models;
using DMS.BUSINESS.Services.CM;
using DMS.CORE;
using DMS.CORE.Entities.MT;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Minio;
using Minio.DataModel.Args;
using Minio.Exceptions;
using System.ComponentModel.DataAnnotations;

namespace DMS.BUSINESS.Services.MT
{
    public interface IChatService : IGenericService<TblMtChat, ChatDto>
    {
        Task<ChatDto> SendMessage(ChatCreateDto chatCreateDto, string senderId);
        Task<ChatDto> EditMessage(string messageId, string newContent, string senderId);
        Task<bool> DeleteMessage(string messageId, string senderId);

        Task<List<ChatDto>> GetPublicMessages(string meetingId, int page = 1, int pageSize = 50);
        Task<List<ChatDto>> GetPrivateMessages(string meetingId, string userId, string targetUserId, int page = 1, int pageSize = 50);
        Task<List<ChatDto>> GetAllMessagesForUser(string meetingId, string userId, int page = 1, int pageSize = 50);

        Task<ChatDto> SendFileMessage(ChatCreateDto chatCreateDto, IFormFile file, string senderId);
        Task<(byte[], string, string)> GetAttachmentFile(string filePath);

        Task<bool> ValidateUserInMeeting(string meetingId, string userId);
        Task<ChatDto> GetMessageById(string messageId, string senderId);
        Task<List<ChatDto>> SearchMessages(string meetingId, string keyword, string userId, int page = 1, int pageSize = 20);
        Task<ChatDto> ReplyToMessage(ChatCreateDto chatCreateDto, string replyToMessageId, string senderId);
    }

    public class ChatService : GenericService<TblMtChat, ChatDto>, IChatService
    {
        private readonly IConfiguration _configuration;
        private readonly IMinioService _minioService;
        private readonly IMinioClient _minioClient;

        public ChatService(AppDbContext dbContext, IMapper mapper, IConfiguration configuration,
            IMinioClient minioClient, IMinioService minioService)
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
                var query = _dbContext.TblMtChat.AsQueryable();

                if (!string.IsNullOrWhiteSpace(filter.KeyWord))
                {
                    query = query.Where(x => x.Content.Contains(filter.KeyWord) ||
                                           x.SenderName.Contains(filter.KeyWord));
                }

                if (filter.IsActive.HasValue)
                {
                    query = query.Where(x => x.IsActive == filter.IsActive);
                }

                query = query.Where(x => x.IsDeleted == false);

                return await Paging(query, filter);
            }
            catch (Exception ex)
            {
                Status = false;
                Exception = ex;
                return null;
            }
        }

        public async Task<ChatDto> SendMessage(ChatCreateDto chatCreateDto, string senderId)
        {
            try
            {
                // Validate user is in meeting
                if (await ValidateUserInMeeting(chatCreateDto.MeetingId, senderId))
                {
                    Status = false;
                    Exception = new UnauthorizedAccessException("User không có quyền gửi tin nhắn trong cuộc họp này");
                    return null;
                }

                // Validate business rules
               
                if (chatCreateDto.MessageType == "D")
                {
                    if (await ValidateUserInMeeting(chatCreateDto.MeetingId, chatCreateDto.ReceiverId))
                    {
                        Status = false;
                        Exception = new ArgumentException("Người nhận không tồn tại trong cuộc họp");
                        return null;
                    }
                }

                // Get sender information from meeting member
                var senderInfo = await _dbContext.TblMtMeetingMember
                  .Join(_dbContext.TblAdUser,
                        member => member.UserId,
                        user => user.PKID,
                        (member, user) => new { Member = member, User = user })
                  .FirstOrDefaultAsync(x => x.Member.MeetingId == chatCreateDto.MeetingId && x.Member.UserId == senderId);

                var chatEntity = _mapper.Map<TblMtChat>(chatCreateDto);
                chatEntity.Id = Guid.NewGuid().ToString();
                chatEntity.SenderId = senderId;
                chatEntity.SenderUsername = null;
                chatEntity.SenderName = senderInfo?.User.FullName;
                chatEntity.CreateBy = senderId;
                chatEntity.CreateDate = DateTime.Now;
                chatEntity.SentTime = DateTime.Now;
                chatEntity.IsActive = true;
                chatEntity.IsDeleted = false;
                chatEntity.IsEdited = false;

                // Set receiver information for private chat
                if (chatCreateDto.MessageType == "D" && !string.IsNullOrEmpty(chatCreateDto.ReceiverId))
                {
                    var receiverInfo = await _dbContext.TblMtMeetingMember
                        .FirstOrDefaultAsync(x => x.MeetingId == chatCreateDto.MeetingId && x.UserId == chatCreateDto.ReceiverId);

                    chatEntity.ReceiverUsername = receiverInfo?.UserName;
                    chatEntity.ReceiverName = receiverInfo?.GuestName;
                }

                await _dbContext.TblMtChat.AddAsync(chatEntity);
                await _dbContext.SaveChangesAsync();

                return _mapper.Map<ChatDto>(chatEntity);
            }
            catch (Exception ex)
            {
                Status = false;
                Exception = ex;
                return null;
            }
        }

        public async Task<ChatDto> SendFileMessage(ChatCreateDto chatCreateDto, IFormFile file, string senderId)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    Status = false;
                    Exception = new ArgumentException("File không hợp lệ");
                    return null;
                }

                // Validate file size (max 50MB)
                const long maxFileSize = 50 * 1024 * 1024; // 50MB
                if (file.Length > maxFileSize)
                {
                    Status = false;
                    Exception = new ArgumentException("File không được vượt quá 50MB");
                    return null;
                }

                // Upload file to MinIO
                var bucket = _configuration["Minio:BucketName"];
                bool found = await _minioClient.BucketExistsAsync(new BucketExistsArgs().WithBucket(bucket));
                if (!found)
                {
                    await _minioClient.MakeBucketAsync(new MakeBucketArgs().WithBucket(bucket));
                }

                var fileId = Guid.NewGuid().ToString();
                var now = DateTime.Now;
                var folderPath = $"chat/{now:yyyy_MM_dd}";
                var ext = Path.GetExtension(file.FileName);
                var objectName = $"{folderPath}_{fileId}{ext}";

                using (var stream = file.OpenReadStream())
                {
                    var putObjectArgs = new PutObjectArgs()
                        .WithBucket(bucket)
                        .WithObject(objectName)
                        .WithStreamData(stream)
                        .WithObjectSize(stream.Length)
                        .WithContentType(file.ContentType);

                    await _minioClient.PutObjectAsync(putObjectArgs);
                }

                // Update DTO with file information
                chatCreateDto.FilePath = objectName;
                chatCreateDto.FileName = file.FileName;
                chatCreateDto.FileSize = file.Length;
                chatCreateDto.ContentType = file.ContentType.StartsWith("image/") ? "IMAGE" : "FILE";
                chatCreateDto.Content = $"[File: {file.FileName}]";

                return await SendMessage(chatCreateDto, senderId);
            }
            catch (Exception ex)
            {
                Status = false;
                Exception = ex;
                return null;
            }
        }

        public async Task<(byte[], string, string)> GetAttachmentFile(string filePath)
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

                var fileDb = await _dbContext.TblMtChat.FirstOrDefaultAsync(x => x.FilePath == filePath);
                return (memoryStream.ToArray(), fileDb?.FileName ?? stat.ObjectName, stat.ContentType);
            }
            catch (ObjectNotFoundException)
            {
                Status = false;
                Exception = new ArgumentException("File không tồn tại");
                return (null, null, null);
            }
            catch (Exception ex)
            {
                Status = false;
                Exception = ex;
                return (null, null, null);
            }
        }

        public async Task<ChatDto> EditMessage(string messageId, string newContent, string senderId)
        {
            try
            {
                var message = await _dbContext.TblMtChat.FirstOrDefaultAsync(x => x.Id == messageId && x.IsDeleted == false);
                if (message == null)
                {
                    Status = false;
                    Exception = new ArgumentException("Tin nhắn không tồn tại");
                    return null;
                }

                if (message.SenderId != senderId)
                {
                    Status = false;
                    Exception = new UnauthorizedAccessException("Chỉ có thể chỉnh sửa tin nhắn của chính mình");
                    return null;
                }

                // Only allow editing text messages
                if (message.ContentType != "TEXT")
                {
                    Status = false;
                    Exception = new ArgumentException("Chỉ có thể chỉnh sửa tin nhắn văn bản");
                    return null;
                }

                message.Content = newContent;
                message.IsEdited = true;
                message.UpdateBy = senderId;
                message.UpdateDate = DateTime.Now;

                await _dbContext.SaveChangesAsync();
                return _mapper.Map<ChatDto>(message);
            }
            catch (Exception ex)
            {
                Status = false;
                Exception = ex;
                return null;
            }
        }

        public async Task<bool> DeleteMessage(string messageId, string senderId)
        {
            try
            {
                var message = await _dbContext.TblMtChat.FirstOrDefaultAsync(x => x.Id == messageId && x.IsDeleted == false);
                if (message == null)
                {
                    Status = false;
                    Exception = new ArgumentException("Tin nhắn không tồn tại");
                    return false;
                }

                if (message.SenderId != senderId)
                {
                    Status = false;
                    Exception = new UnauthorizedAccessException("Chỉ có thể xóa tin nhắn của chính mình");
                    return false;
                }

                message.IsDeleted = true;

                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Status = false;
                Exception = ex;
                return false;
            }
        }

        public async Task<List<ChatDto>> GetPublicMessages(string meetingId, int page = 1, int pageSize = 50)
        {
            try
            {
                var messages = await _dbContext.TblMtChat
                    .Where(x => x.MeetingId == meetingId &&
                               x.MessageType == "P" &&
                               x.IsDeleted == false &&
                               x.IsActive == true)
                    .OrderByDescending(x => x.SentTime)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                return _mapper.Map<List<ChatDto>>(messages.OrderBy(x => x.SentTime));
            }
            catch (Exception ex)
            {
                Status = false;
                Exception = ex;
                return null;
            }
        }

        public async Task<List<ChatDto>> GetPrivateMessages(string meetingId, string userId, string targetUserId, int page = 1, int pageSize = 50)
        {
            try
            {
                var messages = await _dbContext.TblMtChat
                    .Where(x => x.MeetingId == meetingId &&
                               x.MessageType == "D" &&
                               ((x.SenderId == userId && x.ReceiverId == targetUserId) ||
                                (x.SenderId == targetUserId && x.ReceiverId == userId)) &&
                               x.IsDeleted == false &&
                               x.IsActive == true)
                    .OrderByDescending(x => x.SentTime)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                return _mapper.Map<List<ChatDto>>(messages.OrderBy(x => x.SentTime));
            }
            catch (Exception ex)
            {
                Status = false;
                Exception = ex;
                return null;
            }
        }

        public async Task<List<ChatDto>> GetAllMessagesForUser(string meetingId, string userId, int page = 1, int pageSize = 50)
        {
            try
            {
                var messages = await _dbContext.TblMtChat
                    .Where(x => x.MeetingId == meetingId &&
                               x.IsDeleted == false &&
                               x.IsActive == true &&
                               (x.MessageType == "P" ||
                                x.SenderId == userId ||
                                x.ReceiverId == userId))
                    .OrderByDescending(x => x.SentTime)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                return _mapper.Map<List<ChatDto>>(messages.OrderBy(x => x.SentTime));
            }
            catch (Exception ex)
            {
                Status = false;
                Exception = ex;
                return null;
            }
        }

        public async Task<List<ChatDto>> SearchMessages(string meetingId, string keyword, string userId, int page = 1, int pageSize = 20)
        {
            try
            {
                var messages = await _dbContext.TblMtChat
                    .Where(x => x.MeetingId == meetingId &&
                               x.IsDeleted == false &&
                               x.IsActive == true &&
                               (x.MessageType == "P" || x.SenderId == userId || x.ReceiverId == userId) &&
                               (x.Content.Contains(keyword) || x.SenderName.Contains(keyword)))
                    .OrderByDescending(x => x.SentTime)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                return _mapper.Map<List<ChatDto>>(messages);
            }
            catch (Exception ex)
            {
                Status = false;
                Exception = ex;
                return null;
            }
        }

        public async Task<ChatDto> ReplyToMessage(ChatCreateDto chatCreateDto, string replyToMessageId, string senderId)
        {
            try
            {
                // Validate the original message exists and user can see it
                var originalMessage = await _dbContext.TblMtChat
                    .FirstOrDefaultAsync(x => x.Id == replyToMessageId &&
                                            x.MeetingId == chatCreateDto.MeetingId &&
                                            x.IsDeleted == false &&
                                            x.IsActive == true);

                if (originalMessage == null)
                {
                    Status = false;
                    Exception = new ArgumentException("Tin nhắn gốc không tồn tại");
                    return null;
                }

                // Check if user can see the original message (for private chats)
                if (originalMessage.MessageType == "D" &&
                    originalMessage.SenderId != senderId &&
                    originalMessage.ReceiverId != senderId)
                {
                    Status = false;
                    Exception = new UnauthorizedAccessException("Không có quyền reply tin nhắn này");
                    return null;
                }

                chatCreateDto.ReplyToMessageId = replyToMessageId;
                return await SendMessage(chatCreateDto, senderId);
            }
            catch (Exception ex)
            {
                Status = false;
                Exception = ex;
                return null;
            }
        }

        public async Task<bool> ValidateUserInMeeting(string meetingId, string userId)
        {
            try
            {
                return await _dbContext.TblMtMeetingMember
                    .AnyAsync(x => x.MeetingId == meetingId && x.UserId == userId && x.IsActive == true);
            }
            catch (Exception ex)
            {
                Status = false;
                Exception = ex;
                return false;
            }
        }

        public async Task<ChatDto> GetMessageById(string messageId, string senderId)
        {
            try
            {
                var message = await _dbContext.TblMtChat
                    .FirstOrDefaultAsync(x => x.Id == messageId && x.IsDeleted == false && x.IsActive == true);

                if (message == null)
                {
                    Status = false;
                    Exception = new ArgumentException("Tin nhắn không tồn tại");
                    return null;
                }

                // Check if user has permission to view this message
                if (message.MessageType == "D" &&
                    message.SenderId != senderId &&
                    message.ReceiverId != senderId)
                {
                    Status = false;
                    Exception = new UnauthorizedAccessException("Không có quyền xem tin nhắn này");
                    return null;
                }

                return _mapper.Map<ChatDto>(message);
            }
            catch (Exception ex)
            {
                Status = false;
                Exception = ex;
                return null;
            }
        }
    }
}