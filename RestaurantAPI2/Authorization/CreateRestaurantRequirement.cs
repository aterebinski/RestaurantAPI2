using Microsoft.AspNetCore.Authorization;

namespace RestaurantAPI2.Authorization
{
    public class CreateRestaurantRequirement : IAuthorizationRequirement
    {
        public int HowManyRestaurantsShouldUserCreated { get; }

        public CreateRestaurantRequirement(int howManyRestaurantsShouldUserCreated)
        {
            HowManyRestaurantsShouldUserCreated = howManyRestaurantsShouldUserCreated;
        }

        
    }
}
