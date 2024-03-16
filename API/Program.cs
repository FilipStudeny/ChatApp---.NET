using API.Database;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using API.Extensions;

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
builder.Services.AddSwaggerGen();


//*** AUTHENTICATION ***//
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer();

builder.Services.ConfigureOptions<JwtBearerOptionsExtension>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
