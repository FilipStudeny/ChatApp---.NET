using API.Database;
using Microsoft.AspNetCore.SignalR;
using MongoDB.Bson;
using MongoDB.Driver;
using Shared.Models;

namespace API.Repository;

public interface INotificationsRepository
{
    public Task<NotificationsStruct?> GetNotificationsStructById(ObjectId id);
    public Task<NotificationsStruct?> GetNotificationsStructByUser(ObjectId userId);
    public Task<Notification?> GetNotification(ObjectId userOrNotificationsStructId,ObjectId notificationId);
    public Task<List<Notification>> GetNotifications(ObjectId userOrNotificationsStructId);
    public Task CreateNotification(ObjectId userOrNotificationsStructId,Notification notification);
    public Task<bool> DeleteNotification(ObjectId userOrNotificationsStructId, ObjectId notificationId);
    public Task<bool> NotificationExists(ObjectId userOrNotificationsStructId, ObjectId notificationId);
    public Task<NotificationsStruct> CreateNotificationsStruct(ObjectId userId);
    public Task<bool> UpdateNotificationsStruct(ObjectId notificationsStructId, string fieldName, object newValue);
}

public class NotificationsRepository(MongoDbContext database) : INotificationsRepository
{
    public async Task<NotificationsStruct?> GetNotificationsStructById(ObjectId id)
    {
        var filter = Builders<NotificationsStruct>.Filter.Eq(u => u.Id, id);
        return await database.NotificationsStructs.Find(filter).FirstOrDefaultAsync();
    }

    public async Task<NotificationsStruct?> GetNotificationsStructByUser(ObjectId userId)
    {
        var filter = Builders<NotificationsStruct>.Filter.Eq(u => u.User, userId);
        return await database.NotificationsStructs.Find(filter).FirstOrDefaultAsync();
    }

    public async Task<Notification?> GetNotification(ObjectId userOrNotificationsStructId,ObjectId notificationId)
    {
        var notificationsStruct = await GetNotificationsStructById(userOrNotificationsStructId) 
                                  ?? await GetNotificationsStructByUser(userOrNotificationsStructId);

        var notification = notificationsStruct?.NotificationsList.Find(n => n.Id == notificationId);
        return notification;
    }

    public async Task<List<Notification>> GetNotifications(ObjectId userOrNotificationsStructId)
    {
        var notificationsStruct = await GetNotificationsStructById(userOrNotificationsStructId) 
                                  ?? await GetNotificationsStructByUser(userOrNotificationsStructId);
        return notificationsStruct?.NotificationsList ?? [];
    }

    public async Task CreateNotification(ObjectId userOrNotificationsStructId, Notification notification)
    {
        var notificationsStruct = (await GetNotificationsStructById(userOrNotificationsStructId) ??
                                   await GetNotificationsStructByUser(userOrNotificationsStructId)) ?? 
                                  await CreateNotificationsStruct(notification.Receiver);

        notificationsStruct.NotificationsList.Add(notification);
        notificationsStruct.NotificationsCount = notificationsStruct.NotificationsList.Count;

        var filter = Builders<NotificationsStruct>.Filter.Eq("_id", notificationsStruct.Id);
        var update = Builders<NotificationsStruct>.Update
            .Set("notifications", notificationsStruct.NotificationsList)
            .Set("count", notificationsStruct.NotificationsCount);

        await database.NotificationsStructs.UpdateOneAsync(filter, update);
    }
    public async Task<bool> DeleteNotification(ObjectId userOrNotificationsStructId, ObjectId notificationId)
    {
        var filter = Builders<NotificationsStruct>.Filter.Eq("_id", userOrNotificationsStructId) &
                     Builders<NotificationsStruct>.Filter.ElemMatch(
                         ns => ns.NotificationsList,
                         Builders<Notification>.Filter.Eq(n => n.Id, notificationId));

        var update = Builders<NotificationsStruct>.Update
            .PullFilter(ns => ns.NotificationsList, n => n.Id == notificationId)
            .Inc(ns => ns.NotificationsCount, -1); // Decrement NotificationsCount by 1

        var result = await database.NotificationsStructs.UpdateOneAsync(filter, update);

        return result.ModifiedCount > 0;
    }

    public async Task<bool> NotificationExists(ObjectId userOrNotificationsStructId, ObjectId notificationId)
    {
        var notifications = await GetNotifications(userOrNotificationsStructId);
        var notification = notifications.Find(n => n.Id == notificationId);
        return notification != null;
    }

    public async Task<NotificationsStruct> CreateNotificationsStruct(ObjectId userId)
    {
        var newNotificationsStruct = new NotificationsStruct()
        {
            User = userId,
            NotificationsCount = 0,
            NotificationsList = []
        };

        await database.NotificationsStructs.InsertOneAsync(newNotificationsStruct);
        return newNotificationsStruct;
    }

    public async Task<bool> UpdateNotificationsStruct(ObjectId notificationsStructId, string fieldName, object newValue)
    {
        var filter = Builders<NotificationsStruct>.Filter.Eq("_id", notificationsStructId);
        var update = Builders<NotificationsStruct>.Update.Set(fieldName, newValue);

        var result = await database.NotificationsStructs.UpdateOneAsync(filter, update);

        return result.ModifiedCount > 0;
    }
}