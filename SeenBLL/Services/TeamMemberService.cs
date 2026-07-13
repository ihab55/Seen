using SeenCL.Domain.Entities;
using SeenCL.DTOs;
using SeenCL.DTOs.Coaching;
using SeenCL.Repositories;
using SeenCL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace SeenBLL.Services
{
    public class TeamMemberService : ITeamMemberService
    {
        private readonly ITeamMemberRepository _repository;
        private readonly ITeamRepository _teamRepository;
        private readonly INotificationService _notificationService;
        private readonly IConfiguration _configuration;

        public enum TeamMemberStatus
        {
            Accepted = 1,
            Rejected = 2,
            Pending = 3
        }

        public TeamMemberService(
            ITeamMemberRepository repository,
            ITeamRepository teamRepository,
            INotificationService notificationService,
            IConfiguration configuration)
        {
            _repository            = repository;
            _teamRepository        = teamRepository;
            _notificationService   = notificationService;
            _configuration         = configuration;
        }

        public async Task<bool> AddTeamAsync(AddTeamMemberDTO dto)
        {
            var created = await Task.FromResult(_repository.Create(dto) > 0);

            if (created)
            {
                // Notify the player that they were added to the team
                var team = await Task.FromResult(_teamRepository.GetById(dto.TeamID));
                var teamName = team?.TeamName ?? "a team";

                await _notificationService.CreateNotificationAsync(
                    userId:   dto.PlayerID,
                    title:    "Team Invitation",
                    body:     $"You have been added to \"{ teamName }\" by your coach.",
                    targetId: dto.TeamID
                );
            }

            return created;
        }

        public async Task<IEnumerable<TeamMemberResponseDTO>> GetTeamMembersAsync(int teamId)
        {
            var members = await Task.FromResult(_repository.GetByTeamId(teamId));
            return members.Select(m => new TeamMemberResponseDTO
            {
                MemberID = m.MemberID,
                TeamID = m.TeamID,
                PlayerID = m.PlayerID,
                JoinedAt = m.JoinedAt,
                IsCoach = m.IsCoach
            });
        }

        public async Task<IEnumerable<TeamPlayerResponseDTO>> GetTeamRosterForPlayerAsync(int teamId, int playerId)
        {
            var rows = await Task.FromResult(_repository.GetRosterByTeamForPlayer(teamId, playerId));
            return rows.Select(ToTeamPlayerDto);
        }


        private TeamPlayerResponseDTO ToTeamPlayerDto(TeamMemberRosterRowDTO r)
        {
            var dto = new TeamPlayerResponseDTO
            {
                PlayerID = r.PlayerID,
                FullName = r.FullName,
                UserName = r.UserName,
                IsInjured = r.IsInjured,
                ImagePath = r.ImagePath,
                ImageUrl = r.ImagePath,
                JoinedAt = r.JoinedAt,
                IsProfileCompleted = r.IsProfileCompleted,
                IsCoach = r.IsCoach,
                Status = r.Status,
                JerseyNumber = r.JerseyNumber,
                Position = r.Position
            };

            return dto;
        }
    }
}
