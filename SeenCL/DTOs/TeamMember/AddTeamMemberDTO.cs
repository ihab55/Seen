namespace SeenCL.DTOs
{
    public class AddTeamMemberDTO
    {
        public int TeamID { get; set; }

        public int PlayerID { get; set; }

        public string Position { get; set; } = "N/A";

        public int JerseyNumber { get; set; }

        public bool IsInjured { get; set; }

        public bool IsRequestByCoach { get; set; }
    }
}
