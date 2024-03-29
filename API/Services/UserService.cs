﻿using API.Database;
using API.Extensions;
using API.Models;
using API.Models.DTOs;
using API.Repository;
using MongoDB.Driver;

namespace API.Services
{
    public interface IUserService
    {
        public Task<ServiceResponse<bool>> Register(Register_DTO register_DTO);
        public Task<ServiceResponse<string>> Login(Login_DTO login_DTO);

        public Task<ServiceResponse<List<User>>> GetUsers();


    }
    public class UserService : IUserService
    {
        private readonly IAuthenticationService authenticationService;
        private readonly IUserRepository userRepository;

        public UserService(IAuthenticationService authenticationService, IUserRepository userRepository)
        {
            this.authenticationService = authenticationService;
            this.userRepository = userRepository;
        }
        public async Task<ServiceResponse<List<User>>> GetUsers()
        {
            List<User> users = await userRepository.GetAllUsers();

            return new ServiceResponse<List<User>>
            {
                Data = users,
                Success = (users != null)
            };
        }
        public async Task<ServiceResponse<string>> Login(Login_DTO login_DTO)
        {
            try
            {
                User user = await userRepository.GetUser(login_DTO.Username);
                if (user == null)
                {
                    throw new UserNotFoundException("User not found");
                }

                if (!authenticationService.VerifyPasswordHash(login_DTO.Password, user.Password.Hash, user.Password.Salt))
                {
                    throw new AuthenticationFailedException("Wrong email or password, try again.");
                }

                string token = authenticationService.CreateToken(user);
                return new ServiceResponse<string> { Data = token };
            }
            catch (UserNotFoundException ex)
            {
                return new ServiceResponse<string>
                {
                    Success = false,
                    Message = ex.Message
                };
            }catch(AuthenticationFailedException ex)
            {
                return new ServiceResponse<string>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
           
        }

        public async Task<ServiceResponse<bool>> Register(Register_DTO register_DTO)
        {
            bool userExists = await userRepository.UserExists(register_DTO.Email);
            if(userExists)
            {
                return new ServiceResponse<bool>
                {
                    Data = false,
                    Success = false,
                    Message = "Error registering user, user already exists"
                };
            }

            if (!register_DTO.Password.Equals(register_DTO.PasswordRepeat))
            {
                return new ServiceResponse<bool>
                {
                    Data = false,
                    Success = false,
                    Message = "Error: Passwords do not match"
                };
            }

            authenticationService.CreatePasswordHash(register_DTO.Password, out byte[] PasswordHash, out byte[] PasswordSalt);
            User newUser = new User
            {
                Username = register_DTO.Username,
                Email = register_DTO.Email.ToLower(),
                Password = new PasswordInfo
                {
                    Hash = PasswordHash,
                    Salt = PasswordSalt
                },
                RegisterDate = DateTime.UtcNow,
                LastActivity = DateTime.UtcNow
            };

            await userRepository.CreateUser(newUser);
            return new ServiceResponse<bool> { Data = true, Message = "New user registered" };
           
        }

    }
}
