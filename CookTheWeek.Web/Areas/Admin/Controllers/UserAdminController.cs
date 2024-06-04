namespace CookTheWeek.Web.Areas.Admin.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Memory;

    using Services.Data.Interfaces;
    using ViewModels.Admin.UserAdmin;

    using static Common.GeneralApplicationConstants;

    public class UserAdminController : BaseAdminController
    {
        private readonly IUserAdminService userAdminService;
        private readonly IMemoryCache memoryCache;
        private readonly ILogger<UserAdminController> logger;

       

        public UserAdminController(IUserAdminService userAdminService
            ,IMemoryCache memoryCache,
            ILogger<UserAdminController> logger
            )
        {
            this.userAdminService = userAdminService;
            this.memoryCache = memoryCache;
            this.logger = logger;
            
        }
        public async Task<IActionResult> All()
        {
            IEnumerable<UserAllViewModel>? users = this.memoryCache.Get<IEnumerable<UserAllViewModel>>(UsersCacheKey);

            if(users == null)
            {
                try
                {
                    users = await this.userAdminService.AllAsync();
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
