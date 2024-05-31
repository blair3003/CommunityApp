using CommunityApp.Data.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace CommunityApp.Policies
{
    public class CommunityManagerAuthorizationHandler : AuthorizationHandler<CommunityManagerRequirement, Community>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            CommunityManagerRequirement requirement,
            Community community)
        {
            if (context.User.HasClaim(c => c.Type == "IsAdmin" && c.Value == "true")
             || community.Managers.Any(manager => manager.Id == context.User.FindFirstValue(ClaimTypes.NameIdentifier)))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
