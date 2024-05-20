using System.Net;
using API.Repository;
using API.Services;
using FluentAssertions;
using NSubstitute;
using Shared;
using Shared.Builders;
using Shared.DTOs;
using Shared.Enums;

namespace Tests.UserService;

public class TestUserServiceAddFriend(MongoDbFixture fixture) : TestBase(fixture)
{
    [Fact]
    public async Task UserService_AddFriend_WhenUserNotFound_ShouldReturnException()
    {
        // ARRANGE
        var sender = new UserBuilder().Build();
        var receiver = new UserBuilder().Build();
        var notificationStruct = new NotificationsStructBuilder().WithUser(receiver.Id).Build();
        receiver.NotificationsStructId = notificationStruct.Id;

        var notification = new NotificationBuilder().WithSender(sender.Id).WithReceiver(receiver.Id)
            .WithMessage(Messages.GenerateFriendRequestMessage(sender.Username))
            .WithNotificationType(NotificationType.FriendRequest).WithNotificationsStruct(notificationStruct.Id).Build();
        notificationStruct.NotificationsCount = 1;
        notificationStruct.NotificationsList = [notification];

        var notificationsCollection = fixture.DbContext.NotificationsStructs;
        await notificationsCollection.InsertOneAsync(notificationStruct);

        var authenticationService = Substitute.For<IAuthenticationService>();
        authenticationService.GetUserIdFromToken().Returns(receiver.Id);
        var userRepository = new UserRepository(fixture.DbContext);
        var notificationsRepository = new NotificationsRepository(fixture.DbContext);
        var notificationsService = new API.Services.NotificationsService(notificationsRepository, userRepository);
        var userService = new API.Services.UserService(authenticationService, userRepository, notificationsService);

        // ACT
        var response = await userService.AddFriend(notification.Id, sender.Id);

        // ASSERT
        response.Data.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        response.Success.Should().BeFalse();
        response.Message.Should().Be(Messages.UserNotFound);
    }
    
    [Fact]
    public async Task UserService_AddFriend_WhenFriendNotFound_ShouldReturnException()
    {
        // ARRANGE
        var sender = new UserBuilder().Build();
        var receiver = new UserBuilder().Build();
        var notificationStruct = new NotificationsStructBuilder().WithUser(receiver.Id).Build();
        receiver.NotificationsStructId = notificationStruct.Id;

        var notification = new NotificationBuilder().WithSender(sender.Id).WithReceiver(receiver.Id)
            .WithMessage(Messages.GenerateFriendRequestMessage(sender.Username))
            .WithNotificationType(NotificationType.FriendRequest).WithNotificationsStruct(notificationStruct.Id).Build();
        notificationStruct.NotificationsCount = 1;
        notificationStruct.NotificationsList = [notification];

        var userCollection = fixture.DbContext.Users;
        var notificationsCollection = fixture.DbContext.NotificationsStructs;
        await userCollection.InsertManyAsync([sender]);
        await notificationsCollection.InsertOneAsync(notificationStruct);

        var authenticationService = Substitute.For<IAuthenticationService>();
        authenticationService.GetUserIdFromToken().Returns(sender.Id);
        var userRepository = new UserRepository(fixture.DbContext);
        var notificationsRepository = new NotificationsRepository(fixture.DbContext);
        var notificationsService = new API.Services.NotificationsService(notificationsRepository, userRepository);
        var userService = new API.Services.UserService(authenticationService, userRepository, notificationsService);

        // ACT
        var response = await userService.AddFriend(notification.Id, receiver.Id);

        // ASSERT
        response.Data.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        response.Success.Should().BeFalse();
        response.Message.Should().Be(Messages.UserNotFound);
    }
    
    [Fact]
    public async Task UserService_AddFriend_WhenRequestSatisfied_ShouldReturnTrue()
    {
        // ARRANGE
        var sender = new UserBuilder().Build();
        var receiver = new UserBuilder().Build();
        var notificationStruct = new NotificationsStructBuilder().WithUser(receiver.Id).Build();
        receiver.NotificationsStructId = notificationStruct.Id;

        var notification = new NotificationBuilder().WithSender(sender.Id).WithReceiver(receiver.Id)
            .WithMessage(Messages.GenerateFriendRequestMessage(sender.Username))
            .WithNotificationType(NotificationType.FriendRequest).WithNotificationsStruct(notificationStruct.Id).Build();
        notificationStruct.NotificationsCount = 1;
        notificationStruct.NotificationsList = [notification];

        var userCollection = fixture.DbContext.Users;
        var notificationsCollection = fixture.DbContext.NotificationsStructs;
        await userCollection.InsertManyAsync([sender, receiver]);
        await notificationsCollection.InsertOneAsync(notificationStruct);

        var authenticationService = Substitute.For<IAuthenticationService>();
        authenticationService.GetUserIdFromToken().Returns(sender.Id);
        var userRepository = new UserRepository(fixture.DbContext);
        var notificationsRepository = new NotificationsRepository(fixture.DbContext);
        var notificationsService = new API.Services.NotificationsService(notificationsRepository, userRepository);
        var userService = new API.Services.UserService(authenticationService, userRepository, notificationsService);

        // ACT
        var response = await userService.AddFriend(notification.Id, receiver.Id);
        var userResponse1 = await userService.GetUser(sender.Id);
        var userResponse2 = await userService.GetUser(receiver.Id);
        var structResponse = await notificationsService.GetUserNotifications(sender.Id);

        // ASSERT
        response.Data.Should().BeTrue();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Success.Should().BeTrue();

        userResponse1.Data.Friends.Should().HaveCount(1);
        userResponse1.Data.Friends.First().Id.Should().Be(receiver.Id);
        userResponse1.Data.Friends.First().Username.Should().Be(receiver.Username);
        
        userResponse2.Data.Friends.Should().HaveCount(1);
        userResponse2.Data.Friends.First().Id.Should().Be(sender.Id);
        userResponse2.Data.Friends.First().Username.Should().Be(sender.Username);

        structResponse.Data.Should().HaveCount(0);
    }
}