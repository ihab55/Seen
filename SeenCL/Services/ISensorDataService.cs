using SeenCL.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SeenCL.Services
{
    public interface ISensorDataService
    {
        Task<int> RecordDataAsync(SensorDataDTO dto);
        Task<IEnumerable<SensorDataDTO>> GetDataBySensorAsync(int sensorId);
    }
}
