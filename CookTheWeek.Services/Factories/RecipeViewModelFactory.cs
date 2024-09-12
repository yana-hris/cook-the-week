namespace CookTheWeek.Services.Data.Factories
{
    using System.Threading.Tasks;

    using CookTheWeek.Services.Data.Factories.Interfaces;
    using CookTheWeek.Services.Data.Interfaces;
    using CookTheWeek.Web.ViewModels.Recipe;
    using CookTheWeek.Web.ViewModels.Recipe.Enums;

    using static CookTheWeek.Common.HelperMethods.EnumHelper;

    public class RecipeViewModelFactory : IRecipeViewModelFactory
    {
        private readonly IRecipeService recipeService;
        private readonly ICategoryService categoryService;

        public RecipeViewModelFactory(IRecipeService recipeService, 
                                      ICategoryService categoryService)
        {
            this.recipeService = recipeService;
            this.categoryService = categoryService;
        }
        public async Task<AllRecipesFilteredAndPagedViewModel> CreateAllRecipesViewModelAsync(AllRecipesQueryModel queryModel, string userId)
        {
            
            var allRecipes = await recipeService.AllAsync(queryModel, userId);
            var categories = await categoryService.AllRecipeCategoryNamesAsync();
            
            var viewModel = new AllRecipesFilteredAndPagedViewModel
            {
                SearchString = queryModel.SearchString,
                TotalRecipes = allRecipes.Count,
                RecipeSorting = queryModel.RecipeSorting,
                Category = queryModel.Category,
                RecipesPerPage = queryModel.RecipesPerPage,
                CurrentPage = queryModel.CurrentPage,
                Recipes = allRecipes,
                Categories = categories,
                RecipeSortings = GetEnumValuesDictionary<RecipeSorting>()
            };

            return viewModel;
        }
    }
}
