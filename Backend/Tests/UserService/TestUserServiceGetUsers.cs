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
    public async Task UserService_GetUser_WhenUserDoesntExists_ShouldReturnException()
    {
        // ARRANGE
        var user = new UserBuilder().Build();
        var authenticationService = Substitute.For<IAuthenticationService>();
        var notificationsRepository = Substitute.For<INotificationsRepository>();
        var userRepository = new UserRepository(fixture.DbContext);
        var userService = new API.Services.UserService(authenticationService, userRepository, notificationsRepository);
        
        // ACT
        var response = await userService.GetUser(user.Id);
        
        // ASSERT
        response.Data.Should().BeNull();
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        response.Success.Should().BeFalse();
    }
    
    [Fact]
    public async Task UserService_GetUser_WhenUserExists_ShouldReturnUser()
    {
        // ARRANGE
        var user = new UserBuilder().Build();
        var collection = fixture.DbContext.Users;
        await collection.InsertOneAsync(user);
        
        var authenticationService = Substitute.For<IAuthenticationService>();
        var notificationsRepository = Substitute.For<INotificationsRepository>();
        var userRepository = new UserRepository(fixture.DbContext);
        var userService = new API.Services.UserService(authenticationService, userRepository, notificationsRepository);
        
        // ACT
        var response = await userService.GetUser(user.Id);
        
        // ASSERT
        response.Data.Should().NotBeNull();
        response.Data.Id.Should().Be(user.Id);
        response.Data.FirstName.Should().Be(user.FirstName);
        response.Data.LastName.Should().Be(user.LastName);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Success.Should().BeTrue();
    }

    [Fact]
    public async Task UserService_GetUsers_WhenUsersDontExist_ShouldReturnEmptyList()
    {
        // ARRANGE
        var authenticationService = Substitute.For<IAuthenticationService>();
        var notificationsRepository = Substitute.For<INotificationsRepository>();
        var userRepository = new UserRepository(fixture.DbContext);
        var userService = new API.Services.UserService(authenticationService, userRepository, notificationsRepository);

        // ACT
        var response = await userService.GetUsers();
        
        // ASSERT
        response.Data.Should().BeNull();
    }
   
    [Fact]
    public async Task UserService_GetUsers_WhenUsersExist_ShouldReturListOfUsers()
    {
        // ARRANGE
        var user = new UserBuilder().Build();
        var user2 = new UserBuilder().Build();
        var collection = fixture.DbContext.Users;
        await collection.InsertManyAsync([user, user2]);
        
        var authenticationService = Substitute.For<IAuthenticationService>();
        var notificationsRepository = Substitute.For<INotificationsRepository>();
        var userRepository = new UserRepository(fixture.DbContext);
        var userService = new API.Services.UserService(authenticationService, userRepository, notificationsRepository);

        // ACT
        var response = await userService.GetUsers();
        
        // ASSERT
        response.Data.Should().NotBeNull();
        response.Success.Should().BeTrue();
        response.Data.First().Id.Should().Be(user.Id);
        response.Data.First().FirstName.Should().Be(user.FirstName);
        response.Data[1].Id.Should().Be(user2.Id);
        response.Data[1].FirstName.Should().Be(user2.FirstName);
    }
}