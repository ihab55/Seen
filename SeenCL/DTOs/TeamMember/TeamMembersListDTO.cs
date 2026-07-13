namespace SeenCL.DTOs
{
    public class TeamMembersListDTO
    {
        public int PlayerID { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;
        public int JerseyNumber { get; set; }
        public string Status { get; set; } = "Active"; // Active or Injured as per UI
        public string? ImageUrl { get; set; }
    }
}
