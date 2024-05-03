using MongoDB.Bson.Serialization.Attributes;

namespace Shared.Models;

public class Notifications
{
    [BsonId]
    [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;
    
    [BsonId]
    [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
    [BsonElement("user")] 
    public string User { get; set; } = string.Empty;

    [BsonElement("notifications")] 
    public List<Notification> NotificationsList { get; set; } = [];
    
    [BsonElement("register_date")] 
    public DateTime RegisterDate { get; set; } = DateTime.UtcNow;

    [BsonElement("count")] public int NotificationsCount { get; set; } = 0;

}

