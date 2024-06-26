﻿using System.ComponentModel.DataAnnotations;
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
        public Task<ServiceResponse<bool>> AddFriend(ObjectId notificationId, ObjectId newFriendId);
        public Task<ServiceResponse<bool>> SendFriendRequest(NotificationDto notificationDto);
    }

    public class UserService : IUserService
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IUserRepository _userRepository;
        private readonly INotificationsService _notificationsService;

        public UserService(IAuthenticationService authenticationService, IUserRepository userRepository, INotificationsService notificationsService)
        {
            _authenticationService = authenticationService;
            _userRepository = userRepository;
            _notificationsService = notificationsService;
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

        public async Task<ServiceResponse<bool>> AddFriend(ObjectId notificationId, ObjectId newFriendId)
        {
            try
            {
                var userId = _authenticationService.GetUserIdFromToken();
                var userExists = await _userRepository.UserExists(userId);
                if (!userExists)
                {
                    throw new UserNotFoundException(Messages.UserNotFound);
                }

                var newFriendExists = await _userRepository.UserExists(newFriendId);
                if (!newFriendExists)
                {
                    throw new UserNotFoundException(Messages.UserNotFound);
                }

                await _notificationsService.NotificationExists(userId, notificationId);
                
                var user = await _userRepository.GetUser(userId);
                var newFriend = await _userRepository.GetUser(newFriendId);

                var friendToUser = new Friend()
                {
                    Id = newFriend.Id,
                    Username = newFriend.Username
                };

                var userToFriend = new Friend()
                {
                    Id = user.Id,
                    Username = user.Username
                };

                await _userRepository.AddFriend(user.Id, friendToUser);
                await _userRepository.AddFriend(newFriend.Id, userToFriend);
                await _notificationsService.DeleteNotification(newFriend.Id, notificationId);

                return new ServiceResponse<bool>() { Data = true, Message = "Friend request accepted." };
            }
            catch (CustomException ex)
            {
                return new ServiceResponse<bool>
                {
                    Data = false,
                    StatusCode = ex.StatusCode,
                    Success = false,
                    Message = ex.Message
                };
            }        
        }

        public async Task<ServiceResponse<bool>> SendFriendRequest(NotificationDto notificationDto)
        {
            try
            {
                var senderExists = await _userRepository.UserExists(notificationDto.SenderId.Value);
                if (!senderExists)
                {
                    throw new UserNotFoundException("Sender not found");
                }

                var receiverExists = await _userRepository.UserExists(notificationDto.ReceiverId.Value);
                if (!receiverExists)
                {
                    throw new UserNotFoundException("Receiver not found");
                }

                var sender = await _userRepository.GetUser(notificationDto.SenderId.Value);
                var receiver = await _userRepository.GetUser(notificationDto.ReceiverId.Value);

                var notification = new Notification()
                {
                    Sender = sender.Id,
                    Receiver = receiver.Id,
                    Message = Messages.GenerateFriendRequestMessage(sender.Username),
                    CreatedAt = DateTime.Now,
                    NotificationsStruct = receiver.NotificationsStructId,
                    NotificationType = NotificationType.FriendRequest
                };

                var response = await _notificationsService.CreateNotification(notification);
                if (response.Success)
                {
                    return new ServiceResponse<bool>() { Data = true, Message = Messages.FriendRequestSent};
                }

                throw new ServiceException(Messages.CouldNotSendFriendRequest);

            }
            catch (CustomException ex)
            {
                return new ServiceResponse<bool>
                {
                    Data = false,
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
                Data = users.Count == 0 ? null : users,
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

                await _userRepository.CreateUser(newUser);
                var userId = newUser.Id;
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
                    Message = ex.Message,
                    StatusCode = ex.StatusCode
                };
            }
        }
    }
}