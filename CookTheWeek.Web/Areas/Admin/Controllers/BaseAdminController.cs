namespace CookTheWeek.Web.Areas.Admin.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using static Common.GeneralApplicationConstants;

    [Area(AdminAreaName)]
    [Authorize(Roles = AdminRoleName)]
    public abstract class BaseAdminController : Controller
    {
       /// <summary>
       /// Extract model errors from the ModelState into a common string 
       /// </summary>
       /// <returns>All model errors, separated by a new line in a single string</returns>
        protected string ExtractModelErrors()
        {
            ICollection<string> modelErrors = ModelState.Values.SelectMany(v => v.Errors)
                                   .Select(e => e.ErrorMessage)
                                   .ToList();

            return string.Join(Environment.NewLine, modelErrors);
        }
    }

   
}
