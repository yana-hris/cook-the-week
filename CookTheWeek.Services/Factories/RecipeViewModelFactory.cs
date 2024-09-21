namespace CookTheWeek.Services.Data.Factories
{
    using System.Threading.Tasks;

    using CookTheWeek.Common.Exceptions;
    using CookTheWeek.Data.Models;
    using CookTheWeek.Data.Repositories;
    using CookTheWeek.Services.Data.Factories.Interfaces;
    using CookTheWeek.Services.Data.Interfaces;
    using CookTheWeek.Web.ViewModels.Category;
    using CookTheWeek.Web.ViewModels.Recipe;
    using CookTheWeek.Web.ViewModels.Recipe.Enums;
    using CookTheWeek.Web.ViewModels.RecipeIngredient;
    using CookTheWeek.Web.ViewModels.Step;

    using static CookTheWeek.Common.EntityValidationConstants.Recipe;
    using static CookTheWeek.Common.ExceptionMessagesConstants.DataRetrievalExceptionMessages;
    using static CookTheWeek.Common.HelperMethods.EnumHelper;
    using static CookTheWeek.Common.HelperMethods.CookingTimeHelper;

    public class RecipeViewModelFactory : IRecipeViewModelFactory
    {
        private readonly IRecipeService recipeService;
        private readonly IFavouriteRecipeRepository favouriteRecipeRepository;
        private readonly ICategoryService categoryService;
        private readonly IRecipeIngredientService recipeIngredientService;
        private readonly IMealService mealService;

        public RecipeViewModelFactory(IRecipeService recipeService, 
                                      ICategoryService categoryService,
                                      IRecipeIngredientService recipeIngredientService,
                                      IFavouriteRecipeRepository favouriteRecipeRepository,
                                      IMealService mealService)
        {
            this.recipeService = recipeService;
            this.categoryService = categoryService;
            this.recipeIngredientService = recipeIngredientService;
            this.mealService = mealService;
            this.favouriteRecipeRepository = favouriteRecipeRepository;
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
            var addModel = await AddRecipeOptionValuesAsync(new RecipeAddFormModel());

            if (addModel is RecipeAddFormModel model && model != null)
            {
                return model;
            }
            throw new DataRetrievalException(RecipeDataRetrievalExceptionMessage, null);           
        }

        /// <summary>
        /// Generates a form model for editing an existing recipe, including categories and ingredient options.
        /// </summary>
        public async Task<RecipeEditFormModel> CreateRecipeEditFormModelAsync(string recipeId, string userId, bool isAdmin)
        {
            RecipeEditFormModel editModel = await this.recipeService.GetForEditByIdAsync(recipeId, userId, isAdmin);
            
            var filledModel = await AddRecipeOptionValuesAsync(editModel);
            if (filledModel is RecipeEditFormModel model)
            {
                return model;
            }
            throw new DataRetrievalException(RecipeDataRetrievalExceptionMessage, null);
        }

        /// <summary>
        /// Generates a detailed view model for a recipe.
        /// </summary>
        public async Task<RecipeDetailsViewModel> CreateRecipeDetailsViewModelAsync(string recipeId, string userId)
        { 
            RecipeDetailsViewModel model = await this.recipeService.DetailsByIdAsync(recipeId);
            model.IsLikedByUser = await this.favouriteRecipeRepository.GetByIdAsync(userId, recipeId);
            model.LikesCount = await this.favouriteRecipeRepository.AllCountByRecipeIdAsync(recipeId);
            model.CookedCount = await this.mealService.MealsCountAsync(recipeId);

            return model;
        }

        /// <summary>
        /// Generates a view model for a user's recipes, including owned and favorite recipes.
        /// </summary>
        public async Task<RecipeMineViewModel> CreateRecipeMineViewModelAsync(string userId)
        {
            RecipeMineViewModel model = new RecipeMineViewModel();

            ICollection<FavouriteRecipe> likedRecipes = await 
                this.favouriteRecipeRepository.GetAllByUserIdAsync(userId);

            model.FavouriteRecipes = likedRecipes
                .Select(fr => new RecipeAllViewModel()
                {
                    Id = fr.Recipe.Id.ToString(),
                    ImageUrl = fr.Recipe.ImageUrl,
                    Title = fr.Recipe.Title,
                    Description = fr.Recipe.Description,
                    Category = new RecipeCategorySelectViewModel()
                    {
                        Id = fr.Recipe.CategoryId,
                        Name = fr.Recipe.Category.Name
                    },
                    Servings = fr.Recipe.Servings,
                    CookingTime = FormatCookingTime(fr.Recipe.TotalTime)
                }).ToList();

            model.OwnedRecipes = await this.recipeService.AllAddedByUserIdAsync(userId);

            return model;
        }

        public async Task<IRecipeFormModel> AddRecipeOptionValuesAsync(IRecipeFormModel model)
        {            
            model.Categories = await categoryService.AllRecipeCategoriesAsync();
            model.ServingsOptions = ServingsOptions;

            if (!model.RecipeIngredients.Any())
            {
                model.RecipeIngredients.Add(new RecipeIngredientFormModel());
            }

            if (!model.Steps.Any())
            {
                model.Steps.Add(new StepFormModel());
            }
            
            var measures = await recipeIngredientService.GetRecipeIngredientMeasuresAsync();
            var specifications = await recipeIngredientService.GetRecipeIngredientSpecificationsAsync();

            // Set the measures and specifications for the first ingredient
            model.RecipeIngredients.First().Measures = measures;
            model.RecipeIngredients.First().Specifications = specifications;

            return model;
        }
    }
}
