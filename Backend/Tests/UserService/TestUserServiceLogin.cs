using System.Net;
using System.Security.Cryptography;
using System.Text;
using API.Repository;
using API.Services;
using NSubstitute;
using Shared.Builders;
using Shared.DTOs;
using Shared.Models;

namespace Tests.UserService;

public class TestUserServiceLogin(MongoDbFixture fixture) : TestBase(fixture)
{

    [Fact]
    public async Task UserService_Login_WhenAccountIsNotFound_ShouldThrownAnException()
    {
        // ARRANGE
        var user = new UserBuilder().Build();
        var loginDto = new LoginDto()
        {
            Username = user.Username,
            Password = "colonel"
        };

        var userRepository = Substitute.For<IUserRepository>();
        var authenticationService = Substitute.For<IAuthenticationService>();
        var userService = new API.Services.UserService(authenticationService, userRepository);
        userRepository.UserExists(Arg.Any<string>(), Arg.Any<string>()).ReturnsForAnyArgs(false);
        
        // ACT
        var response = await userService.Login(loginDto);
        
        // ASSERT
        Assert.False(response.Success);
        Assert.Equal("Account not found.", response.Message);
        Assert.Null(response.Data);
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task UserService_Login_WhenWrongPasswordWasProvided_ShouldThrownAnException()
    {
        // ARRANGE
        var user = new UserBuilder().WithPassword("colonel").Build();
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
        var loginDto = new LoginDto()
        {
            Username = user.Username,
            Password = "colonel"
        };
        
        var userRepository = Substitute.For<IUserRepository>();
        var authenticationService = Substitute.For<IAuthenticationService>();
        var userService = new API.Services.UserService(authenticationService, userRepository);

        var database = fixture.GetDatabase("ChatApp");
        var collection = database.GetCollection<User>("Users");
        userRepository.UserExists(Arg.Any<string>(), Arg.Any<string>()).ReturnsForAnyArgs(false);
        var s = authenticationService.CreatePasswordHash(registerDto.Password)
            .Returns(_ =>
            {
                var password = registerDto.Password;
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
        await userService.Register(registerDto);
        
        userRepository.GetAllUsers().Returns([user]);
        userRepository.GetUserByEmailOrUsername(Arg.Any<string>(), Arg.Any<string>()).Returns(user);
        userRepository.UserExists(loginDto.Username, loginDto.Username).ReturnsForAnyArgs(false);
        
        // ACT
        var response = await userService.Login(loginDto);
        
        // ASSERT
        Assert.False(response.Success);
        Assert.Equal("Wrong email or password, try again.", response.Message);
        Assert.Null(response.Data);
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
}