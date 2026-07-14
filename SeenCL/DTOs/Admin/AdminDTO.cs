namespace SeenCL.DTOs
{
    public class AdminDTO
    {
        public int AdminID { get; set; }
        public string AdminName { get; set; }
        public string Email { get; set; }
        public string? Password { get; set; }
        public string PasswordHash { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }
        public AdminDTO(int AdminID, string AdminName, string Email, string PasswordHash, DateTime CreatedAt, bool IsActive)
        {
            this.AdminID = AdminID;
            this.AdminName = AdminName;
            this.Email = Email;
            this.PasswordHash = PasswordHash;
            this.CreatedAt = CreatedAt;
            this.IsActive = IsActive;
        }
    }
}
