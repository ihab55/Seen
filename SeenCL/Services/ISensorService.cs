using SeenCL.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SeenCL.Services
{
    public interface ISensorService
    {
        Task<IEnumerable<SensorDTO>> GetAllSensorsAsync();
        Task<SensorDTO?> GetSensorByIdAsync(int id);
    }
}
