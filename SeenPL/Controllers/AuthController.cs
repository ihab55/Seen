using Microsoft.AspNetCore.Mvc;
using SeenCL.DTOs.Auth;
using SeenCL.DTOs;
using SeenCL.Services;
using System.Threading.Tasks;

namespace SeenPL.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Authenticates a user and returns JWT tokens.
        /// </summary>
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<AuthResponseDTO>> Login([FromBody] UserLoginDTO dto)
        {
            if (dto == null) return BadRequest("Login data is required.");

            var result = await _authService.LoginAsync(dto);
            if (result == null) return Unauthorized("Invalid credentials");

            return Ok(result);
        }

        /// <summary>
        /// Authenticates an admin and returns JWT tokens.
        /// </summary>
        [HttpPost("admin/login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<AdminAuthResponseDTO>> AdminLogin([FromBody] AdminLoginRequestDTO dto)
        {
            if (dto == null) return BadRequest("Login data is required.");

            var result = await _authService.AdminLoginAsync(dto);
            if (result == null) return Unauthorized("Invalid credentials");

            return Ok(result);
        }

        /// <summary>
        /// Refreshes an access token using a refresh token.
        /// </summary>
        [HttpPost("refresh")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<AuthResponseDTO>> RefreshToken([FromBody] RefreshTokenRequestDTO dto)
        {
            if (dto == null || string.IsNullOrEmpty(dto.RefreshToken))
                return BadRequest("Refresh token is required.");

            var result = await _authService.RefreshTokenAsync(dto.RefreshToken);
            if (result == null) return Unauthorized("Invalid or expired refresh token");

            return Ok(result);
        }

        /// <summary>
        /// Revokes a refresh token (logout).
        /// </summary>
        [HttpPost("revoke")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> RevokeToken([FromBody] RefreshTokenRequestDTO dto)
        {
            if (dto == null || string.IsNullOrEmpty(dto.RefreshToken))
                return BadRequest("Refresh token is required.");

            var result = await _authService.RevokeTokenAsync(dto.RefreshToken);
            if (result) return Ok("Token revoked successfully");

            return BadRequest("Failed to revoke token");
        }
    }
}
