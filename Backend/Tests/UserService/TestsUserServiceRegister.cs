
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
    private readonly MongoDbFixture _fixture = fixture;

    [Fact]
    public async Task UserService_Register_WhenUserAlreadyExists_ShouldThrowUserException()
    {
        // ARRANGE
        var user = new UserBuilder().WithPassword("colonel").Build();

        var collection = _fixture.DbContext.Users;
        await collection.InsertOneAsync(user);

        var userRepository = new UserRepository(fixture.DbContext);
        var s = await userRepository.GetAllUsers();
        // ACT

        s.Should().NotBeNull();
        // ASSERT


        /*
        // ARRANGE
        var user = new UserBuilder().Build();
        var registerDto = new RegisterDto
        {
            Username = user.Username,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            ProfilePicture = user.ProfilePicture,
            Gender = user.Gender,
            Password = "colonel",
            PasswordRepeat = "colonel"
        };

        var userRepository = Substitute.For<IUserRepository>();
        var authenticationService = Substitute.For<IAuthenticationService>();
        var notificationRepository = Substitute.For<INotificationsRepository>();
        var userService = new API.Services.UserService(authenticationService, userRepository, notificationRepository);

        var database = _fixture.GetDatabase("ChatApp");
        var collection = database.GetCollection<User>("Users");
        await collection.InsertOneAsync(user);
        userRepository.UserExists(Arg.Any<string>(), Arg.Any<string>()).ReturnsForAnyArgs(true);

        // ACT
        var response = await userService.Register(registerDto);

        // ASSERT
        Assert.False(response.Success);
        Assert.Equal("Couldn't create an account, username or email already in use.", response.Message);
        Assert.False(response.Data);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        */
    }

    [Fact]
    public async Task UserService_Register_WhenPasswordDoNotMatch_ShouldThrowUserException()
    {
        // ARRANGE
        var user = new UserBuilder().Build();
        var registerDto = new RegisterDto
        {
            Username = user.Username,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            ProfilePicture = user.ProfilePicture,
            Gender = user.Gender,
            Password = "colonel",
            PasswordRepeat = "admin"
        };
        
        var userRepository = Substitute.For<IUserRepository>();
        var authenticationService = Substitute.For<IAuthenticationService>();
        var notificationRepository = Substitute.For<INotificationsRepository>();
        var userService = new API.Services.UserService(authenticationService, userRepository, notificationRepository);
        userRepository.UserExists(Arg.Any<string>(), Arg.Any<string>()).Returns(false);
        
        // ACT
        var response = await userService.Register(registerDto);
        
        // ASSERT
        Assert.False(response.Success);
        Assert.Equal("Passwords do not match, try again.", response.Message);
        Assert.False(response.Data);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task UserService_Register_WhenPasswordIsShortThanSixSymbols_ShouldThrowUserException()
    {
        // ARRANGE
        var user = new UserBuilder().Build();
        var registerDto = new RegisterDto
        {
            Username = user.Username,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            ProfilePicture = user.ProfilePicture,
            Gender = user.Gender,
            Password = "root",
            PasswordRepeat = "root"
        };
        
        var userRepository = Substitute.For<IUserRepository>();
        var authenticationService = Substitute.For<IAuthenticationService>();
        var notificationRepository = Substitute.For<INotificationsRepository>();
        var userService = new API.Services.UserService(authenticationService, userRepository, notificationRepository);
        userRepository.UserExists(Arg.Any<string>(), Arg.Any<string>()).Returns(false);
        
        // ACT
        var response = await userService.Register(registerDto);
        
        // ASSERT
        Assert.False(response.Success);
        Assert.Equal("Password must be longer than 6 symbols.", response.Message);
        Assert.False(response.Data);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
    
    
    [Fact]
    public async Task UserService_Register_WhenRegisterDataIsProvided_ShouldCreateNewAccount()
    {
        var userRepositroy = new UserRepository(fixture.DbContext);

        var ss = new User()
        {
            FirstName = "asda",
            LastName = "user.LastName",
            Email = "Asda"
        };
        userRepositroy.CreateUser(ss);
        var s = ss.Id;
        
        // ARRANGE
        var user = new UserBuilder().Build();
        var registerDto = new RegisterDto
        {
            Username = user.Username,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            ProfilePicture = user.ProfilePicture,
            Gender = user.Gender,
            Password = "colonel",
            PasswordRepeat = "colonel"
        };
        
        var userRepository = Substitute.For<IUserRepository>();
        var authenticationService = Substitute.For<IAuthenticationService>();
        var notificationRepository = Substitute.For<INotificationsRepository>();
        var userService = new API.Services.UserService(authenticationService, userRepository, notificationRepository);
        userRepository.UserExists(Arg.Any<string>(), Arg.Any<string>()).Returns(false);
        userRepository.CreateUser(user).Returns(Task<User>.FromResult);
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

       
        // ASSERT
        Assert.True(response.Success);
        Assert.Equal("Account created.", response.Message);
        Assert.True(response.Data);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

}