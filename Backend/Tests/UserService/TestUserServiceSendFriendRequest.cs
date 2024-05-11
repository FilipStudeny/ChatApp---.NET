using System.Net;
using API.Repository;
using API.Services;
using FluentAssertions;
using MongoDB.Driver;
using NSubstitute;
using Shared.Builders;
using Shared.DTOs;
using Shared.Enums;
using Shared.Models;

namespace Tests.UserService;

public class TestUserServiceSendFriendRequest(MongoDbFixture fixture) : TestBase(fixture)
{
    [Fact]
    public async Task UserService_SendFriendRequest_WhenSenderIsNotFound_ShouldReturnException()
    {
        // ARRANGE
        var user1 = new UserBuilder().Build();
        var user2 = new UserBuilder().Build();

        var userRepository = Substitute.For<IUserRepository>();
        var authenticationService = Substitute.For<IAuthenticationService>();
        var notificationRepository = Substitute.For<INotificationsRepository>();
        userRepository.GetUser(user1.Id)!.Returns<Task<User?>>(Task.FromResult<User?>(null));
        var userService = new API.Services.UserService(authenticationService, userRepository, notificationRepository);

        var notification = new NotificationDto()
        {
            SenderId = user1.Id,
            ReceiverId = user2.Id,
            NotificationType = NotificationType.FriendRequest
        };

        // ACT
        var response = await userService.SendFriendRequest(notification);

        // ASSERT
        response.Data.Should().BeFalse();
        response.Success.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task UserService_SendFriendRequest_WhenReceiverIsNotFound_ShouldReturnException()
    {
        // ARRANGE
        var user1 = new UserBuilder().Build();
        var user2 = new UserBuilder().Build();

        var database = fixture.GetDatabase("ChatApp");
        var collection = database.GetCollection<User>("Users");
        await collection.InsertOneAsync(user1);


        var userRepository = Substitute.For<IUserRepository>();
        var authenticationService = Substitute.For<IAuthenticationService>();
        var notificationRepository = Substitute.For<INotificationsRepository>();
        userRepository.GetUser(user1.Id)!.Returns<Task<User?>>(Task.FromResult<User?>(user1));
        userRepository.GetUser(user2.Id)!.Returns<Task<User?>>(Task.FromResult<User?>(null));
        var userService = new API.Services.UserService(authenticationService, userRepository, notificationRepository);

        var notification = new NotificationDto()
        {
            SenderId = user1.Id,
            ReceiverId = user2.Id,
            NotificationType = NotificationType.FriendRequest
        };

        // ACT
        var response = await userService.SendFriendRequest(notification);

        // ASSERT
        response.Data.Should().BeFalse();
        response.Success.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task UserService_SendFriendRequest_WhenUsersFound_ShouldCreateNewNotification()
    {
        // ARRANGE
        var user1 = new UserBuilder().Build();
        var user2 = new UserBuilder().Build();
        var notificationDto = new NotificationDto()
        {
            SenderId = user1.Id,
            ReceiverId = user2.Id,
            NotificationType = NotificationType.FriendRequest
        };

        var notification = new NotificationBuilder().WithSender(user1.Id).WithReceiver(user2.Id)
            .WithMessage($"{user1.Username} has sent you a friend request")
            .WithNotificationType(NotificationType.FriendRequest).Build();
        var notificationsStruct = new NotificationsStructBuilder().WithUser(user2.Id).WithNotificationsList([notification]).Build();

        var database = fixture.GetDatabase("ChatApp");
        var usersCollection = database.GetCollection<User>("Users");
        var notificationsCollection = database.GetCollection<NotificationsStruct>("Notifications");
        await notificationsCollection.InsertOneAsync(notificationsStruct);
        await usersCollection.InsertManyAsync([user1, user2]);

        var userRepository = Substitute.For<IUserRepository>();
        var authenticationService = Substitute.For<IAuthenticationService>();
        var notificationRepository = Substitute.For<INotificationsRepository>();

        userRepository.GetUser(user1.Id).Returns(user1);
        userRepository.GetUser(user2.Id).Returns(user2);
        userRepository.UserExists(user1.Id).Returns(true);
        userRepository.UserExists(user2.Id).Returns(true);

        notificationRepository.CreateNotification(user2.Id, notification).Returns(true);
        notificationRepository.GetNotificationStructByUser(user2.Id).Returns(notificationsStruct);
        
        var userService = new API.Services.UserService(authenticationService, userRepository, notificationRepository);
        var notificationService = new NotificationsService(notificationRepository, userRepository);

        // ACT
        var response1 = await userService.SendFriendRequest(notificationDto);
       // var response2 = await notificationService.GetUserNotifications(user2.Id);

        // ASSERT
        response1.Data.Should().BeTrue();
        response1.Success.Should().BeTrue();
        response1.StatusCode.Should().Be(HttpStatusCode.OK);


    }
}