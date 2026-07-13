using SeenCL.Domain.Entities;
using SeenCL.DTOs.Coaching;
using SeenCL.Repositories;
using SeenCL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeenBLL.Services
{
    public class CoachService : ICoachService
    {
        private readonly ITeamRepository _teamRepository;
        private readonly ITeamMemberRepository _teamMemberRepository;
        private readonly IUserRepository _userRepository;
        private readonly ITrainingProgramRepository _programRepository;

        public CoachService(
            ITeamRepository teamRepository,
            ITeamMemberRepository teamMemberRepository,
            IUserRepository userRepository,
            ITrainingProgramRepository programRepository)
        {
            _teamRepository = teamRepository;
            _teamMemberRepository = teamMemberRepository;
            _userRepository = userRepository;
            _programRepository = programRepository;
        }

        public async Task<IEnumerable<CoachTeamResponseDTO>> GetTeamsForCoachAsync(int coachId)
        {
            var teams = await Task.FromResult(_teamRepository.GetByCoachId(coachId));
            return teams.Select(t => new CoachTeamResponseDTO
            {
                TeamID = t.TeamID,
                TeamName = t.TeamName,
                TeamCode = t.TeamCode,
                EndDate = t.EndDate,
                IsActive = t.IsActive,
                MemberCount = _teamMemberRepository.GetByTeamId(t.TeamID).Count()
            });
        }

        public async Task<PlayerDashboardDTO?> GetPlayerDashboardAsync(int playerId)
        {
            var user = await Task.FromResult(_userRepository.GetById(playerId));
            if (user == null) return null;

            var dashboard = new PlayerDashboardDTO
            {
                PlayerID = user.UserID,
                FullName = $"{user.FirstName} {user.LastName}",
                Height = user.Height,
                Weight = user.Weight,
                FateRate = user.FateRate
            };

            // Get recent programs for this player across all their teams
            // (Assuming we can find them via TeamMemberID or TeamID)
            // For simplicity, we search programs by team where they are a member
            var memberTeams = _teamMemberRepository.GetAll().Where(m => m.PlayerID == playerId);
            foreach (var mt in memberTeams)
            {
                var programs = _programRepository.GetByTeamId(mt.TeamID)
                    .OrderByDescending(p => p.CreatedAt)
                    .Take(5);

                foreach (var p in programs)
                {
                    dashboard.RecentPrograms.Add(new RecentProgramDTO
                    {
                        ProgramID = p.ProgramID,
                        ProgramName = p.ProgramName,
                        Status = p.Status,
                        EndDate = p.EndDate
                    });
                }
            }

            return dashboard;
        }

        public async Task<int> CreateTrainingProgramAsync(TrainingProgramCreateDTO dto)
        {
            // Find the TeamMemberID for this player in this team
            var member = _teamMemberRepository.GetByTeamId(dto.TeamID)
                .FirstOrDefault(m => m.PlayerID == dto.PlayerID);

            if (member == null) return -1;

            var program = new TrainingProgram
            {
                TeamID = dto.TeamID,
                TeamMemberID = member.MemberID,
                ProgramName = dto.ProgramName,
                Goal = dto.Goal,
                IntensityLevel = dto.IntensityLevel,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                Status = SeenCL.Domain.Enums.ProgramStatus.Active,
                Notes = dto.Notes,
                CreatedAt = DateTime.UtcNow
            };

            return await Task.FromResult(_programRepository.Create(program));
        }
    }
}
