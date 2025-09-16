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
using DMS.CORE.Entities.CM;
using Minio.DataModel.Args;
using Microsoft.Extensions.Configuration;
using Minio;
using System.Text.Json;

namespace DMS.BUSINESS.Services.AD
{
    public interface IAccountService : IGenericService<TblAdAccount, AccountDto>
    {
        Task<PagedResponseDto> Search(AccountFilter filter);
        Task UpdateInformation(AccountUpdateInformationDto dto);
        Task<IList<AccountDto>> GetAll(AccountFilterLite filter);
        Task<AccountTreeRightDto> GetByIdWithRightTree(object id);
        void ResetPassword(string username);
        Task<string> RegisterFaceAsync(string username);
        Task<string> GetUIdFace(string username);
    }

    public class AccountService : GenericService<TblAdAccount, AccountDto>, IAccountService
    {
        private readonly IHubContext<RefreshServiceHub> _hubContext;
        private readonly IConfiguration _configuration;
        private readonly IMinioClient _minioClient;
        private readonly IUserService _userService;
        public AccountService(
            AppDbContext dbContext,
            IMapper mapper,
            IHubContext<RefreshServiceHub> hubContext,
            IConfiguration configuration,
            IMinioClient minioClient,
             IUserService userService)


            : base(dbContext, mapper)
        {
            _hubContext = hubContext;
            _configuration = configuration;
            _minioClient = minioClient;
        }

        public async Task<PagedResponseDto> Search(AccountFilter filter)
        {
            try
            {
                var query = _dbContext.TblAdAccount
                .Include(x => x.Account_AccountGroups)
                .ThenInclude(x => x.AccountGroup)
                // .Include(x => x.OrganizeCode)
                .AsQueryable();

                if (!string.IsNullOrWhiteSpace(filter.KeyWord))
                {
                    query = query.Where(x =>
                        x.UserName.Contains(filter.KeyWord) ||
                        x.FullName.Contains(filter.KeyWord)
                    );
                }

                if (!string.IsNullOrWhiteSpace(filter.RoleCode))
                {
                    query = query.Where(x => x.Account_AccountGroups.Select(x => x.AccountGroup).Any(x => x.RoleCode == filter.RoleCode));
                }

                if (!string.IsNullOrWhiteSpace(filter.AccountType))
                {
                    query = query.Where(x => x.AccountType == filter.AccountType);
                }

                //if (!string.IsNullOrWhiteSpace(filter.OrganizeCode))
                //{
                //    query = query.Where(x => x.OrganizeCode == filter.OrganizeCode);
                //}

                if (filter.IsActive.HasValue)
                {
                    query = query.Where(x => x.IsActive == filter.IsActive);
                }

                if (filter.GroupId.HasValue)
                {
                    query = query.Where(x => x.Account_AccountGroups.Any(x => x.GroupId == filter.GroupId));
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

        public async Task<IList<AccountDto>> GetAll(AccountFilterLite filter)
        {
            if (filter == null)
            {
                Status = false;
                MessageObject.Code = "0000";
                return null;
            }
            try
            {
                var query = _dbContext.TblAdAccount
                    .Include(x => x.Account_AccountGroups)
                    .ThenInclude(x => x.AccountGroup)
                    .AsQueryable();

                if (!string.IsNullOrWhiteSpace(filter.KeyWord))
                {
                    query = query.Where(x =>
                        x.UserName.Contains(filter.KeyWord) ||
                        x.FullName.Contains(filter.KeyWord)
                    );
                }

                if (!string.IsNullOrWhiteSpace(filter.AccountType))
                {
                    query = query.Where(x => x.AccountType == filter.AccountType);
                }

                if (!string.IsNullOrWhiteSpace(filter.RoleCode))
                {
                    query = query.Where(x => x.Account_AccountGroups.Select(x => x.AccountGroup).Any(x => x.RoleCode == filter.RoleCode));
                }

                if (filter.IsActive.HasValue)
                {
                    query = query.Where(x => x.IsActive == filter.IsActive);
                }

                if (filter.GroupId.HasValue)
                {
                    query = query.Where(x => x.Account_AccountGroups.Any(x => x.GroupId == filter.GroupId));
                }

                if ((filter?.ExceptRoles?.Length ?? 0) > 0)
                {
                    query = query.Where(x => x.Account_AccountGroups.Select(x => x.AccountGroup)
                                    .Any(x => !filter.ExceptRoles.Any(y => y == x.RoleCode)));
                }

                query = query.OrderByDescending(x => x.CreateDate);
                return _mapper.Map<IList<AccountDto>>(await query.ToListAsync());
            }
            catch (Exception ex)
            {
                Status = false;
                Exception = ex;
                return null;
            }
        }

        public async Task<AccountTreeRightDto> GetByIdWithRightTree(object Id)
        {
            var data = await _dbContext.TblAdAccount
                                        .Include(x => x.Account_AccountGroups)
                                        .ThenInclude(x => x.AccountGroup)
                                        .ThenInclude(x => x.ListAccountGroupRight)
                                        .Include(x => x.AccountRights)
                                        .ThenInclude(x => x.Right)
                                        // .Include(x=>x.Partner)
                                        .FirstOrDefaultAsync(x => x.UserName == Id as string);


            // Lấy danh sách tất cả các quyền
            var lstNode = new List<RightDto>();
            var rootNode = new RightDto() { Id = "R", PId = "-R", Name = "Danh sách quyền trong hệ thống" };
            lstNode.Add(rootNode);

            var lstAllRight = await _dbContext.TblAdRight.Where(x => x.Id != "R").OrderBy(x => x.OrderNumber).ToListAsync();

            var lstRightInGroup = data.Account_AccountGroups
                    .Select(x => x.AccountGroup)
                    .SelectMany(x => x.ListAccountGroupRight)
                    .Select(x => x.RightId)
                    .Where(x => x != "R")
                    .ToList();

            var lstRightOutGroup = data.AccountRights;

            if (data.Account_AccountGroups.Count > 0)
            {
                rootNode.IsChecked = true;
            }
            foreach (var right in lstAllRight)
            {
                var node = new RightDto() { Id = right.Id, Name = right.Name, PId = right.PId };
                if (lstRightOutGroup.Where(x => x.IsAdded.HasValue && x.IsAdded.Value).Select(x => x.RightId).Contains(right.Id))
                {
                    node.IsChecked = true;
                }
                else if (lstRightInGroup.Contains(right.Id) && !lstRightOutGroup.Where(x => x.IsRemoved.HasValue && x.IsRemoved.Value).Select(x => x.RightId).Contains(right.Id))
                {
                    node.IsChecked = true;
                }
                lstNode.Add(node);
            }

            var nodeDict = lstNode.ToDictionary(n => n.Id);
            foreach (var item in lstNode)
            {
                if (item.PId == "-R" || !nodeDict.TryGetValue(item.PId, out RightDto parentNode))
                {
                    continue;
                }

                parentNode.Children ??= [];
                parentNode.Children.Add(item);
            }

            var result = _mapper.Map<AccountTreeRightDto>(data);
            result.TreeRight = rootNode;

            return result;
        }

        public override async Task<AccountDto> Add(IDto dto)
        {
            var realDto = dto as AccountCreateDto;
            if (string.IsNullOrEmpty(realDto.UserId))
            {
                realDto.UserId = Guid.NewGuid().ToString();
            }
            if (string.IsNullOrEmpty(realDto.Password))
            {
                realDto.Password = Utils.CryptographyMD5($"{realDto.UserName}@123");
            }
            else
            {
                realDto.Password = Utils.CryptographyMD5(realDto.Password);
            }
            if (!string.IsNullOrEmpty(realDto.ImageBase64))
            {
                await ProcessAccountImageUpload(realDto);
            }

            var accountData = await base.Add(dto);
            var registerResponse = await RegisterFaceAsync(accountData.UserId);
            // Đăng ký faceId
            var getFaceIdResponse = await GetUIdFace(accountData.UserId);
            try
            {
                using (JsonDocument jsonDoc = JsonDocument.Parse(getFaceIdResponse))
                {
                    var dataArray = jsonDoc.RootElement.GetProperty("data");
                    if (dataArray.GetArrayLength() > 0)
                    {
                        var firstFace = dataArray[0];
                        var userFaceId = firstFace.GetProperty("user_id").GetString();

                        var userToUpdate = await _dbContext.TblAdAccount.Where(x => x.UserId == accountData.UserId).FirstOrDefaultAsync();
                        if (userToUpdate != null)
                        {
                            userToUpdate.FaceId = userFaceId;
                            await _dbContext.SaveChangesAsync();
                        }

                        accountData.FaceId = userFaceId;
                    }
                }
                var newUser = new TblUser
                {
                    PKID = accountData.UserId,
                    FullName = realDto.FullName,
                    PhoneNumber = realDto.PhoneNumber,
                    Email = realDto.Email,
                    Address = realDto.Address,
                    UrlImage = realDto.UrlImage,
                    FaceId = accountData.FaceId,
                };

                _dbContext.TblAdUser.Add(newUser);
                _dbContext.TblAdAccountOrg.Add(new TblAdAccountOrg
                {
                    Id = Guid.NewGuid().ToString(),
                    Username = accountData.UserName,
                    CompanyCode = realDto.OrganizeCode
                });
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {

            }
            return accountData;
        }

        private async Task<UserDto> CreateUserRecord(UserCreateDto userDto)
        {
            return await _userService.AddUser(userDto);
        }
        //Method upload ảnh lên MinIo user
        private async Task ProcessAccountImageUpload(AccountCreateDto accountDto)
        {
            var bucket = _configuration["Minio:BucketName"];
            bool found = await _minioClient.BucketExistsAsync(new BucketExistsArgs().WithBucket(bucket));
            if (!found)
            {
                await _minioClient.MakeBucketAsync(new MakeBucketArgs().WithBucket(bucket));
            }

            var base64Data = accountDto.ImageBase64.Contains(",")
                ? accountDto.ImageBase64.Split(',')[1]
                : accountDto.ImageBase64;
            var imageBytes = Convert.FromBase64String(base64Data);
            var fileId = Guid.NewGuid().ToString();
            var now = DateTime.Now;
            var folderPath = $"accounts/avatars/{now:yyyy_MM_dd}";

            // Detect extension từ Base64 header
            var ext = GetExtensionFromBase64(accountDto.ImageBase64);
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

            // Lưu vào T_CM_FILE cho Account
            await _dbContext.TblCmFile.AddAsync(new TblCmFile
            {
                Id = fileId,
                RefrenceFileId = accountDto.UserId, // Reference tới Account
                FileName = $"account_avatar_{accountDto.UserId}.jpg",
                FileType = ext.Replace(".", ""),
                FileSize = imageBytes.Length,
                FilePath = objectName,
                IsAllowDelete = true,
                IsActive = true,
                CreateDate = now,
                IsDeleted = false
            });

            // Set URL cho Account
            var protocol = bool.Parse(_configuration["Minio:UseSSL"] ?? "false") ? "https" : "http";
            accountDto.UrlImage = $"{protocol}://{_configuration["Minio:Endpoint"]}:{_configuration["Minio:Port"]}/{bucket}/{objectName}";
        }

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
        public async Task UpdateInformation(AccountUpdateInformationDto dto)
        {
            await base.Update(dto);
            if (Status)
            {
                var groups = await _dbContext.TblAdAccountGroup.Where(x =>
                    x.RoleCode == Roles.PHONG_KINH_DOANH.ToString() ||
                    x.RoleCode == Roles.BAN_GIAM_DOC.ToString() ||
                    x.RoleCode == Roles.BAN_DIEU_HANH.ToString())
                .Select(x => x.Id.ToString().ToLower()).ToListAsync();
            }
            await _hubContext.Clients.All.SendAsync(SignalRMethod.USER.ToString());
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
                var dt = dto as AccountUpdateDto;
                if (dt.ImageBase64 != null && dt.ImageBase64 != "")
                {
                    dt.UrlImage = SaveBase64ToFile(dt.ImageBase64);
                }
                var model = _mapper.Map<AccountDto>(dto as AccountUpdateDto);

                var rootRightInModel = model?.AccountRights?.Where(x => x.RightId == "R").ToList();

                if (rootRightInModel != null && rootRightInModel.Count != 0)
                {
                    rootRightInModel.ForEach(x =>
                    {
                        model.AccountRights.Remove(x);
                    });
                }

                var accountLite = _mapper.Map<AccountLiteDto>(model);

                var currentObj = await _dbContext.TblAdAccount.Include(x => x.Account_AccountGroups)
                    .ThenInclude(x => x.AccountGroup)
                    .ThenInclude(x => x.ListAccountGroupRight)
                    .Include(x => x.AccountRights).FirstOrDefaultAsync(x => x.UserName == model.UserName);


                var listRightInGroups = currentObj.Account_AccountGroups
                    .Select(x => x.AccountGroup)
                    .SelectMany(x => x.ListAccountGroupRight)
                    .Select(x => x.RightId)
                    .Where(x => x != "R")
                    .ToList(); // list right in current Group of user

                var listRightOutGroup = currentObj.AccountRights.Select(x => x.RightId).ToList();

                _mapper.Map(accountLite, currentObj);

                currentObj.AccountRights ??= new List<TblAdAccountRight>();

                foreach (var item in model.AccountRights)
                {
                    var rightInAccountRight = currentObj.AccountRights.FirstOrDefault(x => x.RightId == item.RightId);

                    if (listRightInGroups.Contains(item.RightId))
                    {
                        if (rightInAccountRight != null)
                        {
                            currentObj.AccountRights.Remove(rightInAccountRight);
                        }
                        else continue;
                    }


                    if (rightInAccountRight != null)
                    {
                        if (rightInAccountRight.IsRemoved.HasValue && rightInAccountRight.IsRemoved.Value)
                        {
                            rightInAccountRight.IsAdded = true;
                            rightInAccountRight.IsRemoved = false;
                        }
                        else continue;
                    }
                    else
                    {
                        currentObj.AccountRights.Add(new TblAdAccountRight()
                        {
                            RightId = item.RightId,
                            IsAdded = true,
                            IsRemoved = false
                        });
                    }
                }

                var listRightInGroupRemove = listRightInGroups.Concat(listRightOutGroup).Where(x => !model.AccountRights.Select(x => x.RightId).Contains(x)).ToList();

                foreach (var item in listRightInGroupRemove)
                {
                    if (listRightInGroups.Contains(item))
                    {
                        var rightInAccountRight = currentObj.AccountRights.FirstOrDefault(x => x.RightId == item);

                        if (rightInAccountRight == null)
                        {
                            currentObj.AccountRights.Add(new TblAdAccountRight()
                            {
                                RightId = item,
                                IsAdded = false,
                                IsRemoved = true
                            });
                        }
                        else
                        {
                            if (rightInAccountRight.IsAdded.HasValue && rightInAccountRight.IsAdded.Value)
                            {
                                rightInAccountRight.IsAdded = false;
                                rightInAccountRight.IsRemoved = true;
                            }
                            else continue;
                        }
                    }
                    else
                    {
                        var rightInAccountRight = currentObj.AccountRights.FirstOrDefault(x => x.RightId == item);
                        if (rightInAccountRight != null)
                        {
                            currentObj.AccountRights.Remove(rightInAccountRight);
                        }
                    }
                }
                await _dbContext.SaveChangesAsync();
                if (this.Status)
                {
                    await _hubContext.Clients.Groups(currentObj.UserName).SendAsync(SignalRMethod.RIGHT.ToString(), currentObj.UserName);
                }
            }
            catch (Exception ex)
            {
                Status = false;
                Exception = ex;
            }
        }

        public async Task<string> RegisterFaceAsync(string userId)
        {
            var user = _dbContext.TblAdAccount.Where(x => x.UserId == userId).FirstOrDefault();

            var userImageFile = await _dbContext.TblCmFile
                .Where(f => f.RefrenceFileId == userId)
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
                form.Add(new StringContent(user.UserId), "user_id");
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


        public async Task<string> GetUIdFace(string userId)
        {
            try
            {
                var user = _dbContext.TblAdAccount.Where(x => x.UserId == userId).FirstOrDefault();
                if (user == null)
                    throw new ArgumentException($"Không tìm thấy user với PKID: {userId}");

                // Lấy file ảnh mới nhất của user từ database
                var userImageFile = await _dbContext.TblCmFile
                    .Where(f => f.RefrenceFileId == userId)
                    .OrderByDescending(f => f.CreateDate)
                    .FirstOrDefaultAsync();

                if (userImageFile == null)
                    throw new FileNotFoundException($"Không tìm thấy ảnh của user {userId} trong database");

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
                form.Add(new StringContent(_configuration["FaceApi:MinScore"] ?? "0.8"), "min_score");

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


        //      public async Task<string> RegisterFaceAsync(string username)
        //      {
        //          var user = _dbContext.TblAdAccount.Find(username);

        //          string fullImagePath = Path.GetFullPath(
        //    Path.Combine(AppDomain.CurrentDomain.BaseDirectory, user.UrlImage.TrimStart('/', '\\'))
        //);
        //          if (!File.Exists(fullImagePath))
        //              throw new FileNotFoundException("Không tìm thấy ảnh: " + fullImagePath);

        //          var token = "cba287859e90fd581d177d499250f6aaf0524b739377a396cfd2684303fff302";
        //          var fileName = Path.GetFileName(fullImagePath);

        //          using (var httpClient = new HttpClient())
        //          using (var form = new MultipartFormDataContent())
        //          {
        //              var fileBytes = await File.ReadAllBytesAsync(fullImagePath);
        //              var fileContent = new ByteArrayContent(fileBytes);
        //              fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");

        //              form.Add(fileContent, "file", fileName);
        //              form.Add(new StringContent(user.FaceId), "user_id");
        //              form.Add(new StringContent("true"), "anti_spoofing");
        //              form.Add(new StringContent("0.7"), "threshold_spoofing");

        //              httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        //              var response = await httpClient.PostAsync("http://sso.d2s.com.vn:8559/register-face", form);
        //              var responseContent = await response.Content.ReadAsStringAsync();

        //              if (!response.IsSuccessStatusCode)
        //                  throw new Exception($"Lỗi API: {response.StatusCode}, Nội dung: {responseContent}");

        //              return responseContent;
        //          }
        //      }

        //public async Task<string> GetUIdFace(string username)
        //{
        //    var user = _dbContext.TblAdAccount.Find(username);

        //    string fullImagePath = Path.GetFullPath(
        //    Path.Combine(AppDomain.CurrentDomain.BaseDirectory, user.UrlImage.TrimStart('/', '\\'))
        //                            );
        //    if (!File.Exists(fullImagePath))
        //        throw new FileNotFoundException("Không tìm thấy ảnh: " + fullImagePath);

        //    var token = "cba287859e90fd581d177d499250f6aaf0524b739377a396cfd2684303fff302";
        //    var fileName = Path.GetFileName(fullImagePath);

        //    using (var httpClient = new HttpClient())
        //    using (var form = new MultipartFormDataContent())
        //    {
        //        var fileBytes = await File.ReadAllBytesAsync(fullImagePath);
        //        var fileContent = new ByteArrayContent(fileBytes);
        //        fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");

        //        form.Add(fileContent, "file", fileName);
        //        form.Add(new StringContent("true"), "anti_spoofing");
        //        form.Add(new StringContent("0.7"), "threshold_spoofing");
        //        form.Add(new StringContent("0.5"), "min_score");

        //        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        //        var response = await httpClient.PostAsync("http://sso.d2s.com.vn:8559/search-face", form);
        //        var responseContent = await response.Content.ReadAsStringAsync();

        //        if (!response.IsSuccessStatusCode)
        //            throw new Exception($"Lỗi API: {response.StatusCode}, Nội dung: {responseContent}");

        //        return responseContent;
        //    }
        //}
        public void ResetPassword(string username)
        {
            try
            {
                var user = _dbContext.TblAdAccount.Find(username);
                user.Password = Utils.CryptographyMD5($"{username}@123");
                _dbContext.TblAdAccount.Update(user);
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                Status = false;
                Exception = ex;
            }
        }
    }
}