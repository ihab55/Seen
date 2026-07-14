using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SeenCL.DTOs;
using SeenCL.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace SeenAPI.Controllers
{
    /// <summary>
    /// Controller for system administrators to manage other admins and system-wide settings.
    /// </summary>
    [Route("api/admins")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        /// <summary>
        /// WHO: Admin.
        /// WHAT: Login to the admin dashboard.
        /// </summary>
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<AdminResponseDTO>> Login([FromBody] AdminLoginRequestDTO request)
        {
            var admin = await _adminService.LoginAsync(request);
            if (admin == null) return Unauthorized(new { message = "Invalid email or password" });
            return Ok(admin);
        }

        /// <summary>
        /// WHO: Super Admin.
        /// WHAT: Creates a new administrator account.
        /// </summary>
        [HttpPost("Create")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<int>> Create([FromBody] AdminDTO dto)
        {
            var id = await _adminService.CreateAdminAsync(dto);
            if (id <= 0) return BadRequest(new { message = "Failed to create admin" });
            return StatusCode(StatusCodes.Status201Created, id);
        }

        /// <summary>
        /// WHO: Super Admin.
        /// WHAT: Updates an administrator's profile.
        /// </summary>
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Update(int id, [FromBody] AdminDTO dto)
        {
            if (await _adminService.UpdateAdminAsync(id, dto)) return Ok(new { message = "Admin updated successfully" });
            return BadRequest(new { message = "Failed to update admin" });
        }

        /// <summary>
        /// WHO: Super Admin.
        /// WHAT: Lists all administrators.
        /// </summary>
        [HttpGet("All")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<AdminResponseDTO>>> GetAll()
        {
            var admins = await _adminService.GetAllAdminsAsync();
            return Ok(admins);
        }
    }
}
