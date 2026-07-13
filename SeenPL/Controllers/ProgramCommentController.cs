using Microsoft.AspNetCore.Mvc;
using SeenCL.DTOs;
using SeenCL.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace SeenAPI.Controllers
{
    /// <summary>
    /// Controller for managing comments on training programs, facilitating feedback between coaches and athletes.
    /// </summary>
    [Route("api/comments")]
    [ApiController]
    public class ProgramCommentController : ControllerBase
    {
        private readonly IProgramCommentService _commentService;

        public ProgramCommentController(IProgramCommentService commentService)
        {
            _commentService = commentService;
        }

        /// <summary>
        /// WHO: Coach / Athlete.
        /// WHAT: Adds a feedback comment to a specific training program.
        /// </summary>
        [HttpPost("Add")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ProgramCommentDTO>> AddComment([FromBody] ProgramCommentDTO dto)
        {
            var id = await _commentService.AddCommentAsync(dto);
            if (id <= 0) return BadRequest(new { message = "Failed to add comment" });
            
            dto.CommentID = id;
            return StatusCode(StatusCodes.Status201Created, dto);
        }

        /// <summary>
        /// WHO: Coach / Athlete.
        /// WHAT: Retrieves the comment thread for a specific training program.
        /// </summary>
        [HttpGet("program/{programId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ProgramCommentDTO>>> GetComments(int programId)
        {
            var comments = await _commentService.GetCommentsForProgramAsync(programId);
            return Ok(comments);
        }
    }
}
