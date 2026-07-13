namespace SeenCL.DTOs
{
    /// <summary>
    /// Lightweight read model returned by UserRepository.GetAll()
    /// and used in admin list views.
    /// Renamed from: UserAdminResponseDTO (kept as well for backward compat)
    /// </summary>
    public class UserSummaryDTO
    {
        public int UserId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public bool IsProfileCompleted { get; set; }
        public bool IsCoach { get; set; }
        public bool IsDeleted { get; set; }
    }
}
