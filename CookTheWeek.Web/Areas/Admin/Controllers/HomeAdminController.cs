namespace CookTheWeek.Web.Areas.Admin.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using CookTheWeek.Data.Models;
    using CookTheWeek.Services.Data.Services.Interfaces;
    using CookTheWeek.Web.ViewModels;
    using CookTheWeek.Web.ViewModels.Admin.CategoryAdmin;
    using CookTheWeek.Web.ViewModels.Admin.HomeAdmin;

    public class HomeAdminController : BaseAdminController
    {
        private readonly IRecipeService recipeService; 
        private readonly ICategoryService<IngredientCategory,
                                            IngredientCategoryAddFormModel,
                                            IngredientCategoryEditFormModel,
                                            SelectViewModel> ingredientCategoryService;
        private readonly ICategoryService<RecipeCategory, 
                                            RecipeCategoryAddFormModel, 
                                            RecipeCategoryEditFormModel,
                                            SelectViewModel> recipeCategoryService;
        private readonly IIngredientService ingredientService;
        private readonly IMealPlanService mealplanService;
        private readonly IUserService userService;

        public HomeAdminController(IRecipeService recipeService,
            IUserService userService,
            ICategoryService<IngredientCategory,
                                            IngredientCategoryAddFormModel,
                                            IngredientCategoryEditFormModel,
                                            SelectViewModel> ingredientCategoryService,
            ICategoryService<RecipeCategory,
                                            RecipeCategoryAddFormModel,
                                            RecipeCategoryEditFormModel,
                                            SelectViewModel> recipeCategoryService,

            IIngredientService ingredientService,
            IMealPlanService mealplanService,
            ILogger<HomeAdminController> logger) : base(logger)
        {
            this.recipeService = recipeService; 
            this.recipeCategoryService = recipeCategoryService;
            this.ingredientCategoryService = ingredientCategoryService;
            this.ingredientService = ingredientService;
            this.mealplanService = mealplanService;
            this.userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            AdminServiceModel model = new AdminServiceModel();
            model.RecipesTotalCount = await recipeService.GetAllCountAsync();
            model.RecipeCategoriesTotalCount = await recipeCategoryService.GetAllCategoriesCountAsync();
            model.IngredientsTotalCount = await ingredientService.AllCountAsync();
            model.IngredientCategoriesTotalCount = await ingredientCategoryService.GetAllCategoriesCountAsync();
            model.UsersTotalCount = await userService.AllCountAsync();
            model.MealPlansTotalCount = await mealplanService.AllActiveCountAsync();

            return View(model);
        }
    }
}
