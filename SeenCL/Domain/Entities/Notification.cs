using System;
using SeenCL.Domain.Enums;

namespace SeenCL.Domain.Entities
{
    public class Notification
    {
        public int NotificationID { get; set; }
        public int UserID { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public NotificationType NotificationType { get; set; }
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? TargetID { get; set; }
    }
}
