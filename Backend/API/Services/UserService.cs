using API.Extensions;
using API.Repository;
using API.Services.Helpers;
using MongoDB.Bson;
using Shared;
using Shared.DTOs;
using Shared.Enums;
using Shared.Models;

namespace API.Services
{
    public interface IUserService
    {
        public Task<ServiceResponse<bool>> Register(RegisterDto registerDto);
        public Task<ServiceResponse<string>> Login(LoginDto loginDto);
        public Task<ServiceResponse<List<User>>> GetUsers();
        public Task<ServiceResponse<User>> GetUser(ObjectId id);
        public Task<ServiceResponse<bool>> AddFriend(NotificationDto notificationDto);
        public Task<ServiceResponse<bool>> SendFriendRequest(NotificationDto notificationDto);
    }

    public class UserService : IUserService
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IUserRepository _userRepository;
        private readonly INotificationsRepository _notificationRepository;

        public UserService(IAuthenticationService authenticationService, IUserRepository userRepository, INotificationsRepository notificationRepository)
        {
            _authenticationService = authenticationService;
            _userRepository = userRepository;
            _notificationRepository = notificationRepository;
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

        public async Task<ServiceResponse<bool>> AddFriend(NotificationDto notificationDto)
        {
            return new ServiceResponse<bool>();
        }

        public async Task<ServiceResponse<bool>> SendFriendRequest(NotificationDto notificationDto)
        {
            try
            {
                var sender = await _userRepository.GetUser(notificationDto.SenderId);
                if (sender == null)
                {
                    throw new UserNotFoundException("Sender account not found.");
                }

                var receiver = await _userRepository.GetUser(notificationDto.ReceiverId);
                if (receiver == null)
                {
                    throw new UserNotFoundException("User not found.");
                }

                var newNotification = new Notification()
                {
                    Sender = sender.Id,
                    Receiver = receiver.Id,
                    CreatedAt = DateTime.Now,
                    Message = $"{sender.Username} has sent you friend request",
                    NotificationType = NotificationType.FriendRequest
                };

                var result =
                    await _notificationRepository.CreateNotification(notificationDto.ReceiverId, newNotification);

                if (!result)
                {
                    throw new ServiceException("Friend request couldn't be created, try again");
                }

                return new ServiceResponse<bool>
                {
                    Data = true,
                    Message = "Friend request send."
                };

            }
            catch (CustomException ex)
            {
                return new ServiceResponse<bool>
                {
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
                    throw new UserNotFoundException("Account not found.");
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
                    throw new DatabaseException("Couldn't create an account, username or email already in use.");
                }

                if (!registerDto.Password.Equals(registerDto.PasswordRepeat))
                {
                    throw new DatabaseException("Passwords do not match, try again.");
                }

                if (registerDto.Password.Length < 6)
                {
                    throw new DatabaseException("Password must be longer than 6 symbols.");
                }

                var password = _authenticationService.CreatePasswordHash(registerDto.Password);
                var newUser = new User
                {
                    Username = registerDto.Username,
                    FirstName = registerDto.FirstName,
                    LastName = registerDto.LastName,
                    Email = registerDto.Email.ToLower(),
                    ProfilePicture = registerDto.ProfilePicture,
                    Gender = registerDto.Gender,
                    Password = password,
                    RegisterDate = DateTime.UtcNow,
                    LastActivity = DateTime.UtcNow
                };

                // Insert the newUser into the database
                await _userRepository.CreateUser(newUser);

                // Now retrieve the newUser's ID after insertion
                var userId = newUser.Id;

                // Use the userId as needed

                var structS = new NotificationsStruct()
                {
                    User = userId,
                };

                return new ServiceResponse<bool> { Data = true, Message = "Account created." };
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