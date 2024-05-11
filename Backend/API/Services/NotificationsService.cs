using API.Extensions;
using API.Repository;
using MongoDB.Bson;
using Shared;
using Shared.Models;

namespace API.Services;

public interface INotificationsService
{
    public Task<ServiceResponse<List<Notification>>> GetUserNotifications(ObjectId userId);
    public Task<ServiceResponse<bool>> CreateNotification();
    public Task<ServiceResponse<bool>> RemoveNotification(ObjectId userId, ObjectId notificationId);
}

public class NotificationsService : INotificationsService
{
    private readonly INotificationsRepository _notificationsRepository;
    private readonly IUserRepository _userRepository;

    public NotificationsService(INotificationsRepository notificationsRepository, IUserRepository userRepository)
    {
        _notificationsRepository = notificationsRepository;
        _userRepository = userRepository;
    }

    public Task<ServiceResponse<List<Notification>>> GetUserNotifications(ObjectId userId)
    {
        throw new NotImplementedException();
    }

    public Task<ServiceResponse<bool>> CreateNotification()
    {
        throw new NotImplementedException();
    }

    public Task<ServiceResponse<bool>> RemoveNotification(ObjectId userId, ObjectId notificationId)
    {
        throw new NotImplementedException();
    }
}