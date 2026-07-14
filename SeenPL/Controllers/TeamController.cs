using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SeenCL.DTOs;
using SeenCL.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeenAPI.Controllers
{
    /// <summary>
    /// Controller for managing teams, allowing creation, lookup by code, and listing.
    /// </summary>
    [Route("api/teams")]
    [ApiController]
    public class TeamController : ControllerBase
    {
        private readonly ITeamService _teamService;

        public TeamController(ITeamService teamService)
        {
            _teamService = teamService;
        }

        /// <summary>
        /// WHO: Admin / Coach.
        /// WHAT: Creates a new sports team.
        /// </summary>
        [HttpPost("Create")]
        [Authorize(Roles = "Coach,Admin")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<TeamDTO>> Create([FromBody] TeamCreateDTO dto)
        {
            var id = await _teamService.CreateTeamAsync(dto);
            if (id <= 0) return BadRequest(new { message = "Failed to create team" });

            // Note: In a real scenario, we'd fetch the generated code to return
            return StatusCode(StatusCodes.Status201Created, new { TeamID = id, message = "Team created successfully" });
        }

        /// <summary>
        /// WHO: Athlete / Coach.
        /// WHAT: Retrieves team details using a unique team code.
        /// </summary>
        [HttpGet("code/{code}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TeamDTO>> GetByCode(string code)
        {
            var team = await _teamService.GetTeamByCodeAsync(code);
            if (team == null) return NotFound(new { message = "Team not found with this code" });
            return Ok(team);
        }

        /// <summary>
        /// WHO: Athlete / Coach.
        /// WHAT: Retrieves team details using a unique team code.
        /// </summary>
        [HttpGet("TeamID/{ID}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TeamDTO>> GetByID(int ID)
        {
            var team = await _teamService.GetTeamByIDAsync(ID);
            if (team == null) return NotFound(new { message = "Team not found with this code" });
            return Ok(team);
        }

        /// <summary>
        /// WHO: Admin.
        /// WHAT: Lists all teams in the system.
        /// </summary>
        [HttpGet("All")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<TeamDTO>>> GetAll()
        {
            var teams = await _teamService.GetAllTeamsAsync();
            return Ok(teams);
        }

        /// <summary>
        /// WHO: Player/Athlete.
        /// WHAT: Retrieves all teams where the authenticated user is a member (player).
        /// Includes team info with coach name and membership date.
        /// </summary>
        /// <param name="userId">The user ID of the player</param>
        /// <returns>List of teams with coach details and join date</returns>
        [HttpGet("my-teams/{userId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PlayerTeamViewDTO>> GetMyTeams(int userId)
        {
            if (userId <= 0)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Invalid user ID provided",
                    errorCode = "INVALID_USER_ID"
                });
            }

            IEnumerable<PlayerTeamViewDTO> teams = await _teamService.GetTeamsByUserIdAsync(userId);

            if (teams == null)
            {
                return NotFound
                    ("No teams found for this user. You may not be enrolled in any teams yet.");
            }

            return Ok(teams);
        }

        /// <summary>
        /// WHO: Player/Athlete.
        /// WHAT: Returns full overview data for a selected team and player.
        /// WHY: Used by the mobile Overview tab after a team is selected.
        /// </summary>
        [HttpGet("player-overview")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetPlayerOverview(int teamId, int userId)
        {
            if (teamId <= 0 || userId <= 0)
                return BadRequest("Invalid teamId or userId");

            var result = await _teamService.GetPlayerOverview(teamId, userId);

            if (result == null)
                return NotFound("No data found for this player in the team");

            return Ok(result);
        }

        /// <summary>
        /// List a coach's teams, showing team name, code, and subscription status. 
        /// </summary>
        /// <param name="coachID"></param>
        /// <returns></returns>
        [HttpGet("Coachingteams/{coachID}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetCoachTeams(int coachID)
        {
            if (coachID <= 0)
            {
                return BadRequest("Invalid CoachId");
            }

            var teams = await _teamService.GetCoachTeams(coachID);

            return Ok(teams);
        }
    }
}
