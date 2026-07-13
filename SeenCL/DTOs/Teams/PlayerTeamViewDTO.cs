using System;

namespace SeenCL.DTOs
{
    /// <summary>
    /// DTO representing a team where the user is a player/member
    /// Maps directly to SP_Teams_GetByUserID stored procedure result
    /// </summary>
    public class PlayerTeamViewDTO
    {
        public int TeamID { get; set; }
        public string TeamName { get; set; }
        public string CoachFirstName { get; set; }
        public string CoachLastName { get; set; }
        public string CoachName => $"{CoachFirstName} {CoachLastName}";
        public DateTime Joined { get; set; }
        public string MemberSince => Joined.ToString("MMM dd, yyyy");
        public int DaysAsMember => (DateTime.UtcNow - Joined).Days;

        public PlayerTeamViewDTO()
        {
        }

        public PlayerTeamViewDTO(int teamID, string teamName, string coachFirstName, string coachLastName, DateTime joined)
        {
            TeamID = teamID;
            TeamName = teamName;
            CoachFirstName = coachFirstName;
            CoachLastName = coachLastName;
            Joined = joined;
        }
    }
}

