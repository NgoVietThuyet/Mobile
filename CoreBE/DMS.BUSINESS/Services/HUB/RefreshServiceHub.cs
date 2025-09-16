using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace DMS.BUSINESS.Services.HUB
{
    public class RefreshServiceHub : Hub
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly List<Claim> _Claims;

        public RefreshServiceHub(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
            try
            {
                var token = _contextAccessor?.HttpContext?.Request?.Query["access_token"];
                if (!string.IsNullOrWhiteSpace(token))
                {
                    JwtSecurityTokenHandler tokenHandler = new();
                    JwtSecurityToken securityToken = (JwtSecurityToken)tokenHandler.ReadToken(token);
                    _Claims = securityToken.Claims.ToList();
                }
            }
            catch (Exception)
            {
                _Claims = [];
            }
        }

        private string UserName
        {
            get
            {
                try
                {
                    return _Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;

                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        protected string FullName
        {
            get
            {
                try
                {
                    return _Claims.FirstOrDefault(x => x.Type == ClaimTypes.GivenName)?.Value;
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        public override Task OnConnectedAsync()
        {
            if (!string.IsNullOrEmpty(UserName))
            {
                Groups.AddToGroupAsync(Context.ConnectionId, UserName);
            }
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            if (!string.IsNullOrEmpty(UserName))
            {
                Groups.RemoveFromGroupAsync(Context.ConnectionId, UserName);
            }
            return base.OnDisconnectedAsync(exception);
        }
    }

}
