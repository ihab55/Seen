using Microsoft.AspNetCore.Mvc;
using SeenCL.DTOs;
using SeenCL.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace SeenAPI.Controllers
{
    /// <summary>
    /// Controller for managing hardware devices registered in the system.
    /// </summary>
    [Route("api/devices")]
    [ApiController]
    public class DeviceController : ControllerBase
    {
        private readonly IDeviceService _deviceService;

        public DeviceController(IDeviceService deviceService)
        {
            _deviceService = deviceService;
        }

        /// <summary>
        /// WHO: Hardware / Admin.
        /// WHAT: Registers a new device using its serial number and metadata.
        /// </summary>
        [HttpPost("Register")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<int>> Register([FromBody] DeviceDTO dto)
        {
            var id = await _deviceService.RegisterDeviceAsync(dto);
            if (id <= 0) return BadRequest(new { message = "Failed to register device" });
            return StatusCode(StatusCodes.Status201Created, id);
        }

        /// <summary>
        /// WHO: Admin / Hardware.
        /// WHAT: Retrieves device details using its unique serial number.
        /// </summary>
        [HttpGet("serial/{serial}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<DeviceDTO>> GetBySerial(string serial)
        {
            var device = await _deviceService.GetDeviceBySerialAsync(serial);
            if (device == null) return NotFound(new { message = "Device not found" });
            return Ok(device);
        }

        /// <summary>
        /// WHO: Admin.
        /// WHAT: Enables or disables a hardware device.
        /// </summary>
        [HttpPost("{id:int}/toggle")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> ToggleStatus(int id, [FromQuery] bool isActive)
        {
            if (await _deviceService.ToggleDeviceStatusAsync(id, isActive)) return Ok(new { message = "Device status updated" });
            return BadRequest(new { message = "Failed to update device status" });
        }

        /// <summary>
        /// WHO: Admin.
        /// WHAT: Lists all devices currently in the system.
        /// </summary>
        [HttpGet("All")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<DeviceDTO>>> GetAll()
        {
            var devices = await _deviceService.GetAllDevicesAsync();
            return Ok(devices);
        }
    }
}
