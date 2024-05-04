using API.Database;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using API.Services;
using API.Repository;
using Microsoft.OpenApi.Models;

namespace API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

//**    MONGO DB SETUP      **//
        builder.Services.Configure<MongoSettings>(builder.Configuration.GetSection(nameof(MongoSettings)));
        builder.Services.AddSingleton<MongoDbContext>(serviceProvider =>
        {
            var settings = serviceProvider.GetRequiredService<IOptions<MongoSettings>>().Value;
            return new MongoDbContext(settings.ConnectionString, settings.Database);
        });

        builder.Services.Configure<AppSettings>(builder.Configuration.GetSection(nameof(AppSettings)));

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1",
                new Microsoft.OpenApi.Models.OpenApiInfo { Title = "ChatApp API", Version = "v1" });
            options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                Description = "Enter JWT token",
                Name = "Authorization",
                Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "bearer"
            });

            options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] { }
                }
            });
        });

//** SERVICES **//
        builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
        builder.Services.AddScoped<IUserService, UserService>();

//** REPOSITORY **//
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<INotificationsRepository, NotificationsRepository>();

//*** AUTHENTICATION ***//
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                string secret = builder.Configuration.GetSection("AppSettings:JwtKey").Value;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        System.Text.Encoding.UTF8.GetBytes(secret)
                    ),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
        builder.Services.AddHttpContextAccessor();
        var app = builder.Build();

// Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;
        var mongo = services.GetRequiredService<MongoDbContext>();
        mongo.ClearDatabase();
        mongo.SeedData();

        app.Run();
    }
}