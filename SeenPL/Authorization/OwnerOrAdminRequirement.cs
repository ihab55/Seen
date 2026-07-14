using Microsoft.AspNetCore.Authorization;

namespace SeenPL.Authorization
{
    public class OwnerOrAdminRequirement : IAuthorizationRequirement
    {
        public OwnerOrAdminRequirement()
        {
        }
    }
}
