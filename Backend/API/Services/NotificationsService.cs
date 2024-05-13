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
    public Task<ServiceResponse<bool>> CreateNotification(NotificationDto notificationDto);
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

    public async Task<ServiceResponse<bool>> CreateNotification(NotificationDto notificationDto)
    {
        try
        {
            var sender = await _userRepository.UserExists(notificationDto.SenderId.Value);
            if (sender == false)
            {
                throw new UserNotFoundException("Your account doesn't exist, create one.");
            }

            var receiver = await _userRepository.UserExists(notificationDto.ReceiverId.Value);
            if (receiver == false)
            {
                throw new UserNotFoundException("Couldn't find user, try again.");
            }
            
            var notificationStruct = await _notificationsRepository.GetNotificationsStructByUser(notificationDto.ReceiverId.Value) ??
                                     await _notificationsRepository.CreateNotificationsStruct(notificationDto.ReceiverId.Value);

            var notification = new Notification()
            {
                CreatedAt = DateTime.Now,
                Message = notificationDto.Message,
                NotificationsStruct = notificationStruct.Id,
                Sender = notificationDto.SenderId.Value,
                Receiver = notificationDto.ReceiverId.Value,
                NotificationType = notificationDto.NotificationType.Value
            };

            await _notificationsRepository.CreateNotification(notificationDto.ReceiverId.Value,notification);

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

    public async Task<ServiceResponse<bool>> RemoveNotification(ObjectId userId, ObjectId notificationId)
    {
        try
        {
            var sender = await _userRepository.UserExists(userId);
            if (sender == false)
            {
                throw new UserNotFoundException("Couldn't find user, try again.");
            }

            var deleted = await _notificationsRepository.DeleteNotification(userId, notificationId);
            if (deleted)
            {
                return new ServiceResponse<bool>() { Data = true };
            }
            
            return new ServiceResponse<bool>()
            {
                Data = false
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
}