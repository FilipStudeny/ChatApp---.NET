using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;

        [BsonElement("username")]
        public string Username { get; set; } = string.Empty;

        [BsonElement("email")]
        public string Email { get; set; } = string.Empty;

        [BsonElement("password")]
        public required PasswordInfo Password { get; set; }

        [BsonElement("friends")]
        public List<string> Friends { get; set; } = new List<string>();

        [BsonElement("register_date")]
        public DateTime RegisterDate { get; set; } = DateTime.UtcNow;

        [BsonElement("last_activity")]
        public DateTime LastActivity { get; set; } = DateTime.UtcNow;

    }

    public class PasswordInfo
    {
        [BsonElement("hash")]
        public string Hash { get; set; } = string.Empty;

        [BsonElement("salt")]
        public string Salt { get; set; } = string.Empty;
    }
}
