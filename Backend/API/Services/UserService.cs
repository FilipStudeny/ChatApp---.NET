using API.Extensions;
using API.Repository;
using API.Services.Helpers;
using MongoDB.Bson;
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
        public Task<ServiceResponse<User>> GetUser(ObjectId id);
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
        
        public async Task<ServiceResponse<User>> GetUser(ObjectId id)
        {
            try
            {
                var user = await _userRepository.GetUser(id);
                if (user == null)
                {
                    throw new UserNotFoundException("User not found");
                }

                return new ServiceResponse<User>()
                {
                    Data = user
                };
            }
            catch (CustomException ex)
            {
                return new ServiceResponse<User>
                {
                    Data = null,
                    StatusCode = ex.StatusCode,
                    Success = false,
                    Message = ex.Message
                };
            }

        }

        public async Task<ServiceResponse<List<User>>> GetUsers()
        {
            var users = await _userRepository.GetAllUsers();

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
                var user = await _userRepository.GetUserByEmailOrUsername(loginDto.Username, loginDto.Username);
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
                    StatusCode = ex.StatusCode,
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ServiceResponse<bool>> Register(RegisterDto registerDto)
        {
            try
            {
                var userExists = await _userRepository.UserExists(registerDto.Email, registerDto.Username);
                if (userExists)
                {
                    throw new UserException("Couldn't create an account, username or email already in use.");
                }

                if (!registerDto.Password.Equals(registerDto.PasswordRepeat))
                {
                    throw new UserException("Passwords do not match, try again.");
                }

                if (registerDto.Password.Length < 6)
                {
                    throw new UserException("Password must be longer than 6 symbols.");
                }

                _authenticationService.CreatePasswordHash(registerDto.Password, out var passwordHash, out var passwordSalt);
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
                return new ServiceResponse<bool> { Data = true, Message = "Account created" };
            }
            catch (CustomException ex)
            {
                return new ServiceResponse<bool>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }
    }
}