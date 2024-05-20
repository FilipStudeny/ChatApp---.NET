using System.Net;
using API.Repository;
using FluentAssertions;
using NSubstitute;
using Shared.Builders;
using Shared.DTOs;
using Shared.Enums;
using Shared.Models;

namespace Tests.NotificationsService;

public class TestNotificationsServiceCreateNotification(MongoDbFixture fixture) : TestBase(fixture)
{
    [Fact]
    public async Task NotificationsService_CreateNotification_ShouldReturnTrue()
    {
        // ARRANGE
        var sender = new UserBuilder().Build();
        var receiver = new UserBuilder().Build();

        var notificationStruct = new NotificationsStructBuilder().WithUser(receiver.Id).Build();
        var notification = new NotificationBuilder().WithSender(sender.Id).WithReceiver(receiver.Id)
            .WithNotificationType(NotificationType.FriendRequest).WithNotificationsStruct(notificationStruct.Id).Build(); 
        notificationStruct.NotificationsList = [notification];
        notificationStruct.NotificationsCount = 1;

        var notificationsRepository = new NotificationsRepository(fixture.DbContext);
        var userRepository = Substitute.For<IUserRepository>();
        userRepository.UserExists(sender.Id).Returns(false);
        var notificationsService = new API.Services.NotificationsService(notificationsRepository, userRepository);

        // ACT
        var response = await notificationsService.CreateNotification(notification);
        
        // ASSERT
        response.Data.Should().BeTrue();
        response.Success.Should().BeTrue();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    
    
}