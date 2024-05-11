using API.Database;
using Mongo2Go;
using MongoDB.Driver;

namespace Tests;

public class MongoDbFixture : IDisposable
{
    public MongoDbRunner Runner { get; private set; }
    public MongoClient Client { get; private set; }
    public MongoDbContext DbContext { get; set; }

    public MongoDbFixture()
    {
        Runner = MongoDbRunner.Start();
        Client = new MongoClient(Runner.ConnectionString);
        DbContext = new MongoDbContext(Runner.ConnectionString, "ChatApp");
    }

    public IMongoDatabase GetDatabase(string name)
    {
        return Client.GetDatabase(name);
    }

    public void Dispose()
    {
        Runner.Dispose();
    }
}