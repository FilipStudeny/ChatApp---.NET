using System.Security.Cryptography;

namespace API.Services.Helpers
{
    public interface IPasswordService
    {
        public bool VerifyPasswordHash(string Password, IEnumerable<byte> PasswordHash, byte[] PasswordSalt);
        public void CreatePasswordHash(string Password, out byte[] PasswordHash, out byte[] PasswordSalt);
    }
    public class PasswordService : IPasswordService
    {
        void IPasswordService.CreatePasswordHash(string Password, out byte[] PasswordHash, out byte[] PasswordSalt)
        {
            using var hmac = new HMACSHA512();
            PasswordSalt = hmac.Key;
            PasswordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(Password));
        }

        bool IPasswordService.VerifyPasswordHash(string Password, IEnumerable<byte> PasswordHash, byte[] PasswordSalt)
        {
            using var hmac = new HMACSHA512(PasswordSalt);
            var hash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(Password));
            return hash.SequenceEqual(PasswordHash);
        }
    }
    
}
