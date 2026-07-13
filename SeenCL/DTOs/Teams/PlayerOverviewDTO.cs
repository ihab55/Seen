namespace SeenCL.DTOs
{
    public class PlayerOverviewDTO
    {
        public string TeamName { get; set; }
        public string CoachName { get; set; }
        public string UserRole { get; set; }

        public DateTime UserJoinedDate { get; set; }

        public string PlanName { get; set; }
        public DateTime? SubscriptionEndDate { get; set; }

        public string NextTrainingTitle { get; set; }
        public DateTime? NextTrainingDate { get; set; }
        public string NextTrainingLocation { get; set; }
        public int UpcomingTrainingsCount { get; set; }

        public DateTime? LastSessionTime { get; set; }
        public double? TotalDistanceKM { get; set; }
        public double? MaxSpeed { get; set; }
        public double? AvgHeartRate { get; set; }

        public PlayerOverviewDTO(string teamName, string coachName, string userRole, DateTime userJoinedDate, string planName, DateTime? subscriptionEndDate, string nextTrainingTitle, DateTime? nextTrainingDate, string nextTrainingLocation, int upcomingTrainingsCount, DateTime? lastSessionTime, double? totalDistanceKM, double? maxSpeed, double? avgHeartRate)
        {
            TeamName = teamName;
            CoachName = coachName;
            UserRole = userRole;
            UserJoinedDate = userJoinedDate;
            PlanName = planName;
            SubscriptionEndDate = subscriptionEndDate;
            NextTrainingTitle = nextTrainingTitle;
            NextTrainingDate = nextTrainingDate;
            NextTrainingLocation = nextTrainingLocation;
            UpcomingTrainingsCount = upcomingTrainingsCount;
            LastSessionTime = lastSessionTime;
            TotalDistanceKM = totalDistanceKM;
            MaxSpeed = maxSpeed;
            AvgHeartRate = avgHeartRate;
            {
            }
        }
        public PlayerOverviewDTO() { }
    }
}

