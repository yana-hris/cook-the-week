namespace CookTheWeek.Web.Areas.Admin.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using CookTheWeek.Data.Models;
    using CookTheWeek.Services.Data.Services.Interfaces;
    using CookTheWeek.Web.ViewModels.Admin.CategoryAdmin;
    using CookTheWeek.Web.ViewModels.Category;    
    using ViewModels.Admin.HomeAdmin;

    public class HomeAdminController : BaseAdminController
    {
        private readonly IRecipeService recipeService; 
        private readonly ICategoryService<IngredientCategory,
                                            IngredientCategoryAddFormModel,
                                            IngredientCategoryEditFormModel,
                                            IngredientCategorySelectViewModel> ingredientCategoryService;
        private readonly ICategoryService<RecipeCategory, 
                                            RecipeCategoryAddFormModel, 
                                            RecipeCategoryEditFormModel,
                                            RecipeCategorySelectViewModel> recipeCategoryService;
        private readonly IIngredientService ingredientService;
        private readonly IMealPlanService mealplanService;
        private readonly IUserService userService;

        public HomeAdminController(IRecipeService recipeService,
            IUserService userService,
            ICategoryService<IngredientCategory,
                                            IngredientCategoryAddFormModel,
                                            IngredientCategoryEditFormModel,
                                            IngredientCategorySelectViewModel> ingredientCategoryService,
            ICategoryService<RecipeCategory,
                                            RecipeCategoryAddFormModel,
                                            RecipeCategoryEditFormModel,
                                            RecipeCategorySelectViewModel> recipeCategoryService,

            IIngredientService ingredientService,
            IMealPlanService mealplanService)
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
            model.RecipesTotalCount = await this.recipeService.GetAllCountAsync();
            model.RecipeCategoriesTotalCount = await this.recipeCategoryService.GetAllCategoriesCountAsync();
            model.IngredientsTotalCount = await this.ingredientService.AllCountAsync();
            model.IngredientCategoriesTotalCount = await this.ingredientCategoryService.GetAllCategoriesCountAsync();
            model.UsersTotalCount = await this.userService.AllCountAsync();
            model.MealPlansTotalCount = await this.mealplanService.AllActiveCountAsync();

            return View(model);
        }
    }
}
