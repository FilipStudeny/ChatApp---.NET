using MongoDB.Driver;
using API.Database;
using API.Extensions;
using MongoDB.Bson;
using Shared.Models;

namespace API.Repository
{
    public interface IUserRepository
    {
        public Task<User> GetUser(ObjectId id);

        public Task<User> GetUserByEmailOrUsername(string email, string username);
        public Task<List<User>> GetAllUsers();
        public Task CreateUser(User user);
        public Task<bool> UserExists(ObjectId id);
        public Task<bool> UserExists(string email, string username);
        public Task<bool> UpdateUserData(ObjectId userId, string fieldName, object newValue);
    }

    public class UserRepository(MongoDbContext database) : IUserRepository
    {
        public async Task<User> GetUser(ObjectId id)
        {
            var filter = Builders<User>.Filter.Eq(u => u.Id, id);
            var user = await database.Users.Find(filter).FirstOrDefaultAsync();
            return user;
        }

        public async Task<User> GetUserByEmailOrUsername(string email, string username)
        {
            var filter = Builders<User>.Filter.Or(
                Builders<User>.Filter.Eq(u => u.Username, username),
                Builders<User>.Filter.Eq(u => u.Email, email)
            );

            var user = await database.Users
                .Find(filter)
                .FirstOrDefaultAsync();

            return user;
        }

        public async Task<List<User>> GetAllUsers()
        {
            var users = await database.Users.Find(u => true).ToListAsync();
            return users;
        }

        public async Task CreateUser(User user)
        {
            await database.Users.InsertOneAsync(user);
        }

        public async Task<bool> UserExists(ObjectId id)
        {
            var user = await GetUser(id);
            return user != null;
        }

        public async Task<bool> UserExists(string email, string username)
        {
            var user = await GetUserByEmailOrUsername(email, username);
            return user != null;
        }

        public async Task<bool> UpdateUserData(ObjectId userId, string fieldName, object newValue)
        {
            var filter = Builders<User>.Filter.Eq(u => u.Id, userId);
            var update = Builders<User>.Update.Set(fieldName, newValue);

            var result = await database.Users.UpdateOneAsync(filter, update);

            return result.ModifiedCount > 0;
        }
    }
}