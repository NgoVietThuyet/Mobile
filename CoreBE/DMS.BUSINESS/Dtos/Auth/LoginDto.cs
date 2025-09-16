namespace DMS.BUSINESS.Dtos.Auth
{
    public class LoginDto
    {
        public string UserName { get; set; }

        public string Password { get; set; }
    }
    public class FaceLoginRequest
    {
        public string ImageBase64 { get; set; }
    }
}
