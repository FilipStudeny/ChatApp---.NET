
using System.Net;
using System.Security.Cryptography;
using System.Text;
using API.Database;
using API.Repository;
using API.Services;
using API.Services.Helpers;
using FluentAssertions;
using NSubstitute;
using Shared.Builders;
using Shared.DTOs;
using Shared.Models;

namespace Tests.UserService;

public class TestsUserServiceRegister(MongoDbFixture fixture) : TestBase(fixture)
{

    [Fact]
    public async Task UserService_Register_WhenUserAlreadyExists_ShouldThrowUserException()
    {
        // ARRANGE
        var user = new UserBuilder().Build();
        var registerDto = new RegisterDto()
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Gender = user.Gender,
            Password = "password",
            PasswordRepeat = "password",
            Username = user.Username,
            ProfilePicture = "picture"
        };
        
        var collection = fixture.DbContext.Users;
        await collection.InsertOneAsync(user);
        var authenticationService = Substitute.For<IAuthenticationService>();
        var notificationService = Substitute.For<INotificationsService>();
        var userRepository = new UserRepository(fixture.DbContext);
        var userService = new API.Services.UserService(authenticationService, userRepository, notificationService);
        
        // ACT
        var response = await userService.Register(registerDto);

        // ASSERT
        response.Data.Should().BeFalse();
        response.Success.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
    }
    
    [Fact]
    public async Task UserService_Register_WhenPasswordsDoNotMatch_ShouldThrowValidationException()
    {
        // ARRANGE
        var user = new UserBuilder().Build();
        var registerDto = new RegisterDto()
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Gender = user.Gender,
            Password = "password",
            PasswordRepeat = "drowssap",
            Username = user.Username,
            ProfilePicture = "picture"
        };
        var authenticationService = Substitute.For<IAuthenticationService>();
        var userRepository = new UserRepository(fixture.DbContext);
        var notificationService = Substitute.For<INotificationsService>();
        var userService = new API.Services.UserService(authenticationService, userRepository, notificationService);
        
        // ACT
        var response = await userService.Register(registerDto);

        // ASSERT
        response.Data.Should().BeFalse();
        response.Success.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
    }
    

    [Fact]
    public async Task UserService_Register_WhenPasswordIsShortThanSixSymbols_ShouldThrowUserException()
    {
        // ARRANGE
        var user = new UserBuilder().Build();
        var registerDto = new RegisterDto()
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Gender = user.Gender,
            Password = "pass",
            PasswordRepeat = "pass",
            Username = user.Username,
            ProfilePicture = "picture"
        };
        var authenticationService = Substitute.For<IAuthenticationService>();
        var notificationService = Substitute.For<INotificationsService>();
        var userRepository = new UserRepository(fixture.DbContext);
        var userService = new API.Services.UserService(authenticationService, userRepository, notificationService);

        // ACT
        var response = await userService.Register(registerDto);
        
        // ASSERT
        response.Data.Should().BeFalse();
        response.Success.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
    }
    
    
    [Fact]
    public async Task UserService_Register_CreateNewAccount_ShouldCreateNewAccount()
    {
        // ARRANGE
        var user = new UserBuilder().WithPassword("colonel").Build();
        var registerDto = new RegisterDto()
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Gender = user.Gender,
            Password = "colonel",
            PasswordRepeat = "colonel",
            Username = user.Username,
            ProfilePicture = "picture"
        };
        
        var authenticationService = Substitute.For<IAuthenticationService>();
        var notificationService = Substitute.For<INotificationsService>();
        var userRepository = new UserRepository(fixture.DbContext);
        var userService = new API.Services.UserService(authenticationService, userRepository, notificationService);
        authenticationService.CreatePasswordHash(registerDto.Password)
            .Returns(callInfo =>
            {
                var password = callInfo.Arg<string>();
                using var hmac = new HMACSHA512();
                var passwordSalt = hmac.Key;
                var passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

                return new PasswordInfo
                {
                    Hash = passwordHash,
                    Salt = passwordSalt
                };
            }); 
        
        // ACT
        var response = await userService.Register(registerDto);
        var userRepositoryResponse = await userRepository.GetUserByEmailOrUsername(user.Email, user.Username);
        
        // ASSERT
        response.Data.Should().BeTrue();
        response.Success.Should().BeTrue();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        userRepositoryResponse.Email.Should().Be(user.Email);
        userRepositoryResponse.Username.Should().Be(user.Username);
    }

}