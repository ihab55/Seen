using SeenCL.Domain.Entities;
using SeenCL.DTOs;
using SeenCL.Interfaces;

namespace SeenCL.Repositories
{
    public interface ITeamMemberRepository : IRepository<TeamMember, int>
    {
        IEnumerable<TeamMember> GetByTeamId(int teamId);
        IEnumerable<TeamMember> GetAll();
        IEnumerable<TeamMemberRosterRowDTO> GetRosterByTeamForPlayer(int teamId, int playerId);
        int Create(AddTeamMemberDTO entity);
    }
}
