using DMS.BUSINESS.Dtos.AD;

namespace DMS.BUSINESS.Dtos.Auth
{
    public class JWTTokenDto
    {
        public string AccessToken { get; set; }

        public DateTime ExpireDate { get; set; }

        public string RefreshToken { get; set; }

        public DateTime ExpireDateRefreshToken { get; set; }

        public AccountLoginDto AccountInfo { get; set; }

        public UserDto User { get; set; }
    }
}
