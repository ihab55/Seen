using Microsoft.AspNetCore.Mvc;
using SeenCL.DTOs.Coaching;
using SeenCL.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace SeenPL.Controllers
{
    [Route("api/coaching")]
    [ApiController]
    public class CoachController : ControllerBase
    {
        private readonly ICoachService _coachService;
        private readonly ITeamMemberService _teamMemberService;

        public CoachController(ICoachService coachService, ITeamMemberService teamMemberService)
        {
            _coachService = coachService;
            _teamMemberService = teamMemberService;
        }

        /// <summary>
        /// Retrieves all teams managed by a specific coach.
        /// WHO: Coach.
        /// WHAT: Returns a list of teams with member counts.
        /// </summary>
        [HttpGet("coach/{coachId:int}/teams")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<CoachTeamResponseDTO>>> GetCoachTeams(int coachId)
        {
            var teams = await _coachService.GetTeamsForCoachAsync(coachId);
            return Ok(teams);
        }

        /// <summary>
        /// Retrieves a player's performance dashboard.
        /// WHO: Coach.
        /// WHAT: Returns stats and recent training programs for a player.
        /// </summary>
        [HttpGet("player/{playerId:int}/dashboard")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PlayerDashboardDTO>> GetPlayerDashboard(int playerId)
        {
            var dashboard = await _coachService.GetPlayerDashboardAsync(playerId);
            if (dashboard == null) return NotFound();
            return Ok(dashboard);
        }

        /// <summary>
        /// Assigns a new training program to a player in a team.
        /// WHO: Coach.
        /// WHAT: Creates a training program record and returns the ID.
        /// </summary>
        [HttpPost("program/create")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<int>> CreateProgram([FromBody] TrainingProgramCreateDTO dto)
        {
            var id = await _coachService.CreateTrainingProgramAsync(dto);
            if (id <= 0) return BadRequest("Failed to create training program. Ensure player is a member of the team.");
            return Created("", id);
        }
    }
}
