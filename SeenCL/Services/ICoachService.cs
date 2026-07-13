using SeenCL.DTOs.Coaching;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SeenCL.Services
{
    public interface ICoachService
    {
        Task<IEnumerable<CoachTeamResponseDTO>> GetTeamsForCoachAsync(int coachId);
        Task<PlayerDashboardDTO?> GetPlayerDashboardAsync(int playerId);
        Task<int> CreateTrainingProgramAsync(TrainingProgramCreateDTO dto);
    }
}
