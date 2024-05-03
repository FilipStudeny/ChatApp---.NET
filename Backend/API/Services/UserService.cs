using API.Extensions;
using API.Models.DTOs;
using API.Repository;
using Shared;
using Shared.DTOs;
using Shared.Models;

namespace API.Services
{
    public interface IUserService
    {
        public Task<ServiceResponse<bool>> Register(RegisterDto registerDto);
        public Task<ServiceResponse<string>> Login(LoginDto loginDto);
        public Task<ServiceResponse<List<User>>> GetUsers();
    }

    public class UserService : IUserService
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IUserRepository _userRepository;

        public UserService(IAuthenticationService authenticationService, IUserRepository userRepository)
        {
            _authenticationService = authenticationService;
            _userRepository = userRepository;
        }

        public async Task<ServiceResponse<List<User>>> GetUsers()
        {
            List<User> users = await _userRepository.GetAllUsers();

            return new ServiceResponse<List<User>>
            {
                Data = users,
                Success = (true)
            };
        }
        
        public async Task<ServiceResponse<string>> Login(LoginDto loginDto)
        {
            try
            {
                var user = await _userRepository.GetUser(loginDto.Username);
                if (user == null)
                {
                    throw new UserNotFoundException("User not found");
                }

                if (!_authenticationService.VerifyPasswordHash(loginDto.Password, user.Password.Hash,
                        user.Password.Salt))
                {
                    throw new AuthenticationFailedException("Wrong email or password, try again.");
                }

                var token = _authenticationService.CreateToken(user);
                return new ServiceResponse<string> { Data = token };
            }
            catch (CustomException ex)
            {
                return new ServiceResponse<string>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ServiceResponse<bool>> Register(RegisterDto registerDto)
        {
            try
            {

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            var userExists = await _userRepository.UserExists(registerDto.Email);
            if (userExists)
            {
                return new ServiceResponse<bool>
                {
                    Data = false,
                    Success = false,
                    Message = "Error registering user, user already exists."
                };
            }

            if (!registerDto.Password.Equals(registerDto.PasswordRepeat))
            {
                return new ServiceResponse<bool>
                {
                    Data = false,
                    Success = false,
                    Message = "Passwords do not match, try again."
                };
            }

            _authenticationService.CreatePasswordHash(registerDto.Password, out byte[] passwordHash,
                out byte[] passwordSalt);
            var newUser = new User
            {
                Username = registerDto.Username,
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                Email = registerDto.Email.ToLower(),
                ProfilePicture = registerDto.ProfilePicture,
                Gender = registerDto.Gender,
                Password = new PasswordInfo
                {
                    Hash = passwordHash,
                    Salt = passwordSalt
                },
                RegisterDate = DateTime.UtcNow,
                LastActivity = DateTime.UtcNow
            };

            await _userRepository.CreateUser(newUser);
            return new ServiceResponse<bool> { Data = true, Message = "New account created" };
        }
    }
}