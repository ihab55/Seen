namespace SeenCL.DTOs
{
    public enum enProgramStatus { Active = 0, Completed = 1, Cancelled = 2 }
    public class TrainingProgramDTO
    {
        public int ProgramID { get; set; }
        public int TeamID { get; set; }
        public int TeamMemberID { get; set; }
        public string ProgramName { get; set; }
        public string Goal { get; set; }
        public byte IntensityLevel { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public enProgramStatus Status { get; set; }
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; }

        // ????? ?????? ????? ??? (???? ?? ??? JOIN ?? ??? SP)
        public string? CreatedByMemberName { get; set; }

        public TrainingProgramDTO(int ProgramID, int TeamID, int TeamMemberID, string ProgramName, string Goal,
            byte IntensityLevel, DateTime StartDate, DateTime EndDate, enProgramStatus Status, string? Notes, DateTime CreatedAt)
        {
            this.ProgramID = ProgramID;
            this.TeamID = TeamID;
            this.TeamMemberID = TeamMemberID;
            this.ProgramName = ProgramName;
            this.Goal = Goal;
            this.IntensityLevel = IntensityLevel;
            this.StartDate = StartDate;
            this.EndDate = EndDate;
            this.Status = Status;
            this.Notes = Notes;
            this.CreatedAt = CreatedAt;
        }
    }
}
