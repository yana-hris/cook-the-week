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
                model.SiteRecipes = await this.recipeService.AllSiteAsync();
                model.UserRecipes = await this.recipeService.AllUserRecipesAsync();

                if (!model.SiteRecipes.Any() && !model.UserRecipes.Any())
                {
                    return View("None");
                }

                return View(model);
            }
            catch (Exception)
            {
                logger.LogError("Site Recipes unsuccessfully loaded to View Model!");
                return BadRequest();
            }
        }

        [HttpGet]
        public IActionResult None()
        {
            return View();
        }
    }
}
