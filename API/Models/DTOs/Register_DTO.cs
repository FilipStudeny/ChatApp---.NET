namespace API.Models.DTOs
{
    public class Register_DTO
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string PasswordRepeat { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}
