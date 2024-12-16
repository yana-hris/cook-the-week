namespace CookTheWeek.Web.Areas.Admin.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using CookTheWeek.Services.Data.Services.Interfaces;
    using CookTheWeek.Web.ViewModels.Admin.UserAdmin;

    public class UserAdminController : BaseAdminController
    {
        private readonly IUserService userService;
        

        public UserAdminController(
                        IUserService userService,                        
                        ILogger<UserAdminController> logger
                        ) : base(logger) 
        {
            this.userService = userService;
        }


        [HttpGet]
        public async Task<IActionResult> All([FromQuery]AllUsersQueryModel queryModel)
        {
            AllUsersQueryModel model = new AllUsersQueryModel();

            try
            {
                model = await userService.GetAllAsync(queryModel);
                SetViewData("All Users", Request.Path + Request.Query);
                return View(model);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.StackTrace, ex.Message);
                return HandleException(ex, nameof(All), "Users", null);
            }
            
        }
        
    }
}
