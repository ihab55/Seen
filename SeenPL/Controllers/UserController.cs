using Microsoft.AspNetCore.Mvc;
using SeenCL.Domain.Entities;
using SeenCL.Services;
using SeenCL.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Http;

namespace SeenPL.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly Microsoft.Extensions.Configuration.IConfiguration _configuration;

        public UserController(IUserService userService, Microsoft.Extensions.Configuration.IConfiguration configuration)
        {
            _userService = userService;
            _configuration = configuration;
        }

        /// <summary>
        /// Retrieves all user responses for administrative purposes.
        /// WHO: Admin.
        /// WHAT: Returns a full list of user details formatted for administration.
        /// </summary>
        [HttpGet("All", Name = "GetAllUsers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<UserAdminResponseDTO>>> GetAll()
        {
            var users = await _userService.GetAllUserResponsesAsync();
            if (users == null) return StatusCode(500, "Something went wrong while connecting to admin data.");
            return Ok(users);
        }

        /// <summary>
        /// Retrieves a specific user's response profile by their ID.
        /// WHO: User / Mobile App.
        /// WHAT: Returns user profile details. NOTE: Does not load heavy image binary data to optimize performance.
        /// </summary>
        [HttpGet("{id:int}", Name = "GetById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserResponseDTO>> GetById(int id)
        {
            if (id <= 0) return BadRequest("Invalid user ID");

            var user = await _userService.GetUserResponseByIdAsync(id);
            if (user == null) return NotFound();
            return Ok(user);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Username"></param>
        /// <returns></returns>
        [HttpGet("Username/{Username}", Name = "GetByUserName")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserResponseDTO>> GetByUserName(string Username)
        {
            if (string.IsNullOrEmpty(Username)) return BadRequest("Email is required");
            var user = await _userService.GetUserResponseByUsernameAsync(Username);
            if (user == null) return NotFound();
            return Ok(user);
        }

        /// <summary>
        /// Authenticates a user based on username/email and password.
        /// WHO: User.
        /// WHAT: Validates credentials and returns the user's basic profile DTO if successful.
        /// </summary>
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UserResponseDTO>> Login([FromBody] UserLoginDTO dto)
        {
            if (dto == null) return BadRequest("Login data is required.");

            var user = await _userService.LoginAsync(dto);
            if (user == null) return Unauthorized("Invalid credentials");
            return Ok(user);
        }

        /// <summary>
        /// Registers a new user in the system.
        /// WHO: New User.
        /// WHAT: Creates a new user record and returns the created user's response profile.
        /// </summary>
        [HttpPost("Create")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UserResponseDTO>> Create([FromBody] UserCreateDTO dto)
        {
            if (dto == null) return BadRequest("User data is required.");

            var id = await _userService.CreateUserAsync(dto);
            if (id <= 0) return BadRequest("Failed to create user");

            var user = await _userService.GetUserResponseByIdAsync(id);
            return CreatedAtAction(nameof(GetById), new { id = id }, user);
        }

        /// <summary>
        /// Updates an existing user's profile information.
        /// WHO: User.
        /// WHAT: Updates specific profile fields (height, weight, etc.) and handles image upload.
        /// NOTE: Uses [FromForm] to support multipart/form-data for image uploads from the mobile app.
        /// </summary>
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserResponseDTO>> Update(int id, [FromForm] UserProfileUpdateDTO dto)
        {
            if (id <= 0) return BadRequest("Invalid user ID");
            if (dto == null) return BadRequest("Update data is required.");

            if (await _userService.UpdateUserProfileAsync(id, dto))
            {
                var updatedUser = await _userService.GetUserResponseByIdAsync(id);
                return Ok(updatedUser);
            }
            return BadRequest("Update failed. User may not exist or data is invalid.");
        }

        /// <summary>
        /// Performs a soft delete on a user record.
        /// WHO: Admin / User.
        /// WHAT: Marks a user as deleted without removing them from the physical database.
        /// </summary>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Delete(int id)
        {
            if (id <= 0) return BadRequest("Invalid user ID");

            if (await _userService.DeleteUserAsync(id)) return NoContent();
            return BadRequest("Delete failed. User may not exist.");
        }

        /// <summary>
        /// Links a specific hardware device to a user.
        /// WHO: User / Technical Support.
        /// WHAT: Associates a device ID with the user's account for tracking.
        /// </summary>
        [HttpPost("{id:int}/device/{deviceId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> AssignDevice(int id, int deviceId)
        {
            if (id <= 0 || deviceId <= 0) return BadRequest("Invalid ID parameters.");

            if (await _userService.AssignDeviceAsync(id, deviceId)) return Ok("Device assigned successfully.");
            return BadRequest("Assignment failed. Check if user or device exists.");
        }

        /// <summary>
        /// Unlinks the current hardware device from a user.
        /// WHO: User / Technical Support.
        /// WHAT: Removes the device association from the user's account.
        /// </summary>
        [HttpDelete("{id:int}/device")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> RemoveDevice(int id)
        {
            if (id <= 0) return BadRequest("Invalid user ID.");

            if (await _userService.RemoveDeviceAsync(id)) return Ok("Device removed successfully.");
            return BadRequest("Removal failed. User may not have an assigned device.");
        }

        /// <summary>
        /// Retrieves the image file for a user.
        /// </summary>
        [HttpGet("image/{**fileName}")]
        public IActionResult GetUserImage(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) return BadRequest("File name is required.");

            var folderPath = _configuration["FileSettings:SeenImagePath"];
            if (string.IsNullOrEmpty(folderPath)) return NotFound("Image folder not configured.");

            var fullPath = System.IO.Path.Combine(folderPath, fileName);
            if (!System.IO.File.Exists(fullPath))
            {
                // Fallback for .jpg vs .jpeg extension mismatch between DB and disk
                var withoutExt = System.IO.Path.GetFileNameWithoutExtension(fileName);
                var ext = System.IO.Path.GetExtension(fileName).ToLowerInvariant();

                if (ext == ".jpg") fullPath = System.IO.Path.Combine(folderPath, withoutExt + ".jpeg");
                else if (ext == ".jpeg") fullPath = System.IO.Path.Combine(folderPath, withoutExt + ".jpg");

                if (!System.IO.File.Exists(fullPath)) return NotFound("Image not found.");
            }

            var stream = new System.IO.FileStream(fullPath, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read);
            var actualExt = System.IO.Path.GetExtension(fullPath).ToLowerInvariant();
            var contentType = actualExt switch
            {
                ".png" => "image/png",
                ".gif" => "image/gif",
                ".webp" => "image/webp",
                _ => "image/jpeg"
            };

            return File(stream, contentType);
        }
    }
}
