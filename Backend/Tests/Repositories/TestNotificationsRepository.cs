using API.Repository;
using FluentAssertions;
using NSubstitute;
using Shared.Builders;
using Shared.Enums;

namespace Tests.Repositories;

public class TestNotificationsRepository(MongoDbFixture fixture) : TestBase(fixture)
{
    [Fact]
    public async Task NotificationsRepository_GetNotificationsStructById_WhenStructIsNotFound_ShouldReturnNull()
    {
        // ARRANGE
        var user = new UserBuilder().Build();
        var notificationsStruct = new NotificationsStructBuilder().WithUser(user.Id).Build();
        
        var notificationsRepository = new NotificationsRepository(fixture.DbContext);

        // ACT
        var response = await notificationsRepository.GetNotificationsStructById(notificationsStruct.Id);

        // ASSERT
        response.Should().BeNull();
    }
    
    [Fact]
    public async Task NotificationsRepository_GetNotificationsStructById_WhenStructIsFound_ShouldReturnStruct()
    {
        // ARRANGE
        var user = new UserBuilder().Build();
        var notificationsStruct = new NotificationsStructBuilder().WithUser(user.Id).Build();
        
        var collection = fixture.DbContext.NotificationsStructs;
        await collection.InsertOneAsync(notificationsStruct);
        var notificationsRepository = new NotificationsRepository(fixture.DbContext);

        // ACT
        var response = await notificationsRepository.GetNotificationsStructById(notificationsStruct.Id);

        // ASSERT
        response.Should().NotBeNull();
        response.User.Should().Be(notificationsStruct.User).And.Be(user.Id);
    }
    
    [Fact]
    public async Task NotificationsRepository_GetNotificationsStructByUserId_WhenStructIsNotFound_ShouldReturnNull()
    {
        // ARRANGE
        var user = new UserBuilder().Build();
        
        var notificationsRepository = new NotificationsRepository(fixture.DbContext);

        // ACT
        var response = await notificationsRepository.GetNotificationsStructByUser(user.Id);

        // ASSERT
        response.Should().BeNull();
    }
    
    [Fact]
    public async Task NotificationsRepository_GetNotificationsStructByUserId_WhenStructIsFound_ShouldReturnStruct()
    {
        // ARRANGE
        var user = new UserBuilder().Build();
        var notificationsStruct = new NotificationsStructBuilder().WithUser(user.Id).Build();
        
        var collection = fixture.DbContext.NotificationsStructs;
        await collection.InsertOneAsync(notificationsStruct);
        var notificationsRepository = new NotificationsRepository(fixture.DbContext);

        // ACT
        var response = await notificationsRepository.GetNotificationsStructByUser(user.Id);

        // ASSERT
        response.Should().NotBeNull();
        response.User.Should().Be(notificationsStruct.User).And.Be(user.Id);
    }

    [Fact]
    public async Task NotificationsRepository_GetNotification_WhenNotificationIsNotFound_ShouldReturnNull()
    {
        // ARRANGE
        var user = new UserBuilder().Build();
        var notificationsStruct = new NotificationsStructBuilder().WithUser(user.Id).Build();
        var notification = new NotificationBuilder().Build();
        
        var collection = fixture.DbContext.NotificationsStructs;
        await collection.InsertOneAsync(notificationsStruct);
        var notificationsRepository = new NotificationsRepository(fixture.DbContext);
        
        // ACT
        var response = await notificationsRepository.GetNotification(notificationsStruct.Id, notification.Id);

        // ASSERT
        response.Should().BeNull();
    }
    
    [Fact]
    public async Task NotificationsRepository_GetNotification_WhenNotificationIsFound_ShouldReturnNotification()
    {
        // ARRANGE
        var sender = new UserBuilder().Build();
        var receiver = new UserBuilder().Build();
        var notification = new NotificationBuilder().WithSender(sender.Id).WithReceiver(receiver.Id)
            .WithNotificationType(NotificationType.FriendRequest).Build();
        var notificationsStruct = new NotificationsStructBuilder().WithUser(receiver.Id)
            .WithNotificationsList([notification]).WithNotificationsCount().Build();
        notification.NotificationsStruct = notificationsStruct.Id;

        var collection = fixture.DbContext.NotificationsStructs;
        await collection.InsertOneAsync(notificationsStruct);
        var notificationsRepository = new NotificationsRepository(fixture.DbContext);
        
        // ACT
        var response = await notificationsRepository.GetNotification(notificationsStruct.Id, notification.Id);

        // ASSERT
        response.Should().NotBeNull();
        response.Id.Should().Be(notification.Id);
        response.Sender.Should().Be(sender.Id);
        response.Receiver.Should().Be(receiver.Id);
        response.NotificationsStruct.Should().Be(notificationsStruct.Id);
    }
}