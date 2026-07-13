namespace SeenCL.DTOs
{
    public class TeamMemberDTO
    {
        public int MemberID { get; set; }
        public int TeamID { get; set; }
        public int PlayerID { get; set; }
        public DateTime JoinedAt { get; set; }
        public bool IsCoach { get; set; }

        public TeamMemberDTO(int memberID, int teamID, int playerID, DateTime joinedAt, bool isCoach)
        {
            this.MemberID = memberID;
            this.TeamID = teamID;
            this.PlayerID = playerID;
            this.JoinedAt = joinedAt;
            this.IsCoach = isCoach;
        }

    }
}
