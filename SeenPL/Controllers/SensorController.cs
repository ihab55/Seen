using Microsoft.AspNetCore.Mvc;
using SeenCL.DTOs;
using SeenCL.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace SeenAPI.Controllers
{
    /// <summary>
    /// Controller for managing sensors attached to hardware devices.
    /// </summary>
    [Route("api/sensors")]
    [ApiController]
    public class SensorController : ControllerBase
    {
        private readonly ISensorService _sensorService;

        public SensorController(ISensorService sensorService)
        {
            _sensorService = sensorService;
        }

        /// <summary>
        /// WHO: Admin / Hardware Management.
        /// WHAT: Retrieves a list of all sensors in the system.
        /// </summary>
        [HttpGet("All")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<SensorDTO>>> GetAll()
        {
            var sensors = await _sensorService.GetAllSensorsAsync();
            return Ok(sensors);
        }

        /// <summary>
        /// WHO: Admin.
        /// WHAT: Retrieves specific sensor configuration and status.
        /// </summary>
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<SensorDTO>> GetById(int id)
        {
            var sensor = await _sensorService.GetSensorByIdAsync(id);
            if (sensor == null) return NotFound(new { message = "Sensor not found" });
            return Ok(sensor);
        }
    }
}
