using SeenCL.Domain.Entities;
using SeenCL.Interfaces;

namespace SeenCL.Repositories
{
    public interface IAdminRepository : IRepository<Admin, int>
    {
        Admin? GetByEmail(string email);
        IEnumerable<Admin> GetAll();
    }
}
