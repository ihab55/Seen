using SeenCL.Domain.Entities;
using SeenCL.Interfaces;

namespace SeenCL.Repositories
{
    public interface IDeviceRepository : IRepository<Device, int>
    {
        Device? GetByUniqueFields(string identifier);
        bool SetStatus(int deviceId, bool isActive);
        IEnumerable<Device> GetAll();
    }
}
