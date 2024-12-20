﻿namespace CookTheWeek.Web.Infrastructure.Middlewares
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

        public async Task Invoke(HttpContext context, IUserContext userContext)
        {
           
            if (context.User.Identity?.IsAuthenticated ?? false)
            {
                string userIdClaim = context.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";

                if (Guid.TryParse(userIdClaim, out Guid userId))
                {
                    userContext.UserId = userId;
                }
                else
                {
                    userContext.UserId = Guid.Empty; 
                }

                userContext.IsAdmin = context.User.IsInRole(AdminRoleName);

                
                if (bool.TryParse(context.User.FindFirstValue(HasActiveMealPlanClaimName), out bool hasActiveMealplan))
                {
                    userContext.HasActiveMealplan = hasActiveMealplan;
                }
                else
                {
                    userContext.HasActiveMealplan = false;
                }
            }
              
            await next(context);
        }
    }
}
