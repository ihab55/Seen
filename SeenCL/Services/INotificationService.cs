using SeenCL.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SeenCL.Services
{
    public interface INotificationService
    {
        Task<IEnumerable<NotificationDTO>> GetUserNotificationsAsync(int userId);
        Task<bool> MarkAsReadAsync(int notificationId);
        /// <summary>
        /// Creates a new notification record for the given user.
        /// </summary>
        Task<bool> CreateNotificationAsync(int userId, string title, string body, int? targetId = null);
    }
}
