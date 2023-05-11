using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace Stock.API.Authorization
{
    public class StockAPIRequirementHandler : AuthorizationHandler<StockAPIRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, StockAPIRequirement requirement)
        {
            if (!context.User.HasClaim(c => c.Value != null && c.Value == requirement.APIScope))
            {
                context.Fail();
            }
            else
            {
                context.Succeed(requirement);
            }


            return Task.FromResult(0);
        }
    }
}
