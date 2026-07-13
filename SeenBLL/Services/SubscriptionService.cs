using SeenCL.Domain.Entities;
using SeenCL.DTOs;
using SeenCL.Repositories;
using SeenCL.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeenBLL.Services
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly ISubscriptionRepository _repository;

        public SubscriptionService(ISubscriptionRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<SubscriptionDTO>> GetAvailablePlansAsync()
        {
            var plans = await Task.FromResult(_repository.GetAll());
            return plans.Select(p => new SubscriptionDTO(
                p.SubscriptionID,
                p.PlanName,
                p.Description,
                p.MaxPlayers,
                p.DurationDays,
                p.Price,
                p.CreatedAt
            ));
        }
    }
}
