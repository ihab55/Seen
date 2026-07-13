namespace SeenCL.DTOs
{
    public class TeamDTO
    {
        public int TeamID { get; set; }
        public int CoachID { get; set; }
        public string TeamName { get; set; }
        public string TeamCode { get; set; }
        public int SubscriptionID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }

        public TeamDTO(
            int teamID,
            int coachID,
            string teamName,
            string teamCode,
            int subscriptionID,
            DateTime startDate,
            DateTime endDate,
            bool isActive)
        {
            TeamID = teamID;
            CoachID = coachID;
            TeamName = teamName;
            TeamCode = teamCode;
            SubscriptionID = subscriptionID;
            StartDate = startDate;
            EndDate = endDate;
            IsActive = isActive;
        }
    }

}

