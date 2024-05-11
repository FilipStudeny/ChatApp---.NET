using MongoDB.Bson;
using Shared.Models;

namespace Shared.Builders;

public class NotificationsStructBuilder
{
    private readonly NotificationsStruct _notificationsStruct = new NotificationsStruct();

    public NotificationsStructBuilder WithId(ObjectId id)
    {
        _notificationsStruct.Id = id;
        return this;
    }

    public NotificationsStructBuilder WithUser(ObjectId userId)
    {
        _notificationsStruct.User = userId;
        return this;
    }

    public NotificationsStructBuilder WithNotificationsList(List<Notification> notificationsList)
    {
        _notificationsStruct.NotificationsList = notificationsList;
        return this;
    }

    public NotificationsStructBuilder WithNotificationsCount()
    {
        _notificationsStruct.NotificationsCount = _notificationsStruct.NotificationsList.Count;
        return this;
    }

    public NotificationsStruct Build()
    {
        if (_notificationsStruct.Id == ObjectId.Empty)
            _notificationsStruct.Id = ObjectId.GenerateNewId();
        if (_notificationsStruct.NotificationsList == null)
            _notificationsStruct.NotificationsList = new List<Notification>();

        return _notificationsStruct;
    }
}