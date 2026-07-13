using SeenCL.Domain.Entities;
using SeenCL.DTOs;
using SeenCL.Repositories;
using SeenCL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeenBLL.Services
{
    public class TeamService : ITeamService
    {

        private readonly ITeamRepository _repository;

        public TeamService(ITeamRepository repository)
        {
            _repository = repository;
        }

        public async Task<int> CreateTeamAsync(TeamCreateDTO dto)
        {
            
            return await Task.FromResult(_repository.Create(dto));
        }

        public async Task<TeamDTO?> GetTeamByCodeAsync(string code)
        {
            var team = await Task.FromResult(_repository.GetByCode(code));
            return team != null ? MapToDTO(team) : null;
        }
        public async Task<TeamDTO?> GetTeamByIDAsync(int ID)
        {
            Team? team = await Task.FromResult(_repository.GetById(ID));
            return team != null ? MapToDTO(team) : null;
        }
        public async Task<PlayerOverviewDTO?> GetPlayerOverview(int teamId, int userId)
        {
            var overview = await Task.FromResult(_repository.GetPlayerOverview(teamId, userId));
            return overview;
        }
        public async Task<IEnumerable<TeamDTO>> GetAllTeamsAsync()
        {
            var teams = await Task.FromResult(_repository.GetAll());
            return teams.Select(MapToDTO);
        }
        public async Task<IEnumerable<PlayerTeamViewDTO>> GetTeamsByUserIdAsync(int userId)
        {
            return await Task.FromResult(_repository.GetByUserId(userId));
        }
        public async Task <IEnumerable<CoachTeamListDTO>> GetCoachTeams(int coachID)
        {
            return await Task.FromResult(_repository.GetCoachTeams(coachID));
        }
        private TeamDTO MapToDTO(Team t)
        {
            return new TeamDTO(
                t.TeamID,
                t.CoachID,
                t.TeamName,
                t.TeamCode,
                t.SubscriptionID,
                t.StartDate,
                t.EndDate,
                t.IsActive
            );
        }
    }
}
