using System.Security.Cryptography;

namespace API.Services.Helpers
{
    public interface IPasswordService
    {
        public bool VerifyPasswordHash(string password, IEnumerable<byte> passwordHash, byte[] passwordSalt);
        public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt);
    }
    public abstract class PasswordService : IPasswordService
    {
        void IPasswordService.CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512();
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        }

        bool IPasswordService.VerifyPasswordHash(string password, IEnumerable<byte> passwordHash, byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512(passwordSalt);
            var hash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            return hash.SequenceEqual(passwordHash);
        }
    }
    
}
