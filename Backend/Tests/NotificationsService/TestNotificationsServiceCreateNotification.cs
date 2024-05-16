using System.Net;
using API.Repository;
using FluentAssertions;
using NSubstitute;
using Shared.Builders;
using Shared.DTOs;
using Shared.Enums;

namespace Tests.NotificationsService;

public class TestNotificationsServiceCreateNotification(MongoDbFixture fixture) : TestBase(fixture)
{
    [Fact]
    public async Task NotificationsService_CreateNotification_WhenSenderDoesntExist_ShouldReturnException()
    {
        // ARRANGE
        var sender = new UserBuilder().Build();
        var receiver = new UserBuilder().Build();

        var notificationStruct = new NotificationsStructBuilder().WithUser(receiver.Id).Build();
        var notification = new NotificationBuilder().WithSender(sender.Id).WithReceiver(receiver.Id)
            .WithNotificationType(NotificationType.FriendRequest).WithNotificationsStruct(notificationStruct.Id).Build(); 
        notificationStruct.NotificationsList = [notification];
        notificationStruct.NotificationsCount = 1;

        var notificationDto = new NotificationDto()
        {
            ReceiverId = receiver.Id,
            SenderId = sender.Id
        };
        
        var notificationsRepository = new NotificationsRepository(fixture.DbContext);
        var userRepository = Substitute.For<IUserRepository>();
        userRepository.UserExists(sender.Id).Returns(false);
        var notificationsService = new API.Services.NotificationsService(notificationsRepository, userRepository);

        // ACT
        var response = await notificationsService.CreateNotification(notificationDto);
        
        // ASSERT
        response.Data.Should().BeFalse();
        response.Success.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    [Fact]
    public async Task NotificationsService_CreateNotification_WhenReceiverDoesntExist_ShouldReturnException()
    {
        // ARRANGE
        var sender = new UserBuilder().Build();
        var receiver = new UserBuilder().Build();

        var notificationStruct = new NotificationsStructBuilder().WithUser(receiver.Id).Build();
        var notification = new NotificationBuilder().WithSender(sender.Id).WithReceiver(receiver.Id)
            .WithNotificationType(NotificationType.FriendRequest).WithNotificationsStruct(notificationStruct.Id).Build(); 
        notificationStruct.NotificationsList = [notification];
        notificationStruct.NotificationsCount = 1;
        
        var collection = fixture.DbContext.Users;
        await collection.InsertOneAsync(sender);

        var notificationDto = new NotificationDto()
        {
            ReceiverId = receiver.Id,
            SenderId = sender.Id
        };
        
        var notificationsRepository = new NotificationsRepository(fixture.DbContext);
        var userRepository = Substitute.For<IUserRepository>();
        userRepository.UserExists(sender.Id).Returns(true);
        var notificationsService = new API.Services.NotificationsService(notificationsRepository, userRepository);

        // ACT
        var response = await notificationsService.CreateNotification(notificationDto);
        
        // ASSERT
        response.Data.Should().BeFalse();
        response.Success.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    [Fact]
    public async Task NotificationsService_CreateNotification_WhenSenderAndReceiverExist_ShouldReturnTrueOnNotificationCreation()
    {
        // ARRANGE
        var sender = new UserBuilder().Build();
        var receiver = new UserBuilder().Build();

        var notificationStruct = new NotificationsStructBuilder().WithUser(receiver.Id).Build();
        var notification = new NotificationBuilder().WithSender(sender.Id).WithReceiver(receiver.Id)
            .WithNotificationType(NotificationType.FriendRequest).WithNotificationsStruct(notificationStruct.Id).Build(); 
        notificationStruct.NotificationsList = [notification];
        notificationStruct.NotificationsCount = 1;
        
        var collection = fixture.DbContext.Users;
        await collection.InsertManyAsync([sender, receiver]);

        var notificationMessage = $"{sender.Username} has sent you friend request";
        var notificationDto = new NotificationDto()
        {
            ReceiverId = receiver.Id,
            SenderId = sender.Id,
            Message = notificationMessage,
            NotificationType = NotificationType.FriendRequest
        };
        
        var notificationsRepository = new NotificationsRepository(fixture.DbContext);
        var userRepository = Substitute.For<IUserRepository>();
        userRepository.UserExists(sender.Id).Returns(true);
        userRepository.UserExists(receiver.Id).Returns(true);
        var notificationsService = new API.Services.NotificationsService(notificationsRepository, userRepository);

        // ACT
        var response = await notificationsService.CreateNotification(notificationDto);
        
        // ASSERT
        response.Data.Should().BeTrue();
        response.Success.Should().BeTrue();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}