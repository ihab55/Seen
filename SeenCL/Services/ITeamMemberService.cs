using SeenCL.DTOs;
using SeenCL.DTOs.Coaching;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SeenCL.Services
{
    public interface ITeamMemberService
    {
        Task<bool> AddTeamAsync(AddTeamMemberDTO dto);
        Task<IEnumerable<TeamMemberResponseDTO>> GetTeamMembersAsync(int teamId);
        Task<IEnumerable<TeamPlayerResponseDTO>> GetTeamRosterForPlayerAsync(int teamId, int playerId);
    }
}
