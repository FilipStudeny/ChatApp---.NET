using API.Models.DTOs;
using API.Models;
using MongoDB.Driver;
using API.Database;
using Microsoft.AspNetCore.Identity;

namespace API.Repository
{
    public interface IUserRepository
    {
        public Task<User> GetUser(string value);
        public Task<List<User>> GetAllUsers();
        public Task CreateUser(User user);
        public Task<bool> UserExists(string value);
    }

    public class UserRepository(MongoDbContext database) : IUserRepository
    {
        private readonly MongoDbContext _database = database;

        public async Task<User> GetUser(string value)
        {
            FilterDefinition<User> filter = Builders<User>.Filter.Or(
               Builders<User>.Filter.Eq(u => u.Username, value),
               Builders<User>.Filter.Eq(u => u.Email, value.ToLower())
           );

            User user = await _database.Users
                .Find(filter)
                .FirstOrDefaultAsync();

            return user;
        }
        public async Task<List<User>> GetAllUsers()
        {
            List<User> users = await _database.Users.Find(u => true).ToListAsync();
            return users;
        }

        public async Task CreateUser(User user)
        {
            await _database.Users.InsertOneAsync(user);

        }

        public async Task<bool> UserExists(string value)
        {
            return await GetUser(value) != null;
        }
    }
}
