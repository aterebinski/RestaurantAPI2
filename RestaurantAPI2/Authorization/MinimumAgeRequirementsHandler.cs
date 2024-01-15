using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace RestaurantAPI2.Authorization
{
    public class MinimumAgeRequirementsHandler : AuthorizationHandler<MinimumAgeRequirements>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MinimumAgeRequirements requirement)
        {
            var dateofBirth = DateTime.Parse(context.User.FindFirst(i => i.Type == "DateOfBirth").Value);

            if(dateofBirth.AddYears(requirement.MinimumAge) < DateTime.Today)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
