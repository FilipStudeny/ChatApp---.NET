using API.Database;
using API.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Shared.Models;

namespace API.Services
{
    public interface IAuthenticationService
    {
        public string CreateToken(User user);
        public bool VerifyPasswordHash(string Password, byte[] PasswordHash, byte[] PasswordSalt);
        public void CreatePasswordHash(string Password, out byte[] PasswordHash, out byte[] PasswordSalt);
    }

    public class AuthenticationService(IOptions<AppSettings> configuration) : IAuthenticationService
    {
        private readonly AppSettings configuration = configuration.Value;

        public string CreateToken(User user)
        {
            byte[] secret = Encoding.UTF8.GetBytes(configuration.JwtKey);
            SymmetricSecurityKey key = new SymmetricSecurityKey(secret);

            Dictionary<string, object> claims = new Dictionary<string, object>
            {
                [ClaimTypes.NameIdentifier] = user.Id,
                [ClaimTypes.Name] = user.Username,
                [ClaimTypes.Email] = user.Email
            };

            SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor
            {
                Claims = claims,
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature)
            };

            JsonWebTokenHandler handler = new JsonWebTokenHandler();
            handler.SetDefaultTimesOnTokenCreation = false;
            string token = handler.CreateToken(descriptor);
            return token;
        }

        public void CreatePasswordHash(string Password, out byte[] PasswordHash, out byte[] PasswordSalt)
        {
            using var hmac = new HMACSHA512();
            PasswordSalt = hmac.Key;
            PasswordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(Password));
        }


        public bool VerifyPasswordHash(string Password, byte[] PasswordHash, byte[] PasswordSalt)
        {
            using var hmac = new HMACSHA512(PasswordSalt);
            var hash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(Password));
            return hash.SequenceEqual(PasswordHash);
        }

    }
}
