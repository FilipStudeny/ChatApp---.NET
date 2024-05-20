using API.Extensions;
using API.Repository;
using MongoDB.Bson;
using Shared;
using Shared.DTOs;
using Shared.Models;

namespace API.Services;

public interface INotificationsService
{
    public Task<ServiceResponse<Notification>> GetNotification(ObjectId userId, ObjectId notificationId);
    public Task<ServiceResponse<List<Notification>>> GetUserNotifications(ObjectId userId);
    public Task<ServiceResponse<bool>> CreateNotification(Notification notification);
    public Task<ServiceResponse<bool>> DeleteNotification(ObjectId userId, ObjectId notificationId);

    public Task<ServiceResponse<bool>> NotificationExists(ObjectId userOrNotificationsStructId, ObjectId notificationId);
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

    public async Task<ServiceResponse<Notification>> GetNotification(ObjectId userId, ObjectId notificationId)
    {
        try
        {
            var user = await _userRepository.UserExists(userId);
            if (!user)
            {
                throw new UserNotFoundException("User doesn't exist");
            }

            var notification = await _notificationsRepository.GetNotification(userId, notificationId);
            if (notification == null)
            {
                return new ServiceResponse<Notification>
                {
                    Data = null,
                    Message = "Notification not found"
                };
            }

            return new ServiceResponse<Notification>
            {
                Data = notification,
            };
        }
        catch (CustomException ex)
        {
            return new ServiceResponse<Notification>
            {
                Data = null,
                StatusCode = ex.StatusCode,
                Success = false,
                Message = ex.Message
            };
        }
    }

    public async Task<ServiceResponse<List<Notification>>> GetUserNotifications(ObjectId userId)
    {
        try
        {
            var user = await _userRepository.UserExists(userId);
            if (!user)
            {
                throw new UserNotFoundException("User doesn't exist");
            }

            var notifications = await _notificationsRepository.GetNotifications(userId);
            return new ServiceResponse<List<Notification>>
            {
                Data = notifications,
            };
        }
        catch (CustomException ex)
        {
            return new ServiceResponse<List<Notification>>
            {
                Data = null,
                StatusCode = ex.StatusCode,
                Success = false,
                Message = ex.Message
            };
        }
    }

    public async Task<ServiceResponse<bool>> CreateNotification(Notification notification)
    {
        try
        {
            await _notificationsRepository.CreateNotification(notification.Receiver, notification);

            return new ServiceResponse<bool>()
            {
                Data = true
            };

        }
        catch (CustomException ex)
        {
            return new ServiceResponse<bool>
            {
                Data = false,
                StatusCode = ex.StatusCode,
                Success = false,
                Message = ex.Message
            };
        }
    }

    public async Task<ServiceResponse<bool>> DeleteNotification(ObjectId userId, ObjectId notificationId)
    {
        try
        {
            var userExists = await _userRepository.UserExists(userId);
            if (userExists == false)
            {
                throw new UserNotFoundException("Couldn't find user, try again.");
            }

            var notificationExists = await _notificationsRepository.NotificationExists(userId, notificationId);
            if (notificationExists == false)
            {
                throw new UserNotFoundException("Couldn't find notification, try again.");
            }

            var deleted = await _notificationsRepository.DeleteNotification(userId, notificationId);
            if (deleted == false)
            {
                return new ServiceResponse<bool>() { Data = false };
            }
            
            return new ServiceResponse<bool>()
            {
                Data = true
            };

        }
        catch (CustomException ex)
        {
            return new ServiceResponse<bool>
            {
                Data = false,
                StatusCode = ex.StatusCode,
                Success = false,
                Message = ex.Message
            };
        }
    }

    public async Task<ServiceResponse<bool>> NotificationExists(ObjectId userOrNotificationsStructId, ObjectId notificationId)
    {
        try
        {
            var exists = await _notificationsRepository.NotificationExists(userOrNotificationsStructId, notificationId);
            if (exists)
            {
                return new ServiceResponse<bool>() { Data = true };
            }

            throw new DatabaseException(Messages.GenerateNotFoundMessage("Notification"));
        }
        catch (CustomException ex)
        {
            return new ServiceResponse<bool>
            {
                Data = false,
                StatusCode = ex.StatusCode,
                Success = false,
                Message = ex.Message
            };
        }    }
}