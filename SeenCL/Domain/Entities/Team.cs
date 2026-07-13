using System;

namespace SeenCL.Domain.Entities
{
    public class Team
    {
        public int TeamID { get; set; }
        public int CoachID { get; set; }
        public string TeamName { get; set; } = string.Empty;
        public string TeamCode { get; set; } = string.Empty;
        public int SubscriptionID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
    }
}
