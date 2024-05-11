using System.Net;
using API.Repository;
using API.Services;
using FluentAssertions;
using NSubstitute;
using Shared.Builders;
using Shared.Models;

namespace Tests.UserService;

public class TestUserServiceGetUsers(MongoDbFixture fixture) : TestBase(fixture)
{
    [Fact]
    public async Task UserService_GetUsers_RetrieveAllUsers_ShouldReturnListOfUsers()
    {
        // ARRANGE
        var user = new UserBuilder().WithPassword("colonel").Build();
        var user2 = new UserBuilder().WithPassword("colonel").Build();
        var user3 = new UserBuilder().WithPassword("colonel").Build();
        var user4 = new UserBuilder().WithPassword("colonel").Build();

        var database = fixture.GetDatabase("ChatApp");
        var collection = database.GetCollection<User>("Users");
        await collection.InsertManyAsync(new []{user, user2, user3, user4}); 

        var userRepository = Substitute.For<IUserRepository>();
        var authenticationService = Substitute.For<IAuthenticationService>();
        var notificationRepository = Substitute.For<INotificationsRepository>();
        userRepository.GetAllUsers().Returns(new List<User>{user, user2, user3, user4});
        var userService = new API.Services.UserService(authenticationService, userRepository, notificationRepository);

        // ACT
        var response = await userService.GetUsers();

        // ASSERT
        response.Should().NotBeNull();
        response.Data.Should().HaveCount(4);
        response.Data?.First().Id.Should().Be(user.Id);
        response.Data?[1].Id.Should().Be(user2.Id);
        response.Data?[2].Id.Should().Be(user3.Id);
        response.Data?[3].Id.Should().Be(user4.Id);
        response.Success.Should().BeTrue();
    }

    [Fact]
    public async Task UserService_GetUser_WhenUserDoesntExist_ShouldReturnException()
    {
        // ARRANGE
        var user = new UserBuilder().WithFirstName("Pablo").WithLastName("Olbap").WithPassword("colonel").Build();
        var user2 = new UserBuilder().WithPassword("colonel").Build();
        
        var database = fixture.GetDatabase("ChatApp");
        var collection = database.GetCollection<User>("Users");
        await collection.InsertOneAsync(user2); 
        
        var userRepository = Substitute.For<IUserRepository>();
        var authenticationService = Substitute.For<IAuthenticationService>();
        var notificationRepository = Substitute.For<INotificationsRepository>();
        userRepository.GetUser(user.Id)!.Returns<Task<User?>>(Task.FromResult<User?>(null));
        var userService = new API.Services.UserService(authenticationService, userRepository, notificationRepository);
        
        // ACT
        var response = await userService.GetUser(user.Id);

        // ASSERT
        response.Data.Should().BeNull();
        response.Success.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    [Fact]
    public async Task UserService_GetUser_WhenUserExist_ShouldReturnUser()
    {
        // ARRANGE
        var user = new UserBuilder().WithFirstName("Pablo").WithLastName("Olbap").WithPassword("colonel").Build();
        var user2 = new UserBuilder().WithPassword("colonel").Build();
        
        var database = fixture.GetDatabase("ChatApp");
        var collection = database.GetCollection<User>("Users");
        await collection.InsertManyAsync([user, user2]); 
        
        var userRepository = Substitute.For<IUserRepository>();
        var authenticationService = Substitute.For<IAuthenticationService>();
        var notificationRepository = Substitute.For<INotificationsRepository>();
        userRepository.GetUser(user.Id)!.Returns<Task<User?>>(Task.FromResult<User?>(user));
        var userService = new API.Services.UserService(authenticationService, userRepository, notificationRepository);
        
        // ACT
        var response = await userService.GetUser(user.Id);

        // ASSERT
        response.Data.Should().NotBeNull();
        response.Data?.Id.Should().Be(user.Id);
        response.Data?.FirstName.Should().Be(user.FirstName);
        response.Data?.LastName.Should().Be(user.LastName);
        response.Success.Should().BeTrue();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}