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
    public string Id { get; set; } = string.Empty;
    
    [BsonId]
    [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
    [BsonElement("user")] 
    public string User { get; set; } = string.Empty;
    
    [BsonElement("description")] 
    public string Description { get; set; } = string.Empty;
    
    [JsonConverter(typeof(StringEnumConverter))] 
    [BsonElement("gender")] 
    [BsonRepresentation(BsonType.String)]
    public NotificationType NotificationType { get; set; }
    
    [BsonElement("created_at")] 
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
}