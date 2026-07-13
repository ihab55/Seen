using SeenCL.Domain.Entities;
using SeenCL.DTOs;
using System.Collections.Generic;
using SeenCL.Interfaces;

namespace SeenCL.Repositories
{
    public interface ITeamRepository : IRepository<Team, int>
    {
        Team? GetByCode(string code);
        IEnumerable<Team> GetAll();
        IEnumerable<Team> GetByCoachId(int coachId);
        IEnumerable<PlayerTeamViewDTO> GetByUserId(int userId);
        PlayerOverviewDTO? GetPlayerOverview(int teamId, int userId);
        IEnumerable<CoachTeamListDTO> GetCoachTeams(int coachID);
        int Create(TeamCreateDTO entity);
    }
}
