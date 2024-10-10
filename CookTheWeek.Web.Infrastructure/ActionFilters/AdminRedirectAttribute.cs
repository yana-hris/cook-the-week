namespace CookTheWeek.Web.Infrastructure.ActionFilters
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.Extensions.DependencyInjection;

    using CookTheWeek.Services.Data.Services.Interfaces;

    using static CookTheWeek.Common.GeneralApplicationConstants;

    public class AdminRedirectAttribute : ActionFilterAttribute
    {
        private readonly string adminController;
        private readonly string adminAction;

        public AdminRedirectAttribute(string adminAction, string adminController)
        {
            this.adminController = adminController;
            this.adminAction = adminAction;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            // Get the userContext or whatever method you use to determine if the user is an admin
            var userContext = context.HttpContext.RequestServices.GetService<IUserContext>();

            if (userContext == null || string.IsNullOrEmpty(userContext.UserId))
            {
               
                return;
            }

            if (userContext != null && userContext.IsAdmin)
            {
                // Redirect to the admin area action
                context.Result = new RedirectToActionResult(adminAction, adminController, new { area = AdminAreaName });
            }
        }
    }

}
