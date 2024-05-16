using System.Net;
using API.Repository;
using FluentAssertions;
using NSubstitute;
using Shared.Builders;
using Shared.Enums;

namespace Tests.NotificationsService;

public class TestNotificationsServiceDeleteNotification(MongoDbFixture fixture) : TestBase(fixture)
{
    
    [Fact]
    public async Task NotificationsService_DeleteNotification_WhenUserDoesntExist_ShouldReturnException()
    {
        // ARRANGE
        var user = new UserBuilder().Build();

        var notification = new NotificationBuilder().WithSender(user.Id).Build();
        
        var notificationsRepository = new NotificationsRepository(fixture.DbContext);
        var userRepository = Substitute.For<IUserRepository>();
        userRepository.UserExists(user.Id).Returns(false);
        var notificationsService = new API.Services.NotificationsService(notificationsRepository, userRepository);
       
        // ACT
        var response = await notificationsService.DeleteNotification(user.Id, notification.Id);
        
        // ASSERT
        response.Data.Should().BeFalse();
        response.Success.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    [Fact]
    public async Task NotificationsService_DeleteNotification_WhenNotificationDoesntExist_ShouldReturnException()
    {
        // ARRANGE
        var sender = new UserBuilder().Build();
        var receiver = new UserBuilder().Build();

        var notificationStruct = new NotificationsStructBuilder().WithUser(receiver.Id).Build();
        var notification = new NotificationBuilder().WithSender(sender.Id).WithReceiver(receiver.Id)
            .WithNotificationType(NotificationType.FriendRequest).WithNotificationsStruct(notificationStruct.Id).Build(); 
        
        var collection = fixture.DbContext.NotificationsStructs;
        await collection.InsertOneAsync(notificationStruct);

        var notificationsRepository = new NotificationsRepository(fixture.DbContext);
        var userRepository = Substitute.For<IUserRepository>();
        userRepository.UserExists(receiver.Id).Returns(true);
        var notificationsService = new API.Services.NotificationsService(notificationsRepository, userRepository);
       
        // ACT
        var response = await notificationsService.DeleteNotification(receiver.Id, notification.Id);
        
        // ASSERT
        response.Data.Should().BeFalse();
        response.Success.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    [Fact]
    public async Task NotificationsService_DeleteNotification_WhenNotificationExist_ShouldReturnTrue()
    {
        // ARRANGE
        var sender = new UserBuilder().Build();
        var receiver = new UserBuilder().Build();

        var notificationStruct = new NotificationsStructBuilder().WithUser(receiver.Id).Build();
        var notification1 = new NotificationBuilder().WithSender(sender.Id).WithReceiver(receiver.Id)
            .WithNotificationType(NotificationType.FriendRequest).WithNotificationsStruct(notificationStruct.Id).Build(); 
        var notification2 = new NotificationBuilder().WithSender(sender.Id).WithReceiver(receiver.Id)
            .WithNotificationType(NotificationType.FriendRequest).WithNotificationsStruct(notificationStruct.Id).Build(); 

        notificationStruct.NotificationsList = [notification1, notification2];
        notificationStruct.NotificationsCount = 1;

        var collection = fixture.DbContext.NotificationsStructs;
        await collection.InsertOneAsync(notificationStruct);

        var notificationsRepository = new NotificationsRepository(fixture.DbContext);
        var userRepository = Substitute.For<IUserRepository>();
        userRepository.UserExists(receiver.Id).Returns(true);
        var notificationsService = new API.Services.NotificationsService(notificationsRepository, userRepository);

        // ACT
        var notificationsResponse1 = await notificationsService.GetUserNotifications(receiver.Id);
        var notificationsDeleteResponse = await notificationsService.DeleteNotification(receiver.Id, notification1.Id);
        var notificationsResponse2 = await notificationsService.GetUserNotifications(receiver.Id);

        // ASSERT
        notificationsResponse1.Data.Should().HaveCount(2);
        notificationsResponse1.Data[0].Id.Should().Be(notification1.Id);
        notificationsResponse1.Data[1].Id.Should().Be(notification2.Id);

        notificationsDeleteResponse.Data.Should().BeTrue();
        notificationsDeleteResponse.Success.Should().BeTrue();
        notificationsDeleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        
        notificationsResponse2.Data.Should().HaveCount(1);
        notificationsResponse2.Data.First().Id.Should().Be(notification2.Id);
    }

    [Fact]
    public async Task NotificationsRepository_CreateNotificationsStruct_ShouldReturnNewStruct()
    {
        // ARRANGE
        var user = new UserBuilder().Build();
        var notificationsRepository = new NotificationsRepository(fixture.DbContext);

        // ACT
        var response = await notificationsRepository.CreateNotificationsStruct(user.Id);
        
        // ASSERT
        response.User.Should().Be(user.Id);
        response.NotificationsCount.Should().Be(0);
    }
}