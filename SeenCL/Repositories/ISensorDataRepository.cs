using SeenCL.Domain.Entities;
using SeenCL.Interfaces;

namespace SeenCL.Repositories
{
    public interface ISensorDataRepository : IRepository<SensorData, long>
    {
        IEnumerable<SensorData> GetBySensorId(int sensorId);
    }
}
