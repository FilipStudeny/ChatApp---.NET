using MongoDB.Bson;
using Shared.Enums;
using Shared.Models;
 
namespace Shared.Builders;

using System;
public class UserBuilder
{
    private readonly User _user = new User();
    private static readonly Random Random = new Random();
    
    public UserBuilder WithId(ObjectId id)
    {
        _user.Id = id;
        return this;
    }

    public UserBuilder WithUsername(string username)
    {
        _user.Username = username;
        return this;
    }

    public UserBuilder WithEmail(string email)
    {
        _user.Email = email;
        return this;
    }

    public UserBuilder WithFirstName(string firstName)
    {
        _user.FirstName = firstName;
        return this;
    }

    public UserBuilder WithLastName(string lastName)
    {
        _user.LastName = lastName;
        return this;
    }

    public UserBuilder WithProfilePicture(string profilePicture)
    {
        _user.ProfilePicture = profilePicture;
        return this;
    }

    public UserBuilder WithGender(Gender gender)
    {
        _user.Gender = gender;
        return this;
    }

    public UserBuilder WithPassword(byte[] hash, byte[] salt)
    {
        _user.Password = new PasswordInfo { Hash = hash, Salt = salt };
        return this;
    }

    public UserBuilder WithFriends(List<Friend> friends)
    {
        _user.Friends = friends;
        return this;
    }

    public UserBuilder WithRegisterDate(DateTime registerDate)
    {
        _user.RegisterDate = registerDate;
        return this;
    }

    public UserBuilder WithLastActivity(DateTime lastActivity)
    {
        _user.LastActivity = lastActivity;
        return this;
    }

    public UserBuilder WithNotificationsStructId(ObjectId notificationsStructId)
    {
        _user.NotificationsStructId = notificationsStructId;
        return this;
    }
    
    private string GenerateRandomName()
    {
        // Generate a random name
        return "RandomName" + Random.Next(1000);
    }

    private string GenerateRandomEmail()
    {
        // Generate a random email
        return $"user{Random.Next(1000)}@example.com";
    }

    private string GenerateRandomUsername()
    {
        // Generate a random username
        return Faker.Internet.UserName();
    }

    private string GenerateRandomFirstName()
    {
        return Faker.Name.First();
    }

    private string GenerateRandomLastName()
    {
        return Faker.Name.Last();
    }
    private string GenerateRandomPassword()
    {
        // Generate a random password
        return Faker.RandomNumber.Next(0, 10000).ToString(); // For demonstration purposes, generate a random string
    }
    
    private Gender PickRandomGender()
    {
        var randomIndex = new Random().Next(2);
        var randomGender = (Gender)randomIndex;

        return randomGender;
    }

    private string GenerateRandomProfilePicture()
    {
        // Generate a random profile picture URL
        return "data:image/jpeg;base64," + Guid.NewGuid().ToString().Substring(0, 10); // For demonstration purposes, generate a random string
    }

    public User Build()
    {
        // Generate random data for properties that are not set
        if (_user.Id == ObjectId.Empty)
            _user.Id = ObjectId.GenerateNewId();
        if (string.IsNullOrEmpty(_user.Username))
            _user.Username = GenerateRandomUsername();
        if (string.IsNullOrEmpty(_user.Email))
            _user.Email = GenerateRandomEmail();
        if (string.IsNullOrEmpty(_user.FirstName))
            _user.FirstName = GenerateRandomFirstName();
        if (string.IsNullOrEmpty(_user.LastName))
            _user.LastName = GenerateRandomLastName();
        if (string.IsNullOrEmpty(_user.ProfilePicture))
            _user.ProfilePicture = GenerateRandomProfilePicture();
        if (_user.RegisterDate == default)
            _user.RegisterDate = DateTime.UtcNow;
        if (_user.LastActivity == default)
            _user.LastActivity = DateTime.UtcNow;
        if (_user.NotificationsStructId == ObjectId.Empty)
            _user.NotificationsStructId = ObjectId.GenerateNewId();

        return _user;
    }

}
