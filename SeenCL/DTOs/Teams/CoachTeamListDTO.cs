namespace SeenCL.DTOs
{
    public class CoachTeamListDTO
    {
        public int TeamID { get; set; }
        public string TeamName { get; set; } = string.Empty;
        public string TeamCode { get; set; } = string.Empty;
        public int PlayerCount { get; set; }
        public string SubscriptionName { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
    }
}
