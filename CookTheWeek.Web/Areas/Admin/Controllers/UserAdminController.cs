namespace CookTheWeek.Web.Areas.Admin.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Memory;

    using Services.Data.Services.Interfaces;
    using ViewModels.Admin.UserAdmin;

    using static Common.GeneralApplicationConstants;

    public class UserAdminController : BaseAdminController
    {
        private readonly IUserService userService;
        private readonly IMemoryCache memoryCache;

        public UserAdminController(
                        IUserService userService,
                        IMemoryCache memoryCache,
                        ILogger<UserAdminController> logger
                        ) : base(logger) 
        {
            this.userService = userService;
            this.memoryCache = memoryCache;
        }


        [HttpGet]
        public async Task<IActionResult> All()
        {
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
