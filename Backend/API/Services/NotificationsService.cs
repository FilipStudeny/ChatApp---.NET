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


    public async Task<ServiceResponse<List<Notification>>> GetUserNotifications(ObjectId userId)
    {
        try
        {
            var userExists = await _userRepository.UserExists(userId);
            if (!userExists)
            {
                throw new UserNotFoundException("Couldn't find user notifications, user doesn't exist.");
            }

            var notifications = await _notificationsRepository.GetAllUserNotifications(userId);
            return new ServiceResponse<List<Notification>>
            {
                Data = notifications,
            };
            
        }
        catch (CustomException ex)
        {
            return new ServiceResponse<List<Notification>>
            {
                StatusCode = ex.StatusCode,
                Success = false,
                Message = ex.Message
            };
        }
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