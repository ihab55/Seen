using System;
using SeenCL.Domain.Enums;

namespace SeenCL.Domain.Entities
{
    public class TrainingProgram
    {
        public int ProgramID { get; set; }
        public int TeamID { get; set; }
        public int TeamMemberID { get; set; }
        public string ProgramName { get; set; } = string.Empty;
        public string Goal { get; set; } = string.Empty;
        public byte IntensityLevel { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public ProgramStatus Status { get; set; }
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
