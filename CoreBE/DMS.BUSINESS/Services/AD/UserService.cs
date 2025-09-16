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
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using Minio.DataModel.Args;
using Microsoft.Extensions.Configuration;
using Minio;
using DMS.CORE.Entities.CM;
using DMS.BUSINESS.Services.CM;
using System.Text.Json;

namespace DMS.BUSINESS.Services.AD
{
    public interface IUserService : IGenericService<TblUser, UserDto>
    {
        Task<PagedResponseDto> Search(UserFilter filter);
        Task<IList<UserDto>> GetAll(UserFilterLite filter);
        Task<UserDto> AddUser(IDto dto);
        Task<string> RegisterFaceAsync(string username);
        Task<string> GetUIdFace(string username);
        Task<UserDto> GetUserById(object Id);
        Task<object> AddUserGuest(string meetingId, string guestName);
        Task<object> GuestJoinMeeting(string meetingId, string meetingCode, string guestName);
        Task<object> LoginUserMember(string meetingId, string userId);
    }

    public class UserService : GenericService<TblUser, UserDto>, IUserService
    {
        private readonly IHubContext<RefreshServiceHub> _hubContext;
        private readonly IConfiguration _configuration;
        private readonly IMinioClient _minioClient;

        public UserService(
            AppDbContext dbContext,
            IMapper mapper,
            IHubContext<RefreshServiceHub> hubContext,
            IConfiguration configuration,
            IMinioClient minioClient)
            : base(dbContext, mapper)
        {
            _hubContext = hubContext;
            _configuration = configuration;
            _minioClient = minioClient;
        }
        public async Task<PagedResponseDto> Search(UserFilter filter)
        {
            try
            {
                var query = _dbContext.TblAdUser
                .AsQueryable();

                if (!string.IsNullOrWhiteSpace(filter.KeyWord))
                {
                    query = query.Where(x =>
                        x.PKID.Contains(filter.KeyWord) ||
                        x.FullName.Contains(filter.KeyWord)
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

        public async Task<IList<UserDto>> GetAll(UserFilterLite filter)
        {
            if (filter == null)
            {
                Status = false;
                MessageObject.Code = "0000";
                return null;
            }
            try
            {
                var query = _dbContext.TblAdUser
                    .AsQueryable();

                if (!string.IsNullOrWhiteSpace(filter.KeyWord))
                {
                    query = query.Where(x =>
                        x.PKID.Contains(filter.KeyWord)
                    );
                }

                if (filter.IsActive.HasValue)
                {
                    query = query.Where(x => x.IsActive == filter.IsActive);
                }
                query = query.OrderByDescending(x => x.CreateDate);
                return _mapper.Map<IList<UserDto>>(await query.ToListAsync());
            }
            catch (Exception ex)
            {
                Status = false;
                Exception = ex;
                return null;
            }
        }

        public async Task<UserDto> GetUserById(object Id)
        {
            var data = await _dbContext.TblAdUser.FirstOrDefaultAsync(x => x.PKID == Id as string);
            var result = _mapper.Map<UserDto>(data);
            return result;
        }



        public async Task<UserDto> AddUser(IDto dto)
        {
            try
            {
                var realDto = dto as UserCreateDto;
                realDto.PKID = Guid.NewGuid().ToString();

                if (!string.IsNullOrEmpty(realDto.ImageBase64))
                {
                    var bucket = _configuration["Minio:BucketName"];
                    bool found = await _minioClient.BucketExistsAsync(new BucketExistsArgs().WithBucket(bucket));
                    if (!found)
                    {
                        await _minioClient.MakeBucketAsync(new MakeBucketArgs().WithBucket(bucket));
                    }

                    var base64Data = realDto.ImageBase64.Contains(",") ? realDto.ImageBase64.Split(',')[1] : realDto.ImageBase64;
                    var imageBytes = Convert.FromBase64String(base64Data);
                    var fileId = Guid.NewGuid().ToString();
                    var now = DateTime.Now;
                    var folderPath = $"users/avatars/{now:yyyy_MM_dd}";

                    // Detect extension từ Base64 header
                    var ext = GetExtensionFromBase64(realDto.ImageBase64);
                    var contentType = GetContentTypeFromExtension(ext);
                    var objectName = $"{folderPath}_{fileId}{ext}";

                    using (var stream = new MemoryStream(imageBytes))
                    {
                        var putObjectArgs = new PutObjectArgs()
                            .WithBucket(bucket)
                            .WithObject(objectName)
                            .WithStreamData(stream)
                            .WithObjectSize(stream.Length)
                            .WithContentType(contentType);
                        await _minioClient.PutObjectAsync(putObjectArgs);
                    }

                    // Lưu vào T_CM_FILE
                    await _dbContext.TblCmFile.AddAsync(new TblCmFile
                    {
                        Id = fileId,
                        RefrenceFileId = realDto.PKID,
                        FileName = $"avatar_{realDto.PKID}.jpg",
                        FileType = ext.Replace(".", ""),
                        FileSize = imageBytes.Length,
                         FilePath = objectName,
                        IsAllowDelete = true,
                        IsActive = true,
                        CreateDate = now,
                        IsDeleted = false
                    });

                    // Set URL
                    var protocol = bool.Parse(_configuration["Minio:UseSSL"] ?? "false") ? "https" : "http";
                    realDto.UrlImage = $"{protocol}://{_configuration["Minio:Endpoint"]}:{_configuration["Minio:Port"]}/{bucket}/{objectName}";
                }

                var data = await base.Add(dto);
                await RegisterFaceAsync(data.PKID);
                var getFaceIdResponse = await GetUIdFace(data.PKID);
                try
                {
                    using (JsonDocument jsonDoc = JsonDocument.Parse(getFaceIdResponse))
                    {
                        var dataArray = jsonDoc.RootElement.GetProperty("data");
                        if (dataArray.GetArrayLength() > 0)
                        {
                            var firstFace = dataArray[0];
                            var userId = firstFace.GetProperty("user_id").GetString();

                            var userToUpdate = await _dbContext.TblAdUser.FindAsync(data.PKID);
                            if (userToUpdate != null)
                            {
                                userToUpdate.FaceId = userId;
                                await _dbContext.SaveChangesAsync();
                            }

                            data.FaceId = userId;
                        }
                    }
                }
                catch (Exception ex)
                {
                }
                return data;
            }
            catch (Exception ex)
            {
                this.Status = false;
                this.Exception = ex;
                return null;
            }
        }

        // Helper methods
        private string GetExtensionFromBase64(string base64)
        {
            if (base64.StartsWith("data:image/jpeg") || base64.StartsWith("data:image/jpg")) return ".jpg";
            if (base64.StartsWith("data:image/png")) return ".png";
            if (base64.StartsWith("data:image/gif")) return ".gif";
            if (base64.StartsWith("data:image/webp")) return ".webp";
            return ".jpg"; 
        }

        private string GetContentTypeFromExtension(string ext)
        {
            return ext switch
            {
                ".jpg" => "image/jpeg",
                ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                ".webp" => "image/webp",
                _ => "image/jpeg"
            };
        }
        // Hàm kiểm tra 

        public async Task<object> GuestJoinMeeting(string meetingId, string meetingCode, string guestName)
        {
            try
            {
                var meetInfo = _dbContext.TblMtMeeting.FirstOrDefault(x => x.Id == meetingId);
                if (meetInfo == null || meetInfo.Code != meetingCode)
                {
                    this.Status = false;
                    return null;
                }
                var id = Guid.NewGuid().ToString();

                _dbContext.TblMtMeetingMember.Add(new TblMtMeetingMember
                {
                    Id = id,
                    MeetingId = meetingId,
                    UserId = id,
                    GuestName = guestName,
                    Type = "GUEST",
                    IsActive = true,
                });
                await _dbContext.SaveChangesAsync();

                return new
                {
                    Id = id,
                    MeetingId = meetingId,
                    UserName = id,
                    UserId = id,
                    GuestName = guestName,
                    FullName = guestName,
                    Type = "GUEST",
                    IsActive = true,
                    Message = "Guest đã được thêm thành công"
                };
            }
            catch (Exception ex)
            {
                this.Status = false;
                return null;
            }
        }


        public async Task<object> LoginUserMember(string meetingId, string userId)
        {
            try
            {
                var meetInfo = _dbContext.TblMtMeeting.FirstOrDefault(x => x.Id == meetingId);

                if (meetInfo == null)
                {
                    this.Status = false;
                    return null;
                }

                var meetingMember = _dbContext.TblMtMeetingMember.FirstOrDefault(x => x.UserId == userId && x.MeetingId == meetingId);

                if (meetingMember == null)
                {
                    this.Status = false;
                    return null;
                }
                var user = _dbContext.TblAdUser.FirstOrDefault(x => x.PKID == meetingMember.UserId);

                if (user == null)
                {
                    this.Status = false;
                    return null;
                }

                return new
                {
                    Id = userId,
                    MeetingId = meetingId,
                    UserName = userId,
                    UserId = userId,
                    FullName = user.FullName,
                    GuestName = "",
                    Type = meetingMember.Type,
                    IsActive = true,
                };
            }
            catch (Exception ex)
            {
                this.Status = false;
                return null;
            }
        }

        public async Task<object> AddUserGuest(string meetingId, string guestName)
        {
            try
            {
                var meeting = await _dbContext.TblMtMeeting.FindAsync(meetingId);

                if (meeting == null)
                {
                    Status = false;
                    return null;
                }

                var memberId = Guid.NewGuid().ToString();
                var newMeetingMember = new TblMtMeetingMember
                {
                    Id = memberId,
                    MeetingId = meetingId,
                    GuestName = guestName,
                    UserId = memberId, 
                    Type = "GUEST",
                    CreateDate = DateTime.Now,
                    IsActive = true
                };

                // Thêm vào database
                _dbContext.TblMtMeetingMember.Add(newMeetingMember);
                await _dbContext.SaveChangesAsync();

                Status = true;

                return new
                {
                    Id = memberId,
                    MeetingId = meetingId,
                    UserId = memberId,
                    GuestName = guestName,
                    FullName = guestName,
                    Type = "GUEST",
                    CreateDate = newMeetingMember.CreateDate,
                    Message = "Guest đã được thêm thành công"
                };
            }
            catch (Exception ex)
            {
                Status = false;
                Exception = ex;
                return null;
            }
        }

        public string SaveBase64ToFile(string base64String)
        {
            try
            {
                if (base64String.Contains(","))
                {
                    base64String = base64String.Split(',')[1];
                }
                byte[] fileBytes = Convert.FromBase64String(base64String);
                string rootPath = "Uploads/Images";
                string datePath = $"{DateTime.Now:yyyy/MM/dd}";
                string fullPath = Path.Combine(rootPath, datePath);
                if (!Directory.Exists(fullPath))
                {
                    Directory.CreateDirectory(fullPath);
                }
                string fileName = $"{Guid.NewGuid()}.jpg";
                string filePath = Path.Combine(fullPath, fileName);
                File.WriteAllBytes(filePath, fileBytes);
                return Path.Combine("/Uploads/Images", datePath, fileName).Replace("\\", "/");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi: {ex.Message}");
                return null;
            }
        }

        public override async Task Update(IDto dto)
        {
            try
            {
                var dt = dto as UserCreateDto;
                if (dt.ImageBase64 != null && dt.ImageBase64 != "")
                {
                    dt.UrlImage = SaveBase64ToFile(dt.ImageBase64);
                }
                var model = _mapper.Map<UserCreateDto>(dto as UserCreateDto);
                await _dbContext.SaveChangesAsync();
              
            }
            catch (Exception ex)
            {
                Status = false;
                Exception = ex;
            }
        }



        public async Task<string> RegisterFaceAsync(string pkid)
        {
            var user = _dbContext.TblAdUser.Find(pkid);

            var userImageFile = await _dbContext.TblCmFile
                .Where(f => f.RefrenceFileId == pkid)
                .OrderByDescending(f => f.CreateDate)
                .FirstOrDefaultAsync();

            if (userImageFile == null)
                throw new FileNotFoundException("Không tìm thấy ảnh của user trong database");
            byte[] imageBytes;
            string fileName = userImageFile.FileName;
            string contentType = userImageFile.FileType.StartsWith("image/") ? userImageFile.FileType : "image/jpeg";
            try
            {
                var bucket = _configuration["Minio:BucketName"];
                var memoryStream = new MemoryStream();

                var getObjectArgs = new GetObjectArgs()
                    .WithBucket(bucket)
                    .WithObject(userImageFile.FilePath)
                    .WithCallbackStream(stream => stream.CopyTo(memoryStream));

                await _minioClient.GetObjectAsync(getObjectArgs);
                imageBytes = memoryStream.ToArray();

                if (imageBytes.Length == 0)
                    throw new Exception("File ảnh trống hoặc không tồn tại trên MinIO");
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi tải ảnh từ MinIO: {ex.Message}");
            }

            // Gửi API đăng ký face
            var token = "cba287859e90fd581d177d499250f6aaf0524b739377a396cfd2684303fff302";

            using (var httpClient = new HttpClient())
            using (var form = new MultipartFormDataContent())
            {
                var fileContent = new ByteArrayContent(imageBytes);
                fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse(contentType);

                form.Add(fileContent, "file", fileName);
                form.Add(new StringContent(user.PKID), "user_id");
                form.Add(new StringContent("true"), "anti_spoofing");
                form.Add(new StringContent("0.7"), "threshold_spoofing");

                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await httpClient.PostAsync("https://llm.xbot.vn/face/register-face", form);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                    throw new Exception($"Lỗi API: {response.StatusCode}, Nội dung: {responseContent}");

                return responseContent;
            }
        }

        public async Task<string> GetUIdFace(string pkid)
        {
            try
            {
                var user = _dbContext.TblAdUser.Find(pkid);
                if (user == null)
                    throw new ArgumentException($"Không tìm thấy user với PKID: {pkid}");

                // Lấy file ảnh mới nhất của user từ database
                var userImageFile = await _dbContext.TblCmFile
                    .Where(f => f.RefrenceFileId == pkid )
                    .OrderByDescending(f => f.CreateDate)
                    .FirstOrDefaultAsync();

                if (userImageFile == null)
                    throw new FileNotFoundException($"Không tìm thấy ảnh của user {pkid} trong database");

                // Lấy ảnh từ MinIO (giống như RegisterFaceAsync)
                byte[] imageBytes;
                string fileName = userImageFile.FileName;
                string contentType = userImageFile.FileType.StartsWith("image/") ? userImageFile.FileType : "image/jpeg";

                try
                {
                    var bucket = _configuration["Minio:BucketName"];
                    using var memoryStream = new MemoryStream();

                    var getObjectArgs = new GetObjectArgs()
                        .WithBucket(bucket)
                        .WithObject(userImageFile.FilePath)
                        .WithCallbackStream(stream => stream.CopyTo(memoryStream));

                    await _minioClient.GetObjectAsync(getObjectArgs);
                    imageBytes = memoryStream.ToArray();

                    if (imageBytes.Length == 0)
                        throw new Exception("File ảnh trống hoặc không tồn tại trên MinIO");
                }
                catch (Exception ex)
                {
                    throw new Exception($"Lỗi khi tải ảnh từ MinIO: {ex.Message}", ex);
                }

                // Gọi Face Search API
                var token = _configuration["FaceApi:Token"] ?? "cba287859e90fd581d177d499250f6aaf0524b739377a396cfd2684303fff302";
                var apiUrl = _configuration["FaceApi:SearchUrl"] ?? "https://llm.xbot.vn/face/search-face";

                using var httpClient = new HttpClient();
                using var form = new MultipartFormDataContent();

                var fileContent = new ByteArrayContent(imageBytes);
                fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse(contentType);

                form.Add(fileContent, "file", fileName);
                form.Add(new StringContent("true"), "anti_spoofing");
                form.Add(new StringContent(_configuration["FaceApi:ThresholdSpoofing"] ?? "0.7"), "threshold_spoofing");
                form.Add(new StringContent(_configuration["FaceApi:MinScore"] ?? "0.5"), "min_score");

                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await httpClient.PostAsync(apiUrl, form);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Lỗi Face Search API: {response.StatusCode}, Nội dung: {responseContent}");
                }

                return responseContent;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}
