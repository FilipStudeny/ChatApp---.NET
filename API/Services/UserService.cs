using API.Database;
using API.Models;
using API.Models.DTOs;

namespace API.Services
{
    public interface IUserService
    {
        public Task<ServiceResponse<bool>> Register();
        public Task<ServiceResponse<bool>> Login();

    }
    public class UserService : IUserService
    {
        private readonly MongoDbContext _database;
        public UserService(MongoDbContext database)
        {
            _database = database;
        }
        Task<ServiceResponse<bool>> IUserService.Login()
        {
            throw new NotImplementedException();
        }

        Task<ServiceResponse<bool>> IUserService.Register()
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceResponse<bool>> CreateNewUser(Register_DTO register_DTO)
        {
            return new ServiceResponse<bool> { Success = true };
        }
    }
}
