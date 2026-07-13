namespace SeenCL.DTOs
{
    public class AdminResponseDTO
    {
        public int AdminID { get; set; }
        public string AdminName { get; set; }
        public string Email { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }
        public AdminResponseDTO(int AdminID, string AdminName, string Email, DateTime CreatedAt, bool IsActive)
        {
            this.AdminID = AdminID;
            this.AdminName = AdminName;
            this.Email = Email;
            this.CreatedAt = CreatedAt;
            this.IsActive = IsActive;
        }
    }
}
