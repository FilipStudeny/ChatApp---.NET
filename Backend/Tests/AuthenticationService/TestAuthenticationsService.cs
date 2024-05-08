using System.IdentityModel.Tokens.Jwt;
using System.Text;
using API.Database;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NSubstitute;
using Shared.Builders;
using System.Security.Claims;
using System.Text.Json;


namespace Tests.AuthenticationService;

public class TestAuthenticationsService(MongoDbFixture fixture) : TestBase(fixture)
{
    [Fact]
    public async Task AuthenticationService_CreateToken_WhenUserDataProvided_ShouldReturnToken()
    {
        // ARRANGE
        var user = new UserBuilder().Build();
        const string key = "super-secret-double-key-super-secret-double-key-super-secret-double-key-super-secret-double-key";

        var configurationMock = Substitute.For<IOptions<AppSettings>>();
        var httpAccessorMock = Substitute.For<IHttpContextAccessor>();
        configurationMock.Value.Returns(new AppSettings { JwtKey = key });
        var authenticationService = new API.Services.AuthenticationService(configurationMock, httpAccessorMock);
        
        // ACT
        var response = authenticationService.CreateToken(user);
        
        // ASSERT
        response.Should().NotBeNullOrEmpty();
    }
    
    [Fact]
    public async Task AuthenticationService_CreateToken_WhenTokenIsProvided_ShouldReturnDecodedToken()
    {
        // ARRANGE
        var user = new UserBuilder().Build();
        const string key = "super-secret-double-key-super-secret-double-key-super-secret-double-key-super-secret-double-key";

        var configurationMock = Substitute.For<IOptions<AppSettings>>();
        var httpAccessorMock = Substitute.For<IHttpContextAccessor>();
        configurationMock.Value.Returns(new AppSettings { JwtKey = key });
        var authenticationService = new API.Services.AuthenticationService(configurationMock, httpAccessorMock);

        // ACT
        var response = authenticationService.CreateToken(user);
        var identity = new ClaimsIdentity(ParseClaimsFromToken(response), "jwt").Claims.ToList();

        // ASSERT
        identity.Should().NotBeNullOrEmpty();
        identity[1].Value.Should().Be(user.Id.ToString());
        identity[2].Value.Should().Be(user.Username);
        identity[3].Value.Should().Be(user.Email);
    }
    
    private byte[] ParseBase64WithoutPadding(string base64)
    {
        switch (base64.Length % 4)
        {
            case 2: base64 += "=="; break;
            case 3: base64 += "="; break;
        }
        return Convert.FromBase64String(base64);
    }

    private IEnumerable<Claim> ParseClaimsFromToken(string token)
    {
        var payload = token.Split('.')[1];
        var jsonBytes = ParseBase64WithoutPadding(payload);
        var keyPair = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

        var claims = keyPair!.Select(kv => new Claim(kv.Key, kv.Value.ToString()!));
        return claims;
    }

}