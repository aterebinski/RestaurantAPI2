using System.Security.Claims;

namespace RestaurantAPI2.Services
{
    public interface IUserContextService
    {
        int? getUserId { get; }
        ClaimsPrincipal User { get; }
    }

    public class UserContextService : IUserContextService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserContextService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public ClaimsPrincipal User => _httpContextAccessor.HttpContext?.User;
        public int? getUserId => User is null ? null : (int?)int.Parse(User.Claims.FirstOrDefault(i => i.Type == ClaimTypes.NameIdentifier).Value);
    }
}
