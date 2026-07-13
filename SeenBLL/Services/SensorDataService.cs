using SeenCL.Domain.Entities;
using SeenCL.DTOs;
using SeenCL.Repositories;
using SeenCL.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeenBLL.Services
{
    public class SensorDataService : ISensorDataService
    {
        private readonly ISensorDataRepository _repository;

        public SensorDataService(ISensorDataRepository repository)
        {
            _repository = repository;
        }

        public async Task<int> RecordDataAsync(SensorDataDTO dto)
        {
            var data = new SensorData
            {
                SensorID = dto.SensorID,
                DeviceID = dto.DeviceID,
                Reader = dto.Reader,
                Timestamp = dto.Timestamp
            };
            return await Task.FromResult((int)_repository.Create(data));
        }

        public async Task<IEnumerable<SensorDataDTO>> GetDataBySensorAsync(int sensorId)
        {
            var results = await Task.FromResult(_repository.GetBySensorId(sensorId));
            return results.Select(d => new SensorDataDTO(
                d.DataID,
                d.Reader,
                d.Timestamp,
                d.SensorID,
                d.DeviceID
            ));
        }
    }
}
