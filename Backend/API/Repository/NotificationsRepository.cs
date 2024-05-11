using API.Database;
using Microsoft.AspNetCore.SignalR;
using MongoDB.Bson;
using MongoDB.Driver;
using Shared.Models;

namespace API.Repository;

public interface INotificationsRepository
{
    public Task<NotificationsStruct> GetNotificationStructById(ObjectId notificationStructId);
    public Task<NotificationsStruct> GetNotificationStructByUser(ObjectId userId);
    public Task<List<Notification>> GetAllUserNotifications(ObjectId notificationStruct);
    public Task<Notification> GetNotification(ObjectId notificationStruct, ObjectId notificationId);
    public Task<bool> CreateNotification(ObjectId userId, Notification notification);
    public Task<bool> DeleteNotification(ObjectId notificationId, ObjectId userId);

    public Task<NotificationsStruct> CreateNotificationsStruct(ObjectId userId);
}

public class NotificationsRepository(MongoDbContext database, IUserRepository userRepository) : INotificationsRepository
{
    public async Task<NotificationsStruct> GetNotificationStructById(ObjectId notificationStructId)
    {
        var notificationStruct =
            await database.NotificationsStructs.Find(n => n.Id == notificationStructId).FirstOrDefaultAsync();
        return notificationStruct;
    }

    public async Task<NotificationsStruct> GetNotificationStructByUser(ObjectId userId)
    {
        var notificationStruct = await database.NotificationsStructs.Find(n => n.User == userId).FirstOrDefaultAsync()
                                 ?? await CreateNotificationsStruct(userId);
        return notificationStruct;
    }

    public async Task<List<Notification>> GetAllUserNotifications(ObjectId userId)
    {
        var notifications = await GetNotificationStructByUser(userId);
        return notifications.NotificationsList;
    }


    public async Task<Notification> GetNotification(ObjectId notificationStruct, ObjectId notificationId)
    {
        var notification = await database.NotificationsStructs
            .Find(n => n.Id == notificationStruct)
            .Project(n => n.NotificationsList.FirstOrDefault(x => x.Id == notificationId))
            .FirstOrDefaultAsync();
        return notification;
    }

    public async Task<bool> CreateNotification(ObjectId userId, Notification notification)
    {
        var notificationsStruct = await GetNotificationStructByUser(userId);

        notificationsStruct.NotificationsList.Add(notification);

        var filter = Builders<NotificationsStruct>.Filter.Eq("_id", notificationsStruct.Id);
        var update = Builders<NotificationsStruct>.Update
            .Set("NotificationsList", notificationsStruct.NotificationsList);

        var result = await database.NotificationsStructs.UpdateOneAsync(filter, update);

        // Check if the update was successful
        return result.ModifiedCount > 0;
    }


    public async Task<NotificationsStruct> CreateNotificationsStruct(ObjectId userId)
    {
        var newStruct = new NotificationsStruct()
        {
            User = userId,
            NotificationsCount = 0,
            NotificationsList = []
        };

        await database.NotificationsStructs.InsertOneAsync(newStruct);
        return newStruct;
    }

    public Task<bool> DeleteNotification(ObjectId notificationId, ObjectId userId)
    {
        throw new NotImplementedException();
    }
}