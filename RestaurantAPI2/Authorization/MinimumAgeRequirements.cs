using Microsoft.AspNetCore.Authorization;

namespace RestaurantAPI2.Authorization
{
    public class MinimumAgeRequirements : IAuthorizationRequirement
    {
        public int MinimumAge { get; }
        public MinimumAgeRequirements(int minimumAge) 
        {
            minimumAge = MinimumAge;
        }        
    }
}
