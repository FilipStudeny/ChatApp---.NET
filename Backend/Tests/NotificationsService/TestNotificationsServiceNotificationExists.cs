using System.Net;
using API.Repository;
using FluentAssertions;
using NSubstitute;
using Shared;
using Shared.Builders;
using Shared.Enums;

namespace Tests.NotificationsService;

public class TestNotificationsServiceNotificationExists(MongoDbFixture fixture) : TestBase(fixture)
{
    [Fact]
    public async Task NotificationsService_NotificationExists_WhenNotificationDoesntExist_ShouldReturnException()
    {
        // ARRANGE
        var user = new UserBuilder().Build();
        var notification = new NotificationBuilder().Build();
        
        var notificationsRepository = new NotificationsRepository(fixture.DbContext);
        var userRepository = Substitute.For<IUserRepository>();
        var notificationsService = new API.Services.NotificationsService(notificationsRepository, userRepository);
       
        // ACT
        var response = await notificationsService.NotificationExists(user.Id, notification.Id);

        // ASSERT
        response.Data.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        response.Success.Should().BeFalse();
        response.Message.Should().Be(Messages.GenerateNotFoundMessage("Notification"));
    }
    
    [Fact]
    public async Task NotificationsService_NotificationExists_WhenNotificationExist_ShouldReturnTrue()
    {
        // ARRANGE
        var sender = new UserBuilder().Build();
        var receiver = new UserBuilder().Build();

        var notificationStruct = new NotificationsStructBuilder().WithUser(receiver.Id).Build();
        var notification1 = new NotificationBuilder().WithSender(sender.Id).WithReceiver(receiver.Id)
            .WithNotificationType(NotificationType.FriendRequest).WithNotificationsStruct(notificationStruct.Id).Build(); 

        notificationStruct.NotificationsList = [notification1];
        notificationStruct.NotificationsCount = 1;

        var collection = fixture.DbContext.NotificationsStructs;
        await collection.InsertOneAsync(notificationStruct);
        var notificationsRepository = new NotificationsRepository(fixture.DbContext);
        var userRepository = Substitute.For<IUserRepository>();
        var notificationsService = new API.Services.NotificationsService(notificationsRepository, userRepository);
       
        // ACT
        var response = await notificationsService.NotificationExists(receiver.Id, notification1.Id);

        // ASSERT
        response.Data.Should().BeTrue();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Success.Should().BeTrue();
    }
}