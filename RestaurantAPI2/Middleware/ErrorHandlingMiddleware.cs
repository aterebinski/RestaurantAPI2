
using RestaurantAPI2.Exceptions;

namespace RestaurantAPI2.Middleware
{
    public class ErrorHandlingMiddleware : IMiddleware
    {
        private readonly ILogger<ErrorHandlingMiddleware> _logger;
        public ErrorHandlingMiddleware(ILogger<ErrorHandlingMiddleware> logger)
        {
                _logger = logger;
        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            //throw new NotImplementedException();

            try
            {
                await next.Invoke(context);
            }
            catch(NotFoundException notFoundException)
            {
                context.Response.StatusCode = 404;
                await context.Response.WriteAsync(notFoundException.Message);
            }
            catch(BadRequestException badRequestException)
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync($"{badRequestException.Message}");
            }
            catch (ForbiddenException forbiddenException)
            {
                context.Response.StatusCode = 403;
            }
            catch (Exception e)
            {
                _logger.LogWarning(e, e.Message);
                context.Response.StatusCode = 500;
                await context.Response.WriteAsync("Nie udało się");
                //throw;
            }
        }
    }
}
