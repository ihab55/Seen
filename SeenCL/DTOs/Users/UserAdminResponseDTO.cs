namespace SeenCL.DTOs
{
    public class UserAdminResponseDTO
    {
        public int UserID { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? UserName { get; set; }
        public bool IsProfileCompleted { get; set; }
        public bool IsCoach { get; set; }
        public bool IsDeleted { get; set; }
    }
}
