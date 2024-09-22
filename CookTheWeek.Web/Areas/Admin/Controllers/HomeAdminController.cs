namespace CookTheWeek.Web.Areas.Admin.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Services.Data.Interfaces;
    using ViewModels.Admin.HomeAdmin;

    public class HomeAdminController : BaseAdminController
    {
        private readonly IRecipeService recipeService;
        private readonly ICategoryService categoryService;
        private readonly IIngredientService ingredientService;
        private readonly IMealPlanService mealplanService;
        private readonly IUserService userService;

        public HomeAdminController(IRecipeService recipeService,
            IUserService userService,
            ICategoryService categoryService,
            IIngredientService ingredientService,
            IMealPlanService mealplanService)
        {
            this.recipeService = recipeService; 
            this.categoryService = categoryService;
            this.ingredientService = ingredientService;
            this.mealplanService = mealplanService;
            this.userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            AdminServiceModel model = new AdminServiceModel();
            model.RecipesTotalCount = await this.recipeService.AllCountAsync();
            model.RecipeCategoriesTotalCount = await this.categoryService.AllRecipeCategoriesCountAsync();
            model.IngredientsTotalCount = await this.ingredientService.AllCountAsync();
            model.IngredientCategoriesTotalCount = await this.categoryService.AllIngredientCategoriesCountAsync();
            model.UsersTotalCount = await this.userService.AllCountAsync();
            model.MealPlansTotalCount = await this.mealplanService.AllActiveCountAsync();

            return View(model);
        }
    }
}
