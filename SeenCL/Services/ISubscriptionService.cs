using SeenCL.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SeenCL.Services
{
    public interface ISubscriptionService
    {
        Task<IEnumerable<SubscriptionDTO>> GetAvailablePlansAsync();
    }
}
