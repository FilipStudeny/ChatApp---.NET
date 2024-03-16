using API.Models;
using MongoDB.Driver;

namespace API.Database
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(string connectionString, string database)
        {
            var client = new MongoClient(connectionString);
            _database = client.GetDatabase(database);
        }

        public IMongoCollection<User> Users => _database.GetCollection<User>("Users");
    }
}
