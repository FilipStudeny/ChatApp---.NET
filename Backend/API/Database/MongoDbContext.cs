﻿
using MongoDB.Driver;
using Shared.Models;

namespace API.Database
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(string connectionString, string database )
        {
            var client = new MongoClient(connectionString);
            _database = client.GetDatabase(database);
        }

        public IMongoCollection<User> Users => _database.GetCollection<User>("Users");
        public IMongoCollection<NotificationsStruct> NotificationsStructs => _database.GetCollection<NotificationsStruct>("Notifications");

        public void ClearDatabase()
        {
            Users.DeleteMany(FilterDefinition<User>.Empty);
        }
        public void SeedData()
        {
            var dataSeeder = new SeedData();
            Users.InsertMany(dataSeeder.SeedUsers());
        }
    }
}
