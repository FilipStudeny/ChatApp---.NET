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
    public Task CreateNotification(Notification notification);
    public Task DeleteNotification(ObjectId notificationId);
    public Task<bool> NotificationExists(ObjectId notificationId);
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

    public Task CreateNotification(Notification notification)
    {
        throw new NotImplementedException();
    }

    public Task DeleteNotification(ObjectId notificationId)
    {
        throw new NotImplementedException();
    }

    public Task<bool> NotificationExists(ObjectId notificationId)
    {
        throw new NotImplementedException();
    }
}