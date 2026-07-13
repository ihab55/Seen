using SeenCL.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SeenCL.Services
{
    public interface ITeamService
    {
        Task<int> CreateTeamAsync(TeamCreateDTO dto);
        Task<TeamDTO?> GetTeamByCodeAsync(string code);
        Task<IEnumerable<TeamDTO>> GetAllTeamsAsync();
        Task<IEnumerable<PlayerTeamViewDTO>> GetTeamsByUserIdAsync(int userId);
        Task<TeamDTO?> GetTeamByIDAsync(int ID);
        Task<PlayerOverviewDTO?> GetPlayerOverview(int teamId, int userId);
        Task<IEnumerable<CoachTeamListDTO>> GetCoachTeams(int coachID);
    }
}
