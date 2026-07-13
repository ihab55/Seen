using SeenCL.Domain.Entities;
using SeenCL.Domain.Enums;
using SeenCL.DTOs;
using SeenCL.Repositories;
using SeenCL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeenBLL.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _repository;

        public NotificationService(INotificationRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<NotificationDTO>> GetUserNotificationsAsync(int userId)
        {
            var notifications = await Task.FromResult(_repository.GetByUserId(userId));
            return notifications.Select(n => new NotificationDTO(
                n.NotificationID,
                n.UserID,
                n.Title,
                n.Body,
                (byte)n.NotificationType,
                n.IsRead,
                n.CreatedAt,
                n.TargetID
            ));
        }

        public async Task<bool> MarkAsReadAsync(int notificationId)
        {
            return await Task.FromResult(_repository.MarkAsRead(notificationId));
        }

        public async Task<bool> CreateNotificationAsync(
            int userId, string title, string body, int? targetId = null)
        {
            var notification = new Notification
            {
                UserID           = userId,
                Title            = title,
                Body             = body,
                NotificationType = NotificationType.Alert,
                IsRead           = false,
                CreatedAt        = DateTime.Now,
                TargetID         = targetId
            };
            return await Task.FromResult(_repository.Create(notification) > 0);
        }
    }
}
