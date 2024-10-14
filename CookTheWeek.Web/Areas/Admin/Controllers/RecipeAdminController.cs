namespace CookTheWeek.Web.Areas.Admin.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using CookTheWeek.Web.ViewModels.Recipe;
    using CookTheWeek.Services.Data.Services.Interfaces;
    using CookTheWeek.Services.Data.Factories;

    public class RecipeAdminController : BaseAdminController
    {
        private readonly IRecipeService recipeService;
        private readonly IUserService userService;
        private readonly IRecipeViewModelFactory viewModelFactory;

        public RecipeAdminController(IRecipeService recipeService,
            IUserService userService,
            IRecipeViewModelFactory viewModelFactory,
            ILogger<RecipeAdminController> logger) 
        : base(logger)
        {
            this.recipeService = recipeService;
            this.viewModelFactory = viewModelFactory;
            this.userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> All()
        {
            try
            {
                RecipeMineAdminViewModel model = await viewModelFactory.CreateAdminAllRecipesViewModelAsync();
                ViewBag.ReturnUrl = Request.Path + Request.QueryString;

                if (!model.SiteRecipes.Any() && !model.UserRecipes.Any())
                {
                    return RedirectToAction("None");
                }

                return View(model);
            }
            catch (Exception ex)
            {
                return HandleException(ex, nameof(All), nameof(RecipeMineAdminViewModel), null);
            }
        }

        [HttpGet]
        public IActionResult None()
        {
            return View();
        }
    }
}
