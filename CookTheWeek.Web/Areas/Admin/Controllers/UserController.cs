namespace CookTheWeek.Web.Areas.Admin.Controllers
{
    using CookTheWeek.Services.Data.Interfaces;
    using Microsoft.AspNetCore.Mvc;

    public class UserController : BaseAdminController
    {
        private readonly IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }
        public async Task<IActionResult> All()
        {
            IEnumerable<UserViewModel> users =
                await this.userService.AllAsync();

            return View(users);
        }
    }
}
