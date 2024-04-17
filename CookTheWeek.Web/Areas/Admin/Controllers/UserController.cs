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
        private readonly ILogger<UserController> logger;

        public UserController(IUserService userService
            ,IMemoryCache memoryCache,
            ILogger<UserController> logger)
        {
            this.userService = userService;
            this.memoryCache = memoryCache;
            this.logger = logger;
        }
        public async Task<IActionResult> All()
        {
            //IEnumerable<UserViewModel> users =
            //    await this.userService.AllAsync();
            IEnumerable<UserAllViewModel>? users = this.memoryCache.Get<IEnumerable<UserAllViewModel>>(UsersCacheKey);

            if(users == null)
            {
                try
                {
                    users = await this.userService.AllAsync();
                }
                catch (Exception)
                {
                    logger.LogError($"Users were not successfully loaded.");
                    return BadRequest();
                }

                MemoryCacheEntryOptions cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan
                        .FromMinutes(UsersCacheDurationMinutes));

                this.memoryCache.Set(UsersCacheKey, users, cacheOptions);
            }

            return View(users);
        }
    }
}
