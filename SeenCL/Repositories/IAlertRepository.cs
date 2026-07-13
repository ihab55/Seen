using SeenCL.Domain.Entities;
using SeenCL.Interfaces;

namespace SeenCL.Repositories
{
    public interface IAlertRepository : IRepository<Alert, int> 
    {
        IEnumerable<Alert> GetByDeviceId(int deviceId);
    }
}
