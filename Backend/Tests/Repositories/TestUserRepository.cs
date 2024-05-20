using API.Repository;
using FluentAssertions;
using Shared.Builders;
using Shared.Models;

namespace Tests.Repositories;

public class TestUserRepository(MongoDbFixture fixture) : TestBase(fixture)
{
    [Fact]
    public async Task UserRepository_GetUser_WhenUserIsNotFound_ShouldReturnNull()
    {
        // ARRANGE
        var user = new UserBuilder().Build();
        var userRepository = new UserRepository(fixture.DbContext);

        // ACT
        var response = await userRepository.GetUser(user.Id);

        // ASSERT
        response.Should().BeNull();
    }
    
    [Fact]
    public async Task UserRepository_GetUser_WhenUserExists_ShouldReturnUser()
    {
        // ARRANGE
        var user = new UserBuilder().Build();
        var user2 = new UserBuilder().Build();
        var collection = fixture.DbContext.Users;
        await collection.InsertManyAsync([user, user2]);
        var userRepository = new UserRepository(fixture.DbContext);

        // ACT
        var response = await userRepository.GetUser(user.Id);

        // ASSERT
        response.Should().NotBeNull();
        response.Id.Should().Be(user.Id);
        response.Id.Should().NotBe(user2.Id);
        response.FirstName.Should().Be(user.FirstName);
        response.LastName.Should().Be(user.LastName);
    }
    
    [Fact]
    public async Task UserRepository_GetAllUser_WhenNoUsersFound_ShouldReturnNull()
    {
        // ARRANGE
        var userRepository = new UserRepository(fixture.DbContext);

        // ACT
        var response = await userRepository.GetAllUsers();

        // ASSERT
        response.Should().HaveCount(0);
    }
    
    [Fact]
    public async Task UserRepository_GetUsers_ShouldReturnListOfAllUsers()
    {
        // ARRANGE
        var user = new UserBuilder().Build();
        var user2 = new UserBuilder().Build();
        var collection = fixture.DbContext.Users;
        await collection.InsertManyAsync([user, user2]);
        var userRepository = new UserRepository(fixture.DbContext);

        // ACT
        var response = await userRepository.GetAllUsers();

        // ASSERT
        response.Should().NotBeNull();
        response.First().Id.Should().Be(user.Id);
        response.First().FirstName.Should().Be(user.FirstName);
        response.First().LastName.Should().Be(user.LastName);
        response[1].Id.Should().Be(user2.Id);
        response[1].FirstName.Should().Be(user2.FirstName);
        response[1].LastName.Should().Be(user2.LastName);
    }

    [Fact]
    public async Task UserRepository_CreateUser_WhenNewUserIsProvided_ShouldCreateNewUser()
    {
        // ARRANGE
        var user = new UserBuilder().Build();
        var newUser = new User()
        {
            Email = user.Email,
            FirstName = user.FirstName,
            Gender = user.Gender,
            LastActivity = user.LastActivity,
            Password = user.Password,
        };
        var userRepository = new UserRepository(fixture.DbContext);

        // ACT
        await userRepository.CreateUser(newUser);
        var response = await userRepository.GetUser(newUser.Id);
        
        // ASSERT
        response.Should().NotBeNull();
        response.Id.Should().Be(newUser.Id);
    }

    [Fact]
    public async Task UserRepository_UserExists_UserDoesntExist_ShouldReturnFalse()
    {
        // ARRANGE
        var user = new UserBuilder().Build();
        var userRepository = new UserRepository(fixture.DbContext);
        
        // ACT
        var response = await userRepository.UserExists(user.Id);
        
        // ASSERT
        response.Should().BeFalse();
    }
    
    [Fact]
    public async Task UserRepository_UserExists_UserExists_ShouldReturnTrue()
    {
        // ARRANGE
        var user = new UserBuilder().Build();
        var userRepository = new UserRepository(fixture.DbContext);
        var collection = fixture.DbContext.Users;
        await collection.InsertOneAsync(user);
        
        // ACT
        var response = await userRepository.UserExists(user.Id);
        
        // ASSERT
        response.Should().BeTrue();
    }

    [Fact]
    public async Task UserRepository_GetUserByEmailOrUsername_WhenUserDoesntExist_ShouldReturnNull()
    {
        // ARRANGE
        var user = new UserBuilder().Build();
        var userRepository = new UserRepository(fixture.DbContext);
        
        // ACT
        var response = await userRepository.GetUserByEmailOrUsername(user.Email, user.Username);
        
        // ASSERT
        response.Should().BeNull();
    }
    
    [Fact]
    public async Task UserRepository_GetUserByEmailOrUsername_WhenUserExists_ShouldReturnTrue()
    {
        // ARRANGE
        var user = new UserBuilder().Build();
        var userRepository = new UserRepository(fixture.DbContext);
        var collection = fixture.DbContext.Users;
        await collection.InsertOneAsync(user);
        
        // ACT
        var response = await userRepository.GetUserByEmailOrUsername(user.Email, user.Username);
        
        // ASSERT
        response.Should().NotBeNull();
        response.Id.Should().Be(user.Id);
        response.FirstName.Should().Be(user.FirstName);
        response.LastName.Should().Be(user.LastName);
    }

    [Fact]
    public async Task UserRepository_UpdateUserData_WhenNewUsernameIsProvided_ShouldReturnTrueOnSucess()
    {
        // ARRANGE
        var user = new UserBuilder().Build();
        var oldUsername = user.Username;
        const string newUsername = "Žižka";
        var userRepository = new UserRepository(fixture.DbContext);
        var collection = fixture.DbContext.Users;
        await collection.InsertOneAsync(user);
        
        // ACT
        var updateResponse = await userRepository.UpdateUserData(user.Id, "username", newUsername);
        var userResponse = await userRepository.GetUser(user.Id);
        
        // ASSERT
        updateResponse.Should().BeTrue();
        userResponse.Should().NotBeNull();
        userResponse.Username.Should().NotBe(oldUsername);
        userResponse.Username.Should().Be(newUsername);
    }
    
    
    [Fact]
    public async Task UserRepository_UpdateUserData_WhenNotificationsStructIsProvided_ShouldReturnTrueOnSucess()
    {
        // ARRANGE
        var user = new UserBuilder().Build();
        var notificationStruct = new NotificationsStructBuilder().WithUser(user.Id).Build();
        
        var userRepository = new UserRepository(fixture.DbContext);
        var usersCollection = fixture.DbContext.Users;
        
        var notificationsStructsCollection = fixture.DbContext.NotificationsStructs;
        await usersCollection.InsertOneAsync(user);
        await notificationsStructsCollection.InsertOneAsync(notificationStruct);
        
        // ACT
        var updateResponse = await userRepository.UpdateUserData(user.Id, "notifications_struct", notificationStruct.Id);
        var userResponse = await userRepository.GetUser(user.Id);
        
        // ASSERT
        updateResponse.Should().BeTrue();
        userResponse.Should().NotBeNull();
        userResponse.NotificationsStructId.Should().NotBeNull();
        userResponse.NotificationsStructId.Should().Be(notificationStruct.Id);
    }
    
    [Fact]
    public async Task UserRepository_AddFriend_WhenNewFriendIsAdded_ShouldReturnTrueOnSucess()
    {
        // ARRANGE
        var user = new UserBuilder().Build();
        var friend = new UserBuilder().Build();
        var newFriend = new Friend()
        {
            Id = friend.Id,
            Username = friend.Username
        };
        
        var userRepository = new UserRepository(fixture.DbContext);
        var usersCollection = fixture.DbContext.Users;
        await usersCollection.InsertOneAsync(user);
        
        // ACT
        var updateResponse = await userRepository.AddFriend(user.Id, newFriend);
        var userResponse = await userRepository.GetUser(user.Id);
        
        // ASSERT
        updateResponse.Should().BeTrue();
        userResponse.Should().NotBeNull();
        userResponse.NotificationsStructId.Should().NotBeNull();
        userResponse.Friends.Should().HaveCount(1);
        userResponse.Friends.First().Username.Should().Be(friend.Username);
    }
}