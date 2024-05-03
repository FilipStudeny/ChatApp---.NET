using API.Database;
using Shared.Models;

namespace API.Repository;

public interface INotificationsRepository
{
    public Task<List<Notification>> GetNotifications(string userId);
    public Task<bool> SaveNotification(Notification notification);
    public Task<bool> DeleteNotification(string notificationId);
}

public class NotificationsRepository(MongoDbContext dbContext) : INotificationsRepository
{
    private readonly MongoDbContext _dbContext = dbContext;

    public Task<List<Notification>> GetNotifications(string userId)
    {
        throw new NotImplementedException();
    }

    public Task<bool> SaveNotification(Notification notification)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteNotification(string notificationId)
    {
        throw new NotImplementedException();
    }
}