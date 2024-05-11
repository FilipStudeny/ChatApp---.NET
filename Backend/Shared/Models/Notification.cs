using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Shared.Enums;

namespace Shared.Models;

public class Notification
{
    [BsonId]
    [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
    public ObjectId Id { get; set; }
    
    [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
    [BsonElement("receiver")] 
    public ObjectId Receiver { get; set; }
    
    [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
    [BsonElement("sender")] 
    public ObjectId Sender { get; set; }
    
    [BsonElement("message")] 
    public string Message { get; set; } = string.Empty;
    
    [JsonConverter(typeof(StringEnumConverter))] 
    [BsonElement("notification_type")] 
    [BsonRepresentation(BsonType.String)]
    public NotificationType NotificationType { get; set; }
    
    [BsonElement("created_at")] 
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
    [BsonElement("notification_struct")] 
    public ObjectId NotificationsStruct { get; set; }

    
}