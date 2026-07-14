using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SeenPL.Authorization
{
    public class OwnerOrAdminHandler : AuthorizationHandler<OwnerOrAdminRequirement>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public OwnerOrAdminHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OwnerOrAdminRequirement requirement)
        {
            var user = context.User;

            // Check if user is authenticated
            if (!user.Identity?.IsAuthenticated ?? true)
            {
                return Task.CompletedTask;
            }

            // Check if user is Admin
            if (user.HasClaim(ClaimTypes.Role, "Admin"))
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }

            // Check if user is the resource owner
            // The resource ID should be passed as a route parameter
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext != null)
            {
                var routeData = httpContext.GetRouteData();
                
                // Try to get the resource ID from route parameters
                // Common parameter names: id, userId, teamId, etc.
                var resourceId = routeData.Values["id"]?.ToString() 
                               ?? routeData.Values["userId"]?.ToString()
                               ?? routeData.Values["teamId"]?.ToString()
                               ?? routeData.Values["programId"]?.ToString();

                if (!string.IsNullOrEmpty(resourceId) && int.TryParse(resourceId, out int id))
                {
                    // Get the user ID from the JWT token
                    var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    
                    if (userIdClaim != null && int.TryParse(userIdClaim, out int userId))
                    {
                        // Check if the authenticated user is the owner of the resource
                        if (userId == id)
                        {
                            context.Succeed(requirement);
                            return Task.CompletedTask;
                        }
                    }
                }
            }

            return Task.CompletedTask;
        }
    }
}
