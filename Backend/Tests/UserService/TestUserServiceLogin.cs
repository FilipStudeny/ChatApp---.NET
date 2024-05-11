using System.Net;
using System.Security.Cryptography;
using System.Text;
using API.Repository;
using API.Services;
using FluentAssertions;
using NSubstitute;
using Shared.Builders;
using Shared.DTOs;
using Shared.Models;

namespace Tests.UserService;

public class TestUserServiceLogin(MongoDbFixture fixture) : TestBase(fixture)
{

    [Fact]
    public async Task UserService_Login_WhenUserDoesntExist_ShouldReturnException()
    {
        // ARRANGE
        var user = new UserBuilder().WithPassword("colonel").Build();
        var loginDto = new LoginDto()
        {
            Username = user.Username,
            Password = "colonel"
        };
        
        var authenticationService = Substitute.For<IAuthenticationService>();
        var notificationsRepository = Substitute.For<INotificationsRepository>();
        var userRepository = new UserRepository(fixture.DbContext);
        var userService = new API.Services.UserService(authenticationService, userRepository, notificationsRepository);
        
        // ACT
        var response = await userService.Login(loginDto);

        // ASSERT
        response.Data.Should().BeNullOrEmpty();
        response.Success.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    [Fact]
    public async Task UserService_Login_WhenPasswordDoesntMatch_ShouldReturnException()
    {
        // ARRANGE
        var user = new UserBuilder().WithPassword("colonel").Build();
        var loginDto = new LoginDto()
        {
            Username = user.Username,
            Password = "root"
        };
        var collection = fixture.DbContext.Users;
        await collection.InsertOneAsync(user);

        var authenticationService = Substitute.For<IAuthenticationService>();
        var notificationsRepository = Substitute.For<INotificationsRepository>();
        var userRepository = new UserRepository(fixture.DbContext);
        var userService = new API.Services.UserService(authenticationService, userRepository, notificationsRepository);
        authenticationService.VerifyPasswordHash(loginDto.Password, Arg.Any<byte[]>(), Arg.Any<byte[]>())
            .Returns(false);
        
        // ACT
        var response = await userService.Login(loginDto);

        // ASSERT
        response.Data.Should().BeNullOrEmpty();
        response.Success.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task UserService_Login_WhenUserFoundAndChecksPassed_ShouldReturnToken()
    {
        // ARRANGE
        var user = new UserBuilder().WithPassword("colonel").Build();
        var loginDto = new LoginDto()
        {
            Username = user.Username,
            Password = "root"
        };
        var collection = fixture.DbContext.Users;
        await collection.InsertOneAsync(user);

        var authenticationService = Substitute.For<IAuthenticationService>();
        var notificationsRepository = Substitute.For<INotificationsRepository>();
        var userRepository = new UserRepository(fixture.DbContext);
        var userService = new API.Services.UserService(authenticationService, userRepository, notificationsRepository);
        authenticationService.VerifyPasswordHash(loginDto.Password, Arg.Any<byte[]>(), Arg.Any<byte[]>())
            .Returns(true);
        authenticationService.CreateToken(Arg.Any<User>()).Returns("dummy_token");
        
        // ACT
        var response = await userService.Login(loginDto);

        // ASSERT
        response.Data.Should().Be("dummy_token");
        response.Success.Should().BeTrue();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}