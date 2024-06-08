using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using CommunityApp.Data.Models;

namespace CommunityApp.Policies
{
    public class LeaseTenantAuthorizationHandler : AuthorizationHandler<LeaseTenantRequirement, Lease>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            LeaseTenantRequirement requirement,
            Lease lease)
        {
            if (context.User.HasClaim(c => c.Type == "IsAdmin" && c.Value == "true"))
            {
                context.Succeed(requirement);
            }

            if (lease.Tenant?.Id == context.User.FindFirstValue(ClaimTypes.NameIdentifier))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}