
using System.Net;
using API.Repository;
using API.Services;
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
        var userService = new API.Services.UserService(authenticationService, userRepository);

        var database = fixture.GetDatabase("ChatApp");
        var collection = database.GetCollection<User>("Users");
        collection.InsertOne(user);
        userRepository.UserExists(Arg.Any<string>(), Arg.Any<string>()).ReturnsForAnyArgs(true);
        
        // ACT
        var response = await userService.Register(registerDto);
        
        // ASSERT
        Assert.False(response.Success);
        Assert.Equal("Couldn't create an account, username or email already in use.", response.Message);
        Assert.False(response.Data);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
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
        var userService = new API.Services.UserService(authenticationService, userRepository);
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
        var userService = new API.Services.UserService(authenticationService, userRepository);
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
        var userService = new API.Services.UserService(authenticationService, userRepository);
        userRepository.UserExists(Arg.Any<string>(), Arg.Any<string>()).Returns(false);
        
        // ACT
        var response = await userService.Register(registerDto);
        
        // ASSERT
        Assert.True(response.Success);
        Assert.Equal("Account created.", response.Message);
        Assert.True(response.Data);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

}