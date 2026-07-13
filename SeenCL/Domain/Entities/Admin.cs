using System;

namespace SeenCL.Domain.Entities
{
    public class Admin
    {
        public int AdminID { get; set; }
        public string AdminName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }
    }
}
