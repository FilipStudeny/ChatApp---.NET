using MongoDB.Driver;
using API.Database;
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

    }

    public class UserRepository(MongoDbContext database) : IUserRepository
    {
        private readonly MongoDbContext _database = database;

        public async Task<User> GetUser(ObjectId id)
        {
            var filter = Builders<User>.Filter.Eq(u => u.Id, id);
            var user = await _database.Users.Find(filter).FirstOrDefaultAsync();
            return user;
        }

        public async Task<User> GetUserByEmailOrUsername(string email, string username)
        {
            var filter = Builders<User>.Filter.Or(
               Builders<User>.Filter.Eq(u => u.Username, email),
               Builders<User>.Filter.Eq(u => u.Email, username)
           );

            var user = await _database.Users
                .Find(filter)
                .FirstOrDefaultAsync();

            return user;
        }
        public async Task<List<User>> GetAllUsers()
        {
            var users = await _database.Users.Find(u => true).ToListAsync();
            return users;
        }

        public async Task CreateUser(User user)
        {
            await _database.Users.InsertOneAsync(user);
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
    }
}
