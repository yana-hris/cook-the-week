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
        private readonly IRecipeIngredientService recipeIngredientService;
        private readonly IFavouriteRecipeService favouriteRecipeService;

        public RecipeViewModelFactory(IRecipeService recipeService, 
                                      ICategoryService categoryService,
                                      IRecipeIngredientService recipeIngredientService,
                                      IFavouriteRecipeService favouriteRecipeService)
        {
            this.recipeService = recipeService;
            this.categoryService = categoryService;
            this.recipeIngredientService = recipeIngredientService;
            this.favouriteRecipeService = favouriteRecipeService;
        }

        /// <summary>
        /// Generates a view model for displaying all recipes with filtering and sorting.
        /// </summary>
        public async Task<AllRecipesFilteredAndPagedViewModel> CreateAllRecipesViewModelAsync(AllRecipesQueryModel queryModel, string userId)
        {
            
            var allRecipes = await recipeService.AllAsync(queryModel, userId);
            var categories = await categoryService.AllRecipeCategoryNamesAsync();
            
            var viewModel = new AllRecipesFilteredAndPagedViewModel
            {
                Category = queryModel.Category,
                SearchString = queryModel.SearchString,
                RecipeSorting = queryModel.RecipeSorting,
                RecipesPerPage = queryModel.RecipesPerPage,
                CurrentPage = queryModel.CurrentPage,
                TotalRecipes = queryModel.TotalRecipes,
                Recipes = allRecipes,
                Categories = categories,
                RecipeSortings = GetEnumValuesDictionary<RecipeSorting>()
            };

            return viewModel;
        }

        /// <summary>
        /// Generates a form model for adding a new recipe with populated categories and ingredient options.
        /// </summary>
        public Task<RecipeAddFormModel> CreateRecipeAddFormModelAsync()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Generates a form model for editing an existing recipe, including categories and ingredient options.
        /// </summary>
        public Task<RecipeEditFormModel> CreateRecipeEditFormModelAsync(string recipeId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Generates a detailed view model for a recipe.
        /// </summary>
        public Task<RecipeDetailsViewModel> CreateRecipeDetailsViewModelAsync(string recipeId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Generates a view model for a user's recipes, including owned and favorite recipes.
        /// </summary>
        public Task<RecipeMineViewModel> CreateRecipeMineViewModelAsync(string userId)
        {
            throw new NotImplementedException();
        }
    }
}
