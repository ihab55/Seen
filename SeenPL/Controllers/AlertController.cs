using Microsoft.AspNetCore.Mvc;
using SeenCL.DTOs;
using SeenCL.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace SeenAPI.Controllers
{
    /// <summary>
    /// Controller for managing hardware alerts and device notifications.
    /// </summary>
    [Route("api/alerts")]
    [ApiController]
    public class AlertController : ControllerBase
    {
        private readonly IAlertService _alertService;

        public AlertController(IAlertService alertService)
        {
            _alertService = alertService;
        }

        /// <summary>
        /// WHO: Hardware Devices / Admin.
        /// WHAT: Retrieves all alerts associated with a specific device.
        /// </summary>
        [HttpGet("device/{deviceId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<AlertDTO>>> GetByDevice(int deviceId)
        {
            var alerts = await _alertService.GetAlertsByDeviceAsync(deviceId);
            return Ok(alerts);
        }

        /// <summary>
        /// WHO: Hardware Devices / Sensors.
        /// WHAT: Records a new alert triggered by a sensor.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<AlertDTO>> Create([FromBody] AlertDTO dto)
        {
            var id = await _alertService.CreateAlertAsync(dto);
            if (id <= 0) return BadRequest(new { message = "Failed to create alert" });
            
            dto.AlertID = id;
            return CreatedAtAction(nameof(GetByDevice), new { deviceId = dto.DeviceID }, dto);
        }
    }
}
