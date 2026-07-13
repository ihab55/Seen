using Microsoft.AspNetCore.Mvc;
using SeenCL.DTOs;
using SeenCL.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace SeenAPI.Controllers
{
    /// <summary>
    /// Controller for managing coach application requests and admin approvals.
    /// </summary>
    [Route("api/coach-approvals")]
    [ApiController]
    public class CoachApprovalController : ControllerBase
    {
        private readonly ICoachApprovalService _approvalService;

        public CoachApprovalController(ICoachApprovalService approvalService)
        {
            _approvalService = approvalService;
        }

        /// <summary>
        /// WHO: User.
        /// WHAT: Submits a request to become a coach.
        /// </summary>
        [HttpPost("request")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> RequestApproval([FromBody] CoachApprovalDTO dto)
        {
            var id = await _approvalService.RequestApprovalAsync(dto);
            if (id <= 0) return BadRequest(new { message = "Failed to submit request" });
            return Ok(new { ApprovalID = id, message = "Application submitted successfully" });
        }

        /// <summary>
        /// WHO: Admin.
        /// WHAT: Approves or rejects a coach application.
        /// </summary>
        [HttpPost("{id:int}/process")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Process(int id, [FromQuery] int adminId, [FromQuery] bool accept)
        {
            if (await _approvalService.ProcessApprovalAsync(id, adminId, accept)) return Ok(new { message = "Request processed successfully" });
            return BadRequest(new { message = "Failed to process request" });
        }

        /// <summary>
        /// WHO: User.
        /// WHAT: Retrieves the current status of a user's coach application.
        /// </summary>
        [HttpGet("user/{userId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CoachApprovalDTO>> GetUserRequest(int userId)
        {
            var request = await _approvalService.GetUserRequestAsync(userId);
            if (request == null) return NotFound(new { message = "No request found for this user" });
            return Ok(request);
        }

        /// <summary>
        /// WHO: Admin.
        /// WHAT: Lists all coach applications for review.
        /// </summary>
        [HttpGet("All")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<CoachApprovalDTO>>> GetAll()
        {
            var requests = await _approvalService.GetAllRequestsAsync();
            return Ok(requests);
        }
    }
}
