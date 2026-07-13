using SeenCL.Domain.Entities;
using SeenCL.DTOs;
using SeenCL.Repositories;
using SeenCL.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeenBLL.Services
{
    public class SensorService : ISensorService
    {
        private readonly ISensorRepository _repository;

        public SensorService(ISensorRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<SensorDTO>> GetAllSensorsAsync()
        {
            var sensors = await Task.FromResult(_repository.GetAll());
            return sensors.Select(MapToDTO);
        }

        public async Task<SensorDTO?> GetSensorByIdAsync(int id)
        {
            var sensor = await Task.FromResult(_repository.GetById(id));
            return sensor != null ? MapToDTO(sensor) : null;
        }

        private SensorDTO MapToDTO(Sensor s)
        {
            return new SensorDTO(
                s.SensorID,
                s.SensorName,
                s.SensorType,
                s.Unit,
                s.MinSafeValue,
                s.MaxSafeValue,
                s.Description
            );
        }
    }
}
