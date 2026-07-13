using SeenCL.Domain.Entities;
using SeenCL.Interfaces;

namespace SeenCL.Repositories
{
    public interface ISubscriptionRepository : IRepository<Subscription, int> {
    IEnumerable<Subscription> GetAll();
    }
}
