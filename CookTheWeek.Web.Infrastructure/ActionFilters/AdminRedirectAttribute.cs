namespace CookTheWeek.Web.Infrastructure.ActionFilters
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;

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

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var user = context.HttpContext.User;

            if (user.Identity.IsAuthenticated && user.IsInRole(AdminRoleName))
            {
                // Redirect the admin user to the specific admin area action
                context.Result = new RedirectToActionResult(adminAction, adminController, new {area = AdminAreaName});
                return;
            }

            // Continue with the execution of the action if the user is not admin
            await next();
        }
    }

}
