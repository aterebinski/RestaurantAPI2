using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration.UserSecrets;
using RestaurantAPI2.Entities;
using System.Security.Claims;

namespace RestaurantAPI2.Authorization
{
    public class CreateRestaurantRequirementHandler : AuthorizationHandler<CreateRestaurantRequirement>
    {
        private readonly RestaurantDbContext _dbContext;

        public CreateRestaurantRequirementHandler(RestaurantDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CreateRestaurantRequirement requirement)
        {
            int userId = int.Parse(context.User.FindFirst(i => i.Type == ClaimTypes.NameIdentifier).Value);
            int howManyRestaurantCreatedByUser = _dbContext.Restaurants.Count(i => i.CreatedById == userId);

            if(howManyRestaurantCreatedByUser >= requirement.HowManyRestaurantsShouldUserCreated)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }


    }
}
