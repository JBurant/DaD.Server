using Microsoft.AspNetCore.Authorization;
using Server.Validation.Requirements;
using System.Threading.Tasks;

namespace Server.Validation.Handlers
{
    public class AuthorAuthorizationHandler : AuthorizationHandler<AuthorAuthorizationRequirement, string>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
                                                       AuthorAuthorizationRequirement requirement,
                                                       string username)
        {
            if (context.User.Identity?.Name == username)
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }

            return Task.CompletedTask;
        }
    }
}