using Microsoft.AspNetCore.Mvc;
using SeenCL.DTOs;
using SeenCL.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace SeenAPI.Controllers
{
    /// <summary>
    /// Controller for recording and retrieving telemetry data from sensors.
    /// </summary>
    [Route("api/sensor-data")]
    [ApiController]
    public class SensorDataController : ControllerBase
    {
        private readonly ISensorDataService _dataService;

        public SensorDataController(ISensorDataService dataService)
        {
            _dataService = dataService;
        }

        /// <summary>
        /// WHO: Hardware Sensors.
        /// WHAT: Records a new data point (e.g., heart rate, speed).
        /// </summary>
        [HttpPost("Record")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Record([FromBody] SensorDataDTO dto)
        {
            var id = await _dataService.RecordDataAsync(dto);
            if (id <= 0) return BadRequest(new { message = "Failed to record data" });
            return Ok(new { DataID = id, message = "Data recorded successfully" });
        }

        /// <summary>
        /// WHO: Admin / Athlete (Dashboard).
        /// WHAT: Retrieves historical data for a specific sensor.
        /// </summary>
        [HttpGet("sensor/{sensorId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<SensorDataDTO>>> GetBySensor(int sensorId)
        {
            var data = await _dataService.GetDataBySensorAsync(sensorId);
            return Ok(data);
        }
    }
}
