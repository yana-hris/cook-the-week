namespace CookTheWeek.Web.Infrastructure.Middlewares
{
    using System.Security.Claims;

    using Microsoft.AspNetCore.Http;
    public class UserContextMiddleware
    {
        private readonly RequestDelegate _next;

        public UserContextMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IUserContext userContext)
        {
            var userId = context.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!string.IsNullOrEmpty(userId))
            {
                userContext.UserId = userId;
            }

            await _next(context);
        }
    }

}
