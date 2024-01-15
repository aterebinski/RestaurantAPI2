using Microsoft.AspNetCore.Authorization;

namespace RestaurantAPI2.Authorization
{
    public enum ResourceOperation
    {
        Create, 
        Update, 
        Delete, 
        Read
    }
    public class ResourceOperationRequirement : IAuthorizationRequirement
    {
        public ResourceOperation ResourceOperation { get; }

        public ResourceOperationRequirement(ResourceOperation resourceOperation)
        {
            ResourceOperation = resourceOperation;
        }
    }
}
