using MongoDB.Bson.Serialization.Attributes;

namespace API.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;
        [BsonElement("username")] public string Username { get; set; } = string.Empty;
        [BsonElement("email")] public string Email { get; set; } = string.Empty;
        [BsonElement("firstname")] public string FirstName { get; set; } = string.Empty;
        [BsonElement("lastname")] public string LastName { get; set; } = string.Empty;
        [BsonElement("gender")] public Gender Gender { get; set; }
        [BsonElement("password")] public required PasswordInfo Password { get; set; }
        [BsonElement("friends")] public List<Friend> Friends { get; set; } = [];
        [BsonElement("register_date")] public DateTime RegisterDate { get; set; } = DateTime.UtcNow;
        [BsonElement("last_activity")] public DateTime LastActivity { get; set; } = DateTime.UtcNow;

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
        public string Id { get; set; } = string.Empty;
        
        [BsonElement("profile_picture")] public string ProfilePicture { get; set; } = string.Empty;
        [BsonElement("username")] public string Username { get; set; } = string.Empty;
        [BsonElement("gender")] public Gender Gender { get; set; }
    }
}
