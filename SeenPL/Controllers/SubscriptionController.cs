using Microsoft.AspNetCore.Mvc;
using SeenCL.DTOs;
using SeenCL.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace SeenAPI.Controllers
{
    /// <summary>
    /// Controller for managing subscription plans and user memberships.
    /// </summary>
    [Route("api/subscriptions")]
    [ApiController]
    public class SubscriptionController : ControllerBase
    {
        private readonly ISubscriptionService _subscriptionService;

        public SubscriptionController(ISubscriptionService subscriptionService)
        {
            _subscriptionService = subscriptionService;
        }

        /// <summary>
        /// WHO: All Users / Mobile App.
        /// WHAT: Retrieves a list of all available subscription plans.
        /// </summary>
        [HttpGet("plans")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<SubscriptionDTO>>> GetPlans()
        {
            var plans = await _subscriptionService.GetAvailablePlansAsync();
            return Ok(plans);
        }
    }
}
