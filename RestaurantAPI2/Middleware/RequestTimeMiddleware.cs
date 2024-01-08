
using System.Diagnostics;

namespace RestaurantAPI2.Middleware
{
    public class RequestTimeMiddleware : IMiddleware
    {
        private readonly ILogger<RequestTimeMiddleware> _logger;
        public RequestTimeMiddleware(ILogger<RequestTimeMiddleware> logger)
        {
            _logger = logger;
        }
        public Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            Stopwatch sw = Stopwatch.StartNew();
            next.Invoke(context);
            sw.Stop();
            long time = sw.ElapsedMilliseconds;
            if (time > 100) 
            {
                _logger.LogWarning("Request bigger than 100ms");
            }
            return Task.CompletedTask;
        }
    }
}
