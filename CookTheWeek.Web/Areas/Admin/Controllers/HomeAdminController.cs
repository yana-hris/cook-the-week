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
        private readonly IUserAdminService userAdminService;
        private readonly IMealPlanService mealplanService;

        public HomeAdminController(IRecipeService recipeService,
            ICategoryService categoryService,
            IIngredientService ingredientService,
            IUserAdminService userAdminService,
            IMealPlanService mealplanService)
        {
            this.recipeService = recipeService; 
            this.categoryService = categoryService;
            this.ingredientService = ingredientService;
            this.userAdminService = userAdminService;
            this.mealplanService = mealplanService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            AdminServiceModel model = new AdminServiceModel();
            model.RecipesTotalCount = await this.recipeService.AllCountAsync();
            model.RecipeCategoriesTotalCount = await this.categoryService.AllRecipeCategoriesCountAsync();
            model.IngredientsTotalCount = await this.ingredientService.AllCountAsync();
            model.IngredientCategoriesTotalCount = await this.categoryService.AllIngredientCategoriesCountAsync();
            model.UsersTotalCount = await this.userAdminService.AllCountAsync();
            model.MealPlansTotalCount = await this.mealplanService.AllActiveCountAsync();

            return View(model);
        }
    }
}
