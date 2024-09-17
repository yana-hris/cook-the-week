namespace CookTheWeek.Services.Data.Factories
{
    using System.Threading.Tasks;
    using CookTheWeek.Common.Exceptions;
    using CookTheWeek.Services.Data.Factories.Interfaces;
    using CookTheWeek.Services.Data.Interfaces;
    using CookTheWeek.Web.ViewModels.Recipe;
    using CookTheWeek.Web.ViewModels.Recipe.Enums;
    using CookTheWeek.Web.ViewModels.RecipeIngredient;
    using CookTheWeek.Web.ViewModels.Step;

    using static CookTheWeek.Common.EntityValidationConstants.Recipe;
    using static CookTheWeek.Common.HelperMethods.EnumHelper;

    public class RecipeViewModelFactory : IRecipeViewModelFactory
    {
        private readonly IRecipeService recipeService;
        private readonly ICategoryService categoryService;
        private readonly IRecipeIngredientService recipeIngredientService;
        private readonly IFavouriteRecipeService favouriteRecipeService;
        private readonly IMealService mealService;

        public RecipeViewModelFactory(IRecipeService recipeService, 
                                      ICategoryService categoryService,
                                      IRecipeIngredientService recipeIngredientService,
                                      IFavouriteRecipeService favouriteRecipeService,
                                      IMealService mealService)
        {
            this.recipeService = recipeService;
            this.categoryService = categoryService;
            this.recipeIngredientService = recipeIngredientService;
            this.favouriteRecipeService = favouriteRecipeService;
            this.mealService = mealService;
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
        public async Task<RecipeAddFormModel> CreateRecipeAddFormModelAsync()
        {
            // Create the ViewModel with default values and empty lists
            var model = new RecipeAddFormModel
            {
                // Load categories and servings options
                Categories = await categoryService.AllRecipeCategoriesAsync(),
                ServingsOptions = ServingsOptions,

                // Initialize lists with default values
                RecipeIngredients = new List<RecipeIngredientFormModel>
                {
                    new RecipeIngredientFormModel() // Empty ingredient
                },
                Steps = new List<StepFormModel>
                {
                    new StepFormModel() // Empty step
                }
            };

            // Perform parallel asynchronous operations
            var measuresTask = recipeIngredientService.GetRecipeIngredientMeasuresAsync();
            var specificationsTask = recipeIngredientService.GetRecipeIngredientSpecificationsAsync();

            // Await the results of parallel tasks
            var measures = await measuresTask;
            var specifications = await specificationsTask;

            // Set the measures and specifications for the first ingredient
            model.RecipeIngredients.First().Measures = measures;
            model.RecipeIngredients.First().Specifications = specifications;

            return model;
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
        public async Task<RecipeDetailsViewModel> CreateRecipeDetailsViewModelAsync(string recipeId, string userId)
        {
            try
            {
                RecipeDetailsViewModel model = await this.recipeService.DetailsByIdAsync(recipeId, userId);
                model.IsLikedByUser = await this.favouriteRecipeService.IsLikedByUserIdAsync(recipeId, userId);
                model.LikesCount = await this.favouriteRecipeService.LikesCountAsync(recipeId);
                model.CookedCount = await this.mealService.MealsCountAsync(recipeId);

                return model;
            }
            catch (UnauthorizedException)
            {
                throw;
            }
            catch (RecordNotFoundException)
            {
                throw;
            }
            catch (DataRetrievalException)
            {
                throw;
            }
        }

        /// <summary>
        /// Generates a view model for a user's recipes, including owned and favorite recipes.
        /// </summary>
        public async Task<RecipeMineViewModel> CreateRecipeMineViewModelAsync(string userId)
        {
            RecipeMineViewModel model = new RecipeMineViewModel();
            model.FavouriteRecipes = await this.favouriteRecipeService.AllLikedByUserIdAsync(userId);
            model.OwnedRecipes = await this.recipeService.AllAddedByUserIdAsync(userId);

            return model;
        }

       
    }
}
