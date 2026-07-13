namespace SeenCL.DTOs
{
    public class UsersDTO
    {
        public int UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }

        public int? Height { get; set; } = null;
        public int? Weight { get; set; } = null;
        public double? FateRate { get; set; } = null;

        public int? DeviceID { get; set; } = null;

        public bool IsCoach { get; set; } = false;

        public bool IsDeleted { get; set; } = false;

        public bool IsProfileCompleted { get; set; } = false;

        public string? ImagePath { get; set; } = null;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; } = null;

        public UsersDTO(int UserID, string FirstName, string LastName, string UserName, string Email, string PasswordHash, int? Height, int? Weight,
            double? FateRate, int? DeviceID, bool IsCoach, bool IsProfileCompleted, bool IsDeleted, string? ImagePath, DateTime CreatedAt, DateTime? UpdatedAt)
        {
            this.UserID = UserID;
            this.FirstName = FirstName;
            this.LastName = LastName;
            this.UserName = UserName;
            this.Email = Email;
            this.PasswordHash = PasswordHash;
            this.Height = Height;
            this.Weight = Weight;
            this.FateRate = FateRate;
            this.DeviceID = DeviceID;
            this.IsCoach = IsCoach;
            this.IsProfileCompleted = IsProfileCompleted;
            this.IsDeleted = IsDeleted;
            this.ImagePath = ImagePath;
            this.CreatedAt = CreatedAt;
            this.UpdatedAt = UpdatedAt;
        }
        public UsersDTO(int UserID, string FirstName, string LastName, string UserName, string Email, string PasswordHash)
        {
            this.UserID = UserID;
            this.FirstName = FirstName;
            this.LastName = LastName;
            this.UserName = UserName;
            this.Email = Email;
            this.PasswordHash = PasswordHash;
        }
    }
}
