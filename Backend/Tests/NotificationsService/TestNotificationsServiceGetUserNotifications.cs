using System.Net;
using API.Repository;
using FluentAssertions;
using NSubstitute;
using Shared.Builders;
using Shared.Enums;

namespace Tests.NotificationsService;

public class TestNotificationsServiceGetUserNotifications(MongoDbFixture fixture) : TestBase(fixture)
{
    [Fact]
    public async Task NotificationsService_GetUserNotifications_WhenUserDoesntExist_ShouldThroException()
    {
        // ARRANGE
        var receiver = new UserBuilder().Build();
        var notificationsRepository = new NotificationsRepository(fixture.DbContext);
        var userRepository = Substitute.For<IUserRepository>();
        userRepository.UserExists(receiver.Id).Returns(false);
        var notificationsService = new API.Services.NotificationsService(notificationsRepository, userRepository);
       
        // ACT
        var response = await notificationsService.GetUserNotifications(receiver.Id);
        
        // ASSERT
        response.Data.Should().BeNullOrEmpty();
        response.Success.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    [Fact]
    public async Task NotificationsService_GetUserNotifications_WhenNoNotificationsExist_ShouldReturnEmptyList()
    {
        // ARRANGE
        var receiver = new UserBuilder().Build();
        var notificationsRepository = new NotificationsRepository(fixture.DbContext);
        var userRepository = Substitute.For<IUserRepository>();
        userRepository.UserExists(receiver.Id).Returns(true);
        var notificationsService = new API.Services.NotificationsService(notificationsRepository, userRepository);
       
        // ACT
        var response = await notificationsService.GetUserNotifications(receiver.Id);

        // ASSERT
        response.Data.Should().BeNullOrEmpty();
        response.Success.Should().BeTrue();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task NotificationsService_GetUserNotifications_WhenNotificationsExist_ShouldReturnListOfNotifications()
    {
        // ARRANGE
        var sender = new UserBuilder().Build();
        var receiver = new UserBuilder().Build();

        var notificationStruct = new NotificationsStructBuilder().WithUser(receiver.Id).Build();
        var notification1 = new NotificationBuilder().WithSender(sender.Id).WithReceiver(receiver.Id)
            .WithNotificationType(NotificationType.FriendRequest).WithNotificationsStruct(notificationStruct.Id).Build();
        var notification2 = new NotificationBuilder().WithSender(sender.Id).WithReceiver(receiver.Id)
            .WithNotificationType(NotificationType.Message).WithNotificationsStruct(notificationStruct.Id).Build();
        notificationStruct.NotificationsList = [notification1, notification2];
        notificationStruct.NotificationsCount = 2;
        
        var collection = fixture.DbContext.NotificationsStructs;
        await collection.InsertOneAsync(notificationStruct);
        
        var notificationsRepository = new NotificationsRepository(fixture.DbContext);
        var userRepository = Substitute.For<IUserRepository>();
        userRepository.UserExists(receiver.Id).Returns(true);
        var notificationsService = new API.Services.NotificationsService(notificationsRepository, userRepository);
       
        // ACT
        var response = await notificationsService.GetUserNotifications(receiver.Id);

        // ASSERT
        response.Data.Should().NotBeNullOrEmpty();
        response.Data.Should().HaveCount(2);
        response.Data[0].Id.Should().Be(notification1.Id);
        response.Data[0].NotificationType.Should().Be(NotificationType.FriendRequest);
        response.Data[1].Id.Should().Be(notification2.Id);
        response.Data[1].NotificationType.Should().Be(NotificationType.Message);
        response.Success.Should().BeTrue();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task NotificationsService_GetNotification_WhenUserDoesntExist_ShouldReturnException()
    {
        
        // ARRANGE
        var receiver = new UserBuilder().Build();
        var notification = new NotificationBuilder().WithReceiver(receiver.Id).Build();
        
        var notificationsRepository = new NotificationsRepository(fixture.DbContext);
        var userRepository = Substitute.For<IUserRepository>();
        userRepository.UserExists(receiver.Id).Returns(false);
        var notificationsService = new API.Services.NotificationsService(notificationsRepository, userRepository);
       
        // ACT
        var response = await notificationsService.GetNotification(receiver.Id, notification.Id);
        
        // ASSERT
        response.Data.Should().BeNull();
        response.Success.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    [Fact]
    public async Task NotificationsService_GetNotification_WhenNotificationDoesntExist_ShouldReturnNull()
    {
        
        // ARRANGE
        var receiver = new UserBuilder().Build();
        var notification = new NotificationBuilder().WithReceiver(receiver.Id).Build();
        
        var notificationsRepository = new NotificationsRepository(fixture.DbContext);
        var userRepository = Substitute.For<IUserRepository>();
        userRepository.UserExists(receiver.Id).Returns(true);
        var notificationsService = new API.Services.NotificationsService(notificationsRepository, userRepository);
       
        // ACT
        var response = await notificationsService.GetNotification(receiver.Id, notification.Id);
        
        // ASSERT
        response.Data.Should().BeNull();
        response.Success.Should().BeTrue();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    
    [Fact]
    public async Task NotificationsService_GetNotification_WhenNotificationExists_ShouldReturnNotification()
    {
        
        // ARRANGE
        var sender = new UserBuilder().Build();
        var receiver = new UserBuilder().Build();

        var notificationStruct = new NotificationsStructBuilder().WithUser(receiver.Id).Build();
        var notification = new NotificationBuilder().WithSender(sender.Id).WithReceiver(receiver.Id)
            .WithNotificationType(NotificationType.FriendRequest).WithNotificationsStruct(notificationStruct.Id).Build(); 
        notificationStruct.NotificationsList = [notification];
        notificationStruct.NotificationsCount = 1;
        
        var collection = fixture.DbContext.NotificationsStructs;
        await collection.InsertOneAsync(notificationStruct);

        var notificationsRepository = new NotificationsRepository(fixture.DbContext);
        var userRepository = Substitute.For<IUserRepository>();
        userRepository.UserExists(receiver.Id).Returns(true);
        var notificationsService = new API.Services.NotificationsService(notificationsRepository, userRepository);
       
        // ACT
        var response = await notificationsService.GetNotification(receiver.Id, notification.Id);
        
        // ASSERT
        response.Data.Should().NotBeNull();
        response.Data.Id.Should().Be(notification.Id);
        response.Success.Should().BeTrue();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}