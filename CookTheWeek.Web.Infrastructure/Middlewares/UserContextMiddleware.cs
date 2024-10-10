namespace CookTheWeek.Web.Infrastructure.Middlewares
{
    using System.Security.Claims;

    using Microsoft.AspNetCore.Http;

    using CookTheWeek.Services.Data.Services.Interfaces;

    using static CookTheWeek.Common.GeneralApplicationConstants;
    public class UserContextMiddleware
    {
        private readonly RequestDelegate next;

        public UserContextMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context, IUserContext userContext)
        {
            try
            {
                if (context.User.Identity?.IsAuthenticated ?? false)
                {
                    userContext.UserId = context.User?.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
                    userContext.IsAdmin = context.User?.IsInRole(AdminRoleName) ?? false;

                }
              
                await next(context);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Middleware error: {ex.Message}");
                throw;
            }
        }

    }

}
