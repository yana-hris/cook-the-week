namespace CookTheWeek.Web.Areas.Admin.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using CookTheWeek.Web.ViewModels.Recipe;
    using CookTheWeek.Services.Data.Services.Interfaces;

    public class RecipeAdminController : BaseAdminController
    {
        private readonly IRecipeService recipeService;
        private readonly IUserService userService;

        public RecipeAdminController(IRecipeService recipeService,
            IUserService userService,
            ILogger<RecipeAdminController> logger) : base(logger)
        {
            this.recipeService = recipeService;
            this.userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> Site()
        {
            try
            {
                RecipeMineAdminViewModel model = new RecipeMineAdminViewModel();
                model.SiteRecipes = await this.recipeService.GetAllSiteRecipesAsync();
                model.UserRecipes = await this.recipeService.GetAllNonSiteRecipesAsync();

                ViewBag.ReturnUrl = Request.Path + Request.QueryString;

                if (!model.SiteRecipes.Any() && !model.UserRecipes.Any())
                {
                    return RedirectToAction("None");
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
