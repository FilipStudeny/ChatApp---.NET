using System.Net;
using API.Repository;
using API.Services;
using FluentAssertions;
using NSubstitute;
using NSubstitute.Extensions;
using Shared;
using Shared.Builders;
using Shared.DTOs;
using Shared.Enums;

namespace Tests.UserService;

public class TestUserServiceSendFriendRequest(MongoDbFixture fixture) : TestBase(fixture)
{
    [Fact]
    public async Task UserService_SendFriendRequest_WhenSenderDoesntExist_ShouldReturnException()
    {
        // ARRANGE
        var sender = new UserBuilder().Build();
        var receiver = new UserBuilder().Build();

        var notificationDto = new NotificationDto()
        {
            SenderId = sender.Id,
            ReceiverId = receiver.Id,
            Message = "Friend request"
        };

        var authenticationService = Substitute.For<IAuthenticationService>();
        var userRepository = new UserRepository(fixture.DbContext);
        var notificationsRepository = new NotificationsRepository(fixture.DbContext);
        var notificationsService = new API.Services.NotificationsService(notificationsRepository, userRepository);
        var userService = new API.Services.UserService(authenticationService, userRepository, notificationsService);

        // ACT
        var response = await userService.SendFriendRequest(notificationDto);

        // ASSERT
        response.Data.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        response.Success.Should().BeFalse();
        response.Message.Should().Be(Messages.SenderNotFound);
    }

    [Fact]
    public async Task UserService_SendFriendRequest_WhenReceiverDoesntExist_ShouldReturnException()
    {
        // ARRANGE
        var sender = new UserBuilder().Build();
        var receiver = new UserBuilder().Build();

        var collection = fixture.DbContext.Users;
        await collection.InsertOneAsync(sender);

        var notificationDto = new NotificationDto()
        {
            SenderId = sender.Id,
            ReceiverId = receiver.Id,
            Message = "Friend request"
        };

        var authenticationService = Substitute.For<IAuthenticationService>();
        var userRepository = new UserRepository(fixture.DbContext);
        var notificationsRepository = new NotificationsRepository(fixture.DbContext);
        var notificationsService = new API.Services.NotificationsService(notificationsRepository, userRepository);
        var userService = new API.Services.UserService(authenticationService, userRepository, notificationsService);

        // ACT
        var response = await userService.SendFriendRequest(notificationDto);

        // ASSERT
        response.Data.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        response.Success.Should().BeFalse();
        response.Message.Should().Be(Messages.ReceiverNotFound);
    }

    [Fact]
    public async Task UserService_SendFriendRequest_WhenFriendRequestSend_ShouldReturnTrue()
    {
        // ARRANGE
        var sender = new UserBuilder().Build();
        var receiver = new UserBuilder().Build();
        var notificationStruct = new NotificationsStructBuilder().WithUser(receiver.Id).Build();
        receiver.NotificationsStructId = notificationStruct.Id;

        var userCollection = fixture.DbContext.Users;
        var notificationsCollection = fixture.DbContext.NotificationsStructs;
        await userCollection.InsertManyAsync(new[] { sender, receiver });
        await notificationsCollection.InsertOneAsync(notificationStruct);

        var notificationDto = new NotificationDto()
        {
            SenderId = sender.Id,
            ReceiverId = receiver.Id,
        };

        var authenticationService = Substitute.For<IAuthenticationService>();
        var userRepository = new UserRepository(fixture.DbContext);
        var notificationsRepository = new NotificationsRepository(fixture.DbContext);
        var notificationsService = new API.Services.NotificationsService(notificationsRepository, userRepository);
        var userService = new API.Services.UserService(authenticationService, userRepository, notificationsService);

        // ACT
        var response = await userService.SendFriendRequest(notificationDto);
        var notificationStructResponse = await notificationsRepository.GetNotificationsStructByUser(receiver.Id);
        var notifications = await notificationsService.GetUserNotifications(receiver.Id);
        var friendRequest = notifications.Data.First();

        // ASSERT
        response.Data.Should().BeTrue();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Success.Should().BeTrue();
        response.Message.Should().Be(Messages.FriendRequestSent);

        friendRequest.Sender.Should().Be(sender.Id);
        friendRequest.Receiver.Should().Be(receiver.Id);
        friendRequest.Message.Should().Be(Messages.GenerateFriendRequestMessage(sender.Username));
        friendRequest.NotificationsStruct.Should().Be(notificationStructResponse.Id);

        notificationStructResponse.NotificationsCount.Should().Be(1);
        notificationStructResponse.NotificationsList.Should().HaveCount(1);
        notificationStructResponse.NotificationsList.First().Should().BeEquivalentTo(friendRequest);
    }
}