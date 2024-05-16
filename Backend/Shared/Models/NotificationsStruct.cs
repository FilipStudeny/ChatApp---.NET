using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Shared.Models;

public class NotificationsStruct
{
    [BsonId]
    [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
    public ObjectId Id { get; set; }
    
    [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
    [BsonElement("user")] 
    public ObjectId User { get; set; }

    [BsonElement("notifications")] 
    public List<Notification> NotificationsList { get; set; } = [];
    
    [BsonElement("count")] public int NotificationsCount { get; set; } = 0;
    
}

