using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Shared.Enums;

namespace Shared.Models;

public class User
{
    [BsonId]
    [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
    public ObjectId Id { get; set; }
    [BsonElement("username")] public string Username { get; set; } = string.Empty;
    [BsonElement("email")] public string Email { get; set; } = string.Empty;
    [BsonElement("firstname")] public string FirstName { get; set; } = string.Empty;
    [BsonElement("lastname")] public string LastName { get; set; } = string.Empty;
    [BsonElement("profile_picture")] public string ProfilePicture { get; set; } = string.Empty;
    [JsonConverter(typeof(StringEnumConverter))] 
    [BsonElement("gender")] 
    [BsonRepresentation(BsonType.String)]
    public Gender Gender { get; set; }
    [BsonElement("password")] public PasswordInfo Password { get; set; }
    [BsonElement("friends")] public List<Friend> Friends { get; set; } = [];
    [BsonElement("register_date")] public DateTime RegisterDate { get; set; } = DateTime.UtcNow;
    [BsonElement("last_activity")] public DateTime LastActivity { get; set; } = DateTime.UtcNow;
    
    [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
    [BsonElement("notifications_struct")] 
    public ObjectId NotificationsStructId { get; set; }

}
    
public class PasswordInfo
{
    [BsonElement("hash")] public required byte[] Hash { get; set; }
    [BsonElement("salt")] public required byte[] Salt { get; set; }
}

public class Friend
{
    [BsonId]
    [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
    public ObjectId Id { get; set; }
    [BsonElement("username")] public string Username { get; set; } = string.Empty;
}

