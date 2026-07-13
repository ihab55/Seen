using SeenCL.Domain.Enums;
using System;
using System.Collections.Generic;

namespace SeenCL.DTOs.Coaching
{
    public class CoachTeamResponseDTO
    {
        public int TeamID { get; set; }
        public string TeamName { get; set; } = string.Empty;
        public string TeamCode { get; set; } = string.Empty;
        public int MemberCount { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
    }

    public class TeamPlayerResponseDTO
    {
        public int PlayerID { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public bool IsInjured { get; set; }
        public string? ImagePath { get; set; }
        /// <summary>Same as ImagePath when returned from the API; helps mobile clients that read ImageUrl.</summary>
        public string? ImageUrl { get; set; }
        public DateTime JoinedAt { get; set; }
        public bool IsProfileCompleted { get; set; }
        public bool IsCoach { get; set; }
        public string Status { get; set; } = "Active";
        public int JerseyNumber { get; set; }
        public string Position { get; set; } = string.Empty;
    }

    public class PlayerDashboardDTO
    {
        public int PlayerID { get; set; }
        public string FullName { get; set; } = string.Empty;
        public int? Height { get; set; }
        public int? Weight { get; set; }
        public double? FateRate { get; set; }
        public List<RecentProgramDTO> RecentPrograms { get; set; } = new();
    }

    public class RecentProgramDTO
    {
        public int ProgramID { get; set; }
        public string ProgramName { get; set; } = string.Empty;
        public ProgramStatus Status { get; set; }
        public DateTime EndDate { get; set; }
    }

    public class TrainingProgramCreateDTO
    {
        public int TeamID { get; set; }
        public int PlayerID { get; set; }
        public string ProgramName { get; set; } = string.Empty;
        public string Goal { get; set; } = string.Empty;
        public byte IntensityLevel { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? Notes { get; set; }
    }
}
