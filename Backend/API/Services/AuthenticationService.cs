using API.Database;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using API.Services.Helpers;
using MongoDB.Bson;
using Shared.Models;

namespace API.Services
{
    public interface IAuthenticationService : IPasswordService
    {
        public string CreateToken(User user);

        public ObjectId GetUserIdFromToken();
        public string GetUserEmailFromToken();
    }

    public class AuthenticationService(IOptions<AppSettings> configuration, IHttpContextAccessor httpContextAccessor) : PasswordService, IAuthenticationService
    {
        private readonly AppSettings _configuration = configuration.Value;

        public string CreateToken(User user)
        {
            var secret = Encoding.UTF8.GetBytes(_configuration.JwtKey);
            var key = new SymmetricSecurityKey(secret);

            var claims = new Dictionary<string, object>
            {
                [ClaimTypes.NameIdentifier] = user.Id.ToString(),
                [ClaimTypes.Name] = user.Username,
                [ClaimTypes.Email] = user.Email
            };

            var descriptor = new SecurityTokenDescriptor
            {
                Claims = claims,
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature)
            };

            var handler = new JsonWebTokenHandler
            {
                SetDefaultTimesOnTokenCreation = false
            };
            var token = handler.CreateToken(descriptor);
            return token;
        }

        public ObjectId GetUserIdFromToken()
        {
            var id = httpContextAccessor?.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            return ObjectId.Parse(id!);
        }

        public string GetUserEmailFromToken()
        {
            var email = httpContextAccessor?.HttpContext?.User.FindFirstValue(ClaimTypes.Email);
            return email!;
        }
    }
}