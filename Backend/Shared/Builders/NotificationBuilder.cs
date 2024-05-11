using MongoDB.Bson;
using Shared.Enums;
using Shared.Models;

namespace Shared.Builders;

public class NotificationBuilder
{
    private readonly Notification _notification = new Notification();

    public NotificationBuilder WithId(ObjectId id)
    {
        _notification.Id = id;
        return this;
    }

    public NotificationBuilder WithReceiver(ObjectId receiverId)
    {
        _notification.Receiver = receiverId;
        return this;
    }

    public NotificationBuilder WithSender(ObjectId senderId)
    {
        _notification.Sender = senderId;
        return this;
    }

    public NotificationBuilder WithMessage(string message)
    {
        _notification.Message = message;
        return this;
    }

    public NotificationBuilder WithNotificationType(NotificationType type)
    {
        _notification.NotificationType = type;
        return this;
    }

    public NotificationBuilder WithCreatedAt(DateTime createdAt)
    {
        _notification.CreatedAt = createdAt;
        return this;
    }
    
    public NotificationBuilder WithNotificationsStruct(ObjectId notificationsStructId)
    {
        _notification.NotificationsStruct = notificationsStructId;
        return this;
    }

    public Notification Build()
    {
        if (_notification.Id == ObjectId.Empty)
            _notification.Id = ObjectId.GenerateNewId();
        if (_notification.CreatedAt == default)
            _notification.CreatedAt = DateTime.UtcNow;

        return _notification;
    }
}