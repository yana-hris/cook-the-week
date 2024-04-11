namespace CookTheWeek.Web.Areas.Admin.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Memory;

    using CookTheWeek.Services.Data.Interfaces;
    using CookTheWeek.Web.ViewModels.User;

    using static Common.GeneralApplicationConstants;

    public class UserController : BaseAdminController
    {
        private readonly IUserService userService;
        private readonly IMemoryCache memoryCache;

        public UserController(IUserService userService
            ,IMemoryCache memoryCache)
        {
            this.userService = userService;
            this.memoryCache = memoryCache;
        }
        public async Task<IActionResult> All()
        {
            //IEnumerable<UserViewModel> users =
            //    await this.userService.AllAsync();
            IEnumerable<UserAllViewModel>? users = this.memoryCache.Get<IEnumerable<UserAllViewModel>>(UsersCacheKey);

            if(users == null)
            {
                users = await this.userService.AllAsync();

                MemoryCacheEntryOptions cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan
                        .FromMinutes(UsersCacheDurationMinutes));

                this.memoryCache.Set(UsersCacheKey, users, cacheOptions);
            }

            return View(users);
        }
    }
}
