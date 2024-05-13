using MongoDB.Bson;
using Shared.Enums;

namespace Shared.DTOs;

public class NotificationDto
{
    public ObjectId? SenderId { get; set; }
    public ObjectId? ReceiverId { get; set; }
    public NotificationType? NotificationType { get; set; }
    public string? Message { get; set; }
}