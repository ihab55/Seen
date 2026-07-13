using Microsoft.AspNetCore.Mvc;
using SeenCL.DTOs;
using SeenCL.DTOs.Coaching;
using SeenCL.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace SeenAPI.Controllers
{
    /// <summary>
    /// Controller for managing team membership, allowing players to join teams via codes.
    /// </summary>
    [Route("api/team-members")]
    [ApiController]
    public class TeamMemberController : ControllerBase
    {
        private readonly ITeamMemberService _memberService;
        public TeamMemberController(ITeamMemberService memberService)
        {
            _memberService = memberService;
        }

        /// <summary>
        /// WHO: Athlete / User.
        /// WHAT: Joins a team using a unique team code.
        /// </summary>
        [HttpPost("Add")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Add([FromBody] AddTeamMemberDTO dto)
        {
            if (await _memberService.AddTeamAsync(dto)) 
                return Ok(new { message = "Successfully joined the team" });
            return BadRequest(new { message = "Could not join team. Check code or user status." });
        }

        /// <summary>
        /// WHO: Authenticated team member (player).
        /// WHAT: Full roster with profile fields (including image path) when the requesting player belongs to the team.
        /// Uses database stored procedure SP_TeamMembers_GetByPlayer.
        /// </summary>
        [HttpGet("team/{teamId:int}/player/{playerId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<TeamPlayerResponseDTO>>> GetTeamRosterForPlayer(int teamId, int playerId)
        {
            var members = await _memberService.GetTeamRosterForPlayerAsync(teamId, playerId);
            return Ok(members);
        }

        /// <summary>
        /// WHO: Coach / Admin.
        /// WHAT: Retrieves a list of all members in a specific team.
        /// </summary>
        [HttpGet("team/{teamId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<TeamMemberResponseDTO>>> GetTeamMembers(int teamId)
        {
            var members = await _memberService.GetTeamMembersAsync(teamId);
            return Ok(members);
        }
    }
}
