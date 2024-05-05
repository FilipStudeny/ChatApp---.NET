using MongoDB.Driver;

namespace Tests;

public class TestBase(MongoDbFixture fixture) : IClassFixture<MongoDbFixture>, IDisposable
{
    public void Dispose()
    {
        var client = fixture.Client;
        const string databaseName = "ChatApp";
        var database = client.GetDatabase(databaseName);
        
        // Delete all collections
        var collections = database.ListCollectionNames().ToList();
        foreach (var collectionName in collections)
        {
            database.DropCollection(collectionName);
        }

        // Delete the entire database
        client.DropDatabase(databaseName);
    }
}