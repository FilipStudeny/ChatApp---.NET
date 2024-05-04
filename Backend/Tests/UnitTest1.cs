using API.Database;
using API.Repository;
using API.Services;
using API.Services.Helpers;
using Mongo2Go;
using MongoDB.Bson;
using MongoDB.Driver;
using NSubstitute;
using Shared.Builders;
using Shared.Models;

namespace Tests
{
    public class UnitTest1 : IClassFixture<MongoDbFixture>, IDisposable
    {
        private readonly MongoDbFixture _fixture;

        public UnitTest1(MongoDbFixture fixture)
        {
            _fixture = fixture;
        }
        
        [Fact]
        public async Task GetUsers_ShouldReturnListOfUsers()
        {
            var user = new UserBuilder().Build();
            var user2 = new UserBuilder().Build();
            // Arrange
            var userRepository = Substitute.For<IUserRepository>();
            var authenticationService = Substitute.For<IAuthenticationService>();

            // Start an in-memory MongoDB instance
            var database = _fixture.GetDatabase("ChatApp");
            var collection = database.GetCollection<User>("users");

            // Insert test data
            var testUsers = new List<User> { user, user2 };
            await collection.InsertManyAsync(testUsers);

            // Set up UserRepository to use the in-memory MongoDB instance
            userRepository.GetAllUsers().ReturnsForAnyArgs(testUsers);

            // Create an instance of UserService
            var userService = new UserService(authenticationService, userRepository);

            // Act
            var serviceResponse = await userService.GetUsers();

            // Assert
            Assert.True(serviceResponse.Success);
            Assert.NotNull(serviceResponse.Data);
            Assert.Equal(testUsers.Count(), serviceResponse.Data.Count);
        }

        public void Dispose()
        {
            var mangoMarketDb = _fixture.GetDatabase("ChatApp");
            var collection = mangoMarketDb.GetCollection<User>("Users");
            collection.DeleteManyAsync(_ => true).Wait();
        }
    }
}