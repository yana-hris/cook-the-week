namespace CookTheWeek.Web.Areas.Admin.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using CookTheWeek.Web.ViewModels.Recipe;
    using CookTheWeek.Services.Data.Interfaces;

    using static CookTheWeek.Common.GeneralApplicationConstants;

    public class RecipeAdminController : BaseAdminController
    {
        private readonly IRecipeService recipeService;
        private readonly IUserService userService;
        private readonly IUserAdminService userAdminService;
        private readonly ILogger<RecipeAdminController> logger;

        public RecipeAdminController(IRecipeService recipeService,
            IUserService userService,
            IUserAdminService userAdminService,
            ILogger<RecipeAdminController> logger)
        {
            this.recipeService = recipeService;
            this.userService = userService;
            this.userAdminService = userAdminService;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Site()
        {
            try
            {
                RecipeMineAdminViewModel model = new RecipeMineAdminViewModel();
                
                string[] allAdminIds = await userAdminService.AllUsersInRoleIdsAsync(AdminRoleName);
                string[] allNonAdminUserIds = await this.userAdminService.AllUsersNotInRoleIdsAsync(AdminRoleName);

                if (allAdminIds != null && allAdminIds.Length > 0)
                {
                    model.SiteRecipes = await this.recipeService.AllSite(allAdminIds);
                }

                if (allNonAdminUserIds != null && allNonAdminUserIds.Length > 0)
                {
                    foreach (var userId in allNonAdminUserIds)
                    {
                        ICollection<RecipeAllViewModel> userRecipes = await this.recipeService.AllAddedByUserAsync(userId);

                        if (userRecipes.Count > 0)
                        {
                            model.UserRecipes.AddRange(userRecipes);
                        }
                    }
                }
                return View(model);
            }
            catch (Exception)
            {
                logger.LogError("Site Recipes unsuccessfully loaded to View Model!");
                return BadRequest();
            }
        }
    }
}
