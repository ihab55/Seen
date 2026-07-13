using System;

namespace SeenCL.Domain.Entities
{
    public class User
    {
        public int UserID { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public int? Height { get; set; }
        public int? Weight { get; set; }
        public double? FateRate { get; set; }
        public int? DeviceID { get; set; }
        public bool IsCoach { get; set; }
        public bool IsProfileCompleted { get; set; }
        public bool IsDeleted { get; set; }
        public string? ImagePath { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
