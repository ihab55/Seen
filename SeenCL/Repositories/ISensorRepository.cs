using SeenCL.Domain.Entities;
using SeenCL.Interfaces;

namespace SeenCL.Repositories
{
    public interface ISensorRepository : IRepository<Sensor, int> 
    {
        IEnumerable<Sensor> GetAll();
    }
}
