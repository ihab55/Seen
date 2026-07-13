using SeenCL.Domain.Entities;
using SeenCL.Interfaces;

namespace SeenCL.Repositories
{
    public interface INotificationRepository : IRepository<Notification, int>
    {
        IEnumerable<Notification> GetByUserId(int userId);
        bool MarkAsRead(int notificationId);
    }
}
