namespace SeenCL.DTOs
{
    public enum enNotificationType : byte
    {
        Alert = 1,
        Comment = 2,
        Program = 3
    }
    public class NotificationDTO
    {
        public int NotificationID { get; set; }
        public int UserID { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public enNotificationType NotificationType { get; set; }
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? TargetID { get; set; }

        public NotificationDTO(int notificationID, int userID, string title, string body,
                               byte notificationType, bool isRead, DateTime createdAt, int? targetID)
        {
            NotificationID = notificationID;
            UserID = userID;
            Title = title;
            Body = body;
            NotificationType = (enNotificationType)notificationType;
            IsRead = isRead;
            CreatedAt = createdAt;
            TargetID = targetID;
        }
    }
}
