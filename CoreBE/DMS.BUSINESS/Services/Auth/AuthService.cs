using Microsoft.EntityFrameworkCore;
using DMS.BUSINESS.Common;
using DMS.BUSINESS.Dtos.AD;
using DMS.BUSINESS.Dtos.Auth;
using DMS.CORE;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using DMS.CORE.Entities.AD;
using Microsoft.Extensions.Configuration;
using AutoMapper;
using Common.Util;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using System.Text.Json;

namespace DMS.BUSINESS.Services.Auth
{
    public interface IAuthService : IGenericService<TblAdAccount, AccountDto>
    {
        Task<JWTTokenDto> Login(LoginDto loginInfo);
        Task<JWTTokenDto> LoginFace(string base64Image); 
        Task<AccountDto> GetAccount(string userName);
        Task ChangePassword(ChangePasswordDto changePasswordDto);
        Task<JWTTokenDto> RefreshToken(RefreshTokenDto refreshTokenDto);
    }

    public class AuthService(AppDbContext dbContext, IMapper mapper, IConfiguration configuration) : GenericService<TblAdAccount, AccountDto>(dbContext, mapper), IAuthService
    {
        private readonly IConfiguration _configuration = configuration;

        public async Task ChangePassword(ChangePasswordDto changePasswordDto)
        {
            var account = await AuthenticationProcess(new LoginDto()
            {
                UserName = changePasswordDto.UserName,
                Password = changePasswordDto.OldPassword
            });

            if (Status)
            {
                var newPasswordHash = Utils.CryptographyMD5(changePasswordDto.NewPassword);
                account.Password = newPasswordHash;
                _dbContext.Update(account);
                await _dbContext.SaveChangesAsync();
            }

            return;
        }

        public async Task<JWTTokenDto> Login(LoginDto loginInfo)
        {
            try
            {
                await _dbContext.Database.BeginTransactionAsync();
                var authUser = await AuthenticationProcess(loginInfo);
                if (Status)
                {
                    var account = _mapper.Map<AccountLoginDto>(authUser);
                    var refreshToken = await GenerateRefreshToken(account.UserName);
                    if (Status)
                    {
                        var token = GeneratenJwtToken(account.UserName, account.FullName);
                        await _dbContext.Database.CommitTransactionAsync();
                        return new()
                        {
                            AccountInfo = account,
                            AccessToken = token.Item1,
                            ExpireDate = token.Item2,
                            RefreshToken = refreshToken.Item1,
                            ExpireDateRefreshToken = refreshToken.Item2,
                        };
                    }
                }
                await _dbContext.Database.CommitTransactionAsync();
                return null;
            }
            catch (Exception ex)
            {
                await _dbContext.Database.RollbackTransactionAsync();
                Status = false;
                Exception = ex;
                return null;
            }
        }

        public async Task<JWTTokenDto> LoginFace(string base64Image)
        {
            try
            {
                var result = await GetUIdFaceFromBase64(base64Image);


                await _dbContext.Database.BeginTransactionAsync();
                var (authUser, user) = await AuthenticationProcessFace(result);
                if (authUser != null)
                {
                    if (Status)
                    {
                        var account = _mapper.Map<AccountLoginDto>(authUser);
                        var refreshToken = await GenerateRefreshToken(account.UserName);
                        if (Status)
                        {
                            var token = GeneratenJwtToken(account.UserName, account.FullName);
                            await _dbContext.Database.CommitTransactionAsync();
                            return new()
                            {
                                AccountInfo = account,
                                AccessToken = token.Item1,
                                ExpireDate = token.Item2,
                                RefreshToken = refreshToken.Item1,
                                ExpireDateRefreshToken = refreshToken.Item2,
                            };
                        }
                    }
                    await _dbContext.Database.CommitTransactionAsync();
                }
                if (user != null)
                {
                    if (Status)
                    {
                        var userdto = _mapper.Map<UserDto>(user);

                        return new()
                        {
                            User = userdto,
                        };
                    }
                    await _dbContext.Database.CommitTransactionAsync();
                }

                return null;

            }
            catch (Exception ex)
            {
                await _dbContext.Database.RollbackTransactionAsync();
                Status = false;
                Exception = ex;
                return null;
            }
        }

        public async Task<string> GetUIdFaceFromBase64(string base64Image)
        {
            if (string.IsNullOrWhiteSpace(base64Image))
                throw new ArgumentException("Dữ liệu ảnh base64 không hợp lệ");

            var token = "cba287859e90fd581d177d499250f6aaf0524b739377a396cfd2684303fff302";

            var base64Data = base64Image.Contains(",") ? base64Image.Split(',')[1] : base64Image;

            byte[] imageBytes;
            try
            {
                imageBytes = Convert.FromBase64String(base64Data);
            }
            catch (FormatException)
            {
                throw new ArgumentException("Chuỗi base64 không hợp lệ");
            }

            using (var httpClient = new HttpClient())
            using (var form = new MultipartFormDataContent())
            {
                var fileContent = new ByteArrayContent(imageBytes);
                fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");

                form.Add(fileContent, "file", "face.jpg");
                form.Add(new StringContent("true"), "anti_spoofing");
                form.Add(new StringContent("0.7"), "threshold_spoofing");
                form.Add(new StringContent("0.8"), "min_score");

                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await httpClient.PostAsync("https://llm.xbot.vn/face/search-face", form);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                    throw new Exception($"Lỗi API: {response.StatusCode}, Nội dung: {responseContent}");

                try
                {
                    using var doc = JsonDocument.Parse(responseContent);
                    var root = doc.RootElement;

                    var data = root.GetProperty("data");
                    if (data.GetArrayLength() == 0)
                        return null!;

                    var firstItem = data[0];

                    if (firstItem.TryGetProperty("is_spoof", out var isSpoofElement) &&
                        isSpoofElement.GetBoolean())
                    {
                       return null!;
                    }

                    var userId = firstItem.GetProperty("user_id").GetString();
                    return userId ?? string.Empty;
                }
                catch (Exception ex)
                {
                    throw new Exception("Không thể phân tích kết quả JSON từ API", ex);
                }
            }
        }


        public async Task<AccountDto> GetAccount(string userName)
        {
            var account = await _dbContext.TblAdAccount.FirstOrDefaultAsync(
                    x => x.UserName == userName);

            return _mapper.Map<AccountDto>(account);
        }

        public async Task<JWTTokenDto> RefreshToken(RefreshTokenDto refreshTokenDto)
        {
            try
            {
                var refreshTokenDb = await _dbContext.TblAdAccountRefreshToken.FirstOrDefaultAsync(x => x.RefreshToken == refreshTokenDto.RefreshToken);

                if (refreshTokenDb is null || refreshTokenDb.ExpireTime <= DateTime.UtcNow)
                {
                    Status = false;
                    MessageObject.Code = "1005";
                    return null;
                }

                var account = await _dbContext.TblAdAccount.FirstOrDefaultAsync(x => x.UserName == refreshTokenDb.UserName);

                var refreshToken = await GenerateRefreshToken(refreshTokenDb.UserName);

                if (Status)
                {
                    var token = GeneratenJwtToken(refreshTokenDb.UserName, account.FullName);
                    return new()
                    {
                        AccountInfo = _mapper.Map<AccountLoginDto>(account),
                        AccessToken = token.Item1,
                        ExpireDate = token.Item2,
                        RefreshToken = refreshToken.Item1,
                        ExpireDateRefreshToken = refreshToken.Item2
                    };
                }

                return null;
            }
            catch (Exception ex)
            {
                Status = false;
                if (ex.GetType() == typeof(SecurityTokenExpiredException))
                {
                    MessageObject.Code = "1004";
                }
                else
                {
                    MessageObject.Code = "1000";
                }
                return null;
            }

        }

        private async Task<TblAdAccount> AuthenticationProcess(LoginDto loginInfo)
        {
            if (string.IsNullOrWhiteSpace(loginInfo.UserName) || string.IsNullOrWhiteSpace(loginInfo.Password))
            {
                Status = false;
                MessageObject.Code = "1001"; 
                return null;
            }

            var account = await _dbContext.TblAdAccount
                .Include(x => x.Account_AccountGroups)
                .ThenInclude(x => x.AccountGroup)
             
                .FirstOrDefaultAsync(
                x => x.UserName.ToLower() == loginInfo.UserName.ToLower() &&
                x.Password == Utils.CryptographyMD5(loginInfo.Password));

            if (account == null)
            {
                Status = false;
                MessageObject.Code = "1002"; //Sai username hoặc mật khẩu
                return null;
            }

            if (!(account?.IsActive ?? true))
            {
                Status = false;
                MessageObject.Code = "1003"; //Tài khoản bị khóa
                return null;
            }

            return account;
        }
        //private async Task<TblAdAccount> AuthenticationProcessFace(string faceId)
        //{
        //    var account = await _dbContext.TblAdAccount
        //        .Include(x => x.Account_AccountGroups)
        //        .ThenInclude(x => x.AccountGroup)
        //        //  .Include(x => x.Partner)
        //        //  .Include(x => x.Driver)
        //        .FirstOrDefaultAsync(
        //        x => x.FaceId == faceId);

        //    if (account == null)
        //    {
        //        Status = false;
        //        MessageObject.Code = "1002"; //Sai username hoặc mật khẩu
        //        return null;
        //    }

        //    if (!(account?.IsActive ?? true))
        //    {
        //        Status = false;
        //        MessageObject.Code = "1003"; //Tài khoản bị khóa
        //        return null;
        //    }

        //    return account;
        //}

        private async Task<(TblAdAccount account, TblUser user)> AuthenticationProcessFace(string faceId)
        {
            var account = await _dbContext.TblAdAccount
                .Include(x => x.Account_AccountGroups)
                .ThenInclude(x => x.AccountGroup)
                .FirstOrDefaultAsync(x => x.FaceId == faceId);

            if (account != null)
            {
                if (!(account?.IsActive ?? true))
                {
                    Status = false;
                    MessageObject.Code = "1003";
                    return (null, null);
                }
                return (account, null);
            }

            var user = await _dbContext.TblAdUser.FirstOrDefaultAsync(x => x.FaceId == faceId);
            if (user == null)
            {
                Status = false;
                MessageObject.Code = "1002";
                return (null, null);
            }

            if (!(user?.IsActive ?? true))
            {
                Status = false;
                MessageObject.Code = "1003";
                return (null, null);
            }

            return (null, user);
        }


        private (string, DateTime) GeneratenJwtToken(string? userName, string fullName)
        {
            var claims = new[] {
                        new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"] ?? string.Empty),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds().ToString()),
                        new Claim(ClaimTypes.Name, userName ?? string.Empty),
                        new Claim(ClaimTypes.GivenName,fullName ),
                    };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? string.Empty));

            var expire = DateTime.UtcNow.AddMinutes(int.Parse(_configuration["Jwt:ExpireToken"] ?? string.Empty));

            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var jwtSecurityToken = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: expire,
                signingCredentials: signIn);

            var token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            return new(token, expire);
        }

        private async Task<(string, DateTime)> GenerateRefreshToken(string username)
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            var refreshToken = Convert.ToBase64String(randomNumber);

            var expire = DateTime.UtcNow.AddDays(int.Parse(_configuration["Jwt:ExpireRefreshToken"] ?? string.Empty));
            var obj = new TblAdAccountRefreshToken()
            {
                UserName = username,
                RefreshToken = refreshToken,
                ExpireTime = expire,
            };

            try
            {
                await _dbContext.TblAdAccountRefreshToken.AddAsync(obj);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Status = false;
                MessageObject.Code = "1000";
                MessageObject.Message = ex.Message;
                return new();
            }

            return new(refreshToken, expire);
        }
    }
}
