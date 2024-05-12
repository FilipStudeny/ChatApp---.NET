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

    [Fact]
    public async Task NotificationsRepository_CreateNotificationsStruct_CreateNewStruct_ShouldReturnNewStruct()
    {
        // ARRANGE
        var user = new UserBuilder().Build();
        var notificationsRepository = new NotificationsRepository(fixture.DbContext);

        // ACT
        var response = await notificationsRepository.CreateNotificationsStruct(user.Id);
        var foundStruct = await notificationsRepository.GetNotificationsStructById(response.Id);

        // ASSERT
        response.User.Should().Be(user.Id);
        foundStruct.Id.Should().Be(response.Id);
        foundStruct.User.Should().Be(user.Id);
    }

    [Fact]
    public async Task
        NotificationsRepository_CreateNotification_CreateNewNotification_ShouldReturnUpdatedNotificationsStruct()
    {
        // ARRANGE
        var sender = new UserBuilder().Build();
        var receiver = new UserBuilder().Build();
        var notification = new NotificationBuilder().WithReceiver(receiver.Id).WithSender(sender.Id)
            .WithNotificationType(NotificationType.FriendRequest).Build();
        var notificationsStruct = new NotificationsStructBuilder().WithUser(receiver.Id).Build();
        notification.NotificationsStruct = notificationsStruct.Id;

        var collection = fixture.DbContext.NotificationsStructs;
        await collection.InsertOneAsync(notificationsStruct);

        var notificationsRepository = new NotificationsRepository(fixture.DbContext);

        // ACT
        await notificationsRepository.CreateNotification(notificationsStruct.Id, notification);
        var response = await notificationsRepository.GetNotificationsStructById(notificationsStruct.Id);

        // ASSERT
        response.Id.Should().Be(notificationsStruct.Id);
        response.NotificationsCount.Should().Be(1);
        response.NotificationsList.Should().NotBeEmpty();
        response.NotificationsList.Should().HaveCount(1);
        response.NotificationsList.First().NotificationsStruct.Should().Be(notificationsStruct.Id);
    }

    [Fact]
    public async Task
        NotificationsRepository_DeleteNotification_WhenNotificationIsToBeDeleted_ShouldReturnListOfNotifications()
    {
        // ARRANGE
        var sender = new UserBuilder().Build();
        var receiver = new UserBuilder().Build();
        var notification1 = new NotificationBuilder().WithReceiver(receiver.Id).WithSender(sender.Id)
            .WithNotificationType(NotificationType.FriendRequest).Build();
        var notification2 = new NotificationBuilder().WithReceiver(receiver.Id).WithSender(sender.Id)
            .WithNotificationType(NotificationType.FriendRequest).Build();
        var notificationsStruct = new NotificationsStructBuilder().WithUser(receiver.Id).Build();
        notification1.NotificationsStruct = notificationsStruct.Id;
        notification2.NotificationsStruct = notificationsStruct.Id;
        notificationsStruct.NotificationsCount = 2;
        notificationsStruct.NotificationsList = [notification1, notification2];

        var collection = fixture.DbContext.NotificationsStructs;
        await collection.InsertOneAsync(notificationsStruct);
        var notificationsRepository = new NotificationsRepository(fixture.DbContext);

        // ACT
        var deleteResponse = await notificationsRepository.DeleteNotification(notificationsStruct.Id, notification2.Id);
        var notificationResponse = await notificationsRepository.GetNotificationsStructById(notificationsStruct.Id);

        // ASSERT
        deleteResponse.Should().BeTrue();
        notificationResponse.NotificationsCount.Should().Be(1);
        notificationResponse.NotificationsList.Should().HaveCount(1);
        notificationResponse.NotificationsList.First().Id.Should().Be(notification1.Id);
        notificationResponse.NotificationsList.First().Id.Should().NotBe(notification2.Id);
    }

    [Fact]
    public async Task NotificationsRepository_UpdateNotificationsStruct_WhenUserIsChanged_ShouldReturnStructWithNewUser()
    {
        // ARRANGE
        var user = new UserBuilder().Build();
        var user2 = new UserBuilder().Build();
        var notificationsStruct = new NotificationsStructBuilder().WithUser(user.Id).Build();
        
        var collection = fixture.DbContext.NotificationsStructs;
        await collection.InsertOneAsync(notificationsStruct);
        var notificationsRepository = new NotificationsRepository(fixture.DbContext);

        // ACT
        var updateResponse =
            await notificationsRepository.UpdateNotificationsStruct(notificationsStruct.Id, "user", user2.Id);
        var structResponse = await notificationsRepository.GetNotificationsStructById(notificationsStruct.Id);
        
        // ASSERT
        updateResponse.Should().BeTrue();
        structResponse.Id.Should().Be(notificationsStruct.Id);
        structResponse.User.Should().Be(user2.Id);
        structResponse.User.Should().NotBe(user.Id);
    }
}