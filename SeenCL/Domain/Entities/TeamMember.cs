using System;

namespace SeenCL.Domain.Entities
{
    public class TeamMember
    {
        public int MemberID { get; set; }
        public int TeamID { get; set; }
        public int PlayerID { get; set; }
        public DateTime JoinedAt { get; set; }
        public bool IsCoach { get; set; }
    }
}
