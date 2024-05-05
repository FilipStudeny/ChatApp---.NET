using System.Security.Cryptography;
using Shared.Models;

namespace API.Services.Helpers
{
    public interface IPasswordService
    {
        public bool VerifyPasswordHash(string password, IEnumerable<byte> passwordHash, byte[] passwordSalt);
        public PasswordInfo CreatePasswordHash(string password);
    }
    public abstract class PasswordService : IPasswordService
    {
        public PasswordInfo CreatePasswordHash(string password)
        {
            using var hmac = new HMACSHA512();
            var passwordSalt = hmac.Key;
            var passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

            return new PasswordInfo
            {
                Hash = passwordHash,
                Salt = passwordSalt
            };
        }

        public bool VerifyPasswordHash(string password, IEnumerable<byte> passwordHash, byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512(passwordSalt);
            var hash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            return hash.SequenceEqual(passwordHash);
        }
    }
    
}