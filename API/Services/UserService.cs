using API.Database;
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
        private readonly MongoDbContext _database;
        private readonly IAuthenticationService authenticationService;
        private readonly IUserRepository userRepository;

        public UserService(MongoDbContext database, IAuthenticationService authenticationService, IUserRepository userRepository)
        {
            _database = database;
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

            User user = await userRepository.GetUser(login_DTO.Username);
            if (user == null)
            {
                return new ServiceResponse<string>
                {
                    Success = false,
                    Message = "ERROR: User not found"
                };
            }

            if(!authenticationService.VerifyPasswordHash(login_DTO.Password, user.Password.Hash, user.Password.Salt))
            {
                return new ServiceResponse<string>
                {
                    Success = false,
                    Message = "ERROR: Wrong password or email address"
                };
            }

            string token = authenticationService.CreateToken(user);
            return new ServiceResponse<string> { Data = token };
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
