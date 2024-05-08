using API.Database;
using API.Repository;
using MongoDB.Bson;
using Shared;
using Shared.Models;

namespace API.Services;

public interface INotificationsService
{
    public Task<ServiceResponse<List<Notification>>> GetUserNotifications(string userId);
    public Task<ServiceResponse<bool>> CreateNotification();
    public Task<ServiceResponse<bool>> RemoveNotification(ObjectId userId, ObjectId notificationId);
}

public class NotificationsService
{
    private readonly INotificationsRepository _notificationsRepository;
    private readonly MongoDbContext _dbContext;

    public NotificationsService(INotificationsRepository notificationsRepository, MongoDbContext dbContext)
    {
        _notificationsRepository = notificationsRepository;
        _dbContext = dbContext;
    }
}