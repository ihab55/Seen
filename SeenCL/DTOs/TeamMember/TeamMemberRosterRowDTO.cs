using System;

namespace SeenCL.DTOs
{
    /// <summary>
    /// Row returned by SP_TeamMembers_GetByPlayer (member + user profile).
    /// </summary>
    public class TeamMemberRosterRowDTO
    {
        public int MemberID { get; set; }
        public int TeamID { get; set; }
        public int PlayerID { get; set; }
        public DateTime JoinedAt { get; set; }
        public bool IsCoach { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public bool IsInjured { get; set; }
        public string? ImagePath { get; set; }
        public bool IsProfileCompleted { get; set; }
        public string Status { get; set; } = "Active";
        public int JerseyNumber { get; set; }
        public string Position { get; set; } = string.Empty;
    }
}
