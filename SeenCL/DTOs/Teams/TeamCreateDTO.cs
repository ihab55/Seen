namespace SeenCL.DTOs
{
    public class TeamCreateDTO
    {
        public int CoachID { get; set; }
        public string TeamName { get; set; } = string.Empty;
        public string TeamCode { get; set; } = string.Empty;
        public int SubscriptionID { get; set; }
    }
}

