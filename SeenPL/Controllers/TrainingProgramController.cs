using Microsoft.AspNetCore.Mvc;
using SeenCL.DTOs;
using SeenCL.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace SeenAPI.Controllers
{
    /// <summary>
    /// Controller for managing training programs assigned to athletes by coaches.
    /// </summary>
    [Route("api/training-programs")]
    [ApiController]
    public class TrainingProgramController : ControllerBase
    {
        private readonly ITrainingProgramService _programService;

        public TrainingProgramController(ITrainingProgramService programService)
        {
            _programService = programService;
        }

        /// <summary>
        /// WHO: Coach.
        /// WHAT: Assigns a new training program to an athlete.
        /// </summary>
        [HttpPost("Create")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<TrainingProgramDTO>> Create([FromBody] TrainingProgramDTO dto)
        {
            var id = await _programService.CreateProgramAsync(dto);
            if (id <= 0) return BadRequest(new { message = "Failed to assign program" });
            
            dto.ProgramID = id;
            return StatusCode(StatusCodes.Status201Created, dto);
        }

        /// <summary>
        /// WHO: Coach / Athlete.
        /// WHAT: Retrieves all training programs active for a specific team.
        /// </summary>
        [HttpGet("team/{teamId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<TrainingProgramDTO>>> GetTeamPrograms(int teamId)
        {
            var programs = await _programService.GetTeamProgramsAsync(teamId);
            return Ok(programs);
        }
    }
}
