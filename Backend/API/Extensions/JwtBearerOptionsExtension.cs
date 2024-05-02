using API.Database;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace API.Extensions
{
    public class JwtBearerOptionsExtension(IOptions<AppSettings> configuration) : IConfigureNamedOptions<JwtBearerOptions>
    {
        private readonly AppSettings configuration = configuration.Value;

        public void Configure(string? name, JwtBearerOptions options)
        {
            Configure(options);
        }

        public void Configure(JwtBearerOptions options)
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(
                    System.Text.Encoding.UTF8.GetBytes(configuration.JwtKey)
                ),
                ValidateIssuer = false,
                ValidateAudience = false
            };
        }
    }
}
