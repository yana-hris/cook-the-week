namespace CookTheWeek.Services.Data.Factories
{
    using System.Threading.Tasks;

    using CookTheWeek.Common.Exceptions;
    using CookTheWeek.Data.Models;
    using CookTheWeek.Services.Data.Factories.Interfaces;
    using CookTheWeek.Services.Data.Services.Interfaces;
    using CookTheWeek.Web.ViewModels.Admin.CategoryAdmin;
    using CookTheWeek.Web.ViewModels.Category;
    using CookTheWeek.Web.ViewModels.Interfaces;
    using CookTheWeek.Web.ViewModels.Recipe;
    using CookTheWeek.Web.ViewModels.Recipe.Enums;
    using CookTheWeek.Web.ViewModels.RecipeIngredient;
    using CookTheWeek.Web.ViewModels.Step;
    using Microsoft.Extensions.Logging;
    using static CookTheWeek.Common.EntityValidationConstants.Recipe;
    using static CookTheWeek.Common.ExceptionMessagesConstants;
    using static CookTheWeek.Common.HelperMethods.EnumHelper;

    public class RecipeViewModelFactory : IRecipeViewModelFactory
    {
        private readonly IRecipeService recipeService;
        private readonly ICategoryService<RecipeCategory, 
            RecipeCategoryAddFormModel, 
            RecipeCategoryEditFormModel, 
            RecipeCategorySelectViewModel> categoryService;
        private readonly IRecipeIngredientService recipeIngredientService;
        private readonly IMealService mealService;
        private readonly ILogger<RecipeViewModelFactory> logger;    

        public RecipeViewModelFactory(IRecipeService recipeService,
                                      ICategoryService<RecipeCategory, 
                                          RecipeCategoryAddFormModel, 
                                          RecipeCategoryEditFormModel, 
                                          RecipeCategorySelectViewModel> categoryService,
                                      IRecipeIngredientService recipeIngredientService,
                                      ILogger<RecipeViewModelFactory> logger,
                                      IMealService mealService)
        {
            this.recipeService = recipeService;
            this.categoryService = categoryService;
            this.recipeIngredientService = recipeIngredientService;
            this.logger = logger;
            this.mealService = mealService;
        }

        /// <inheritdoc/>
        public async Task<AllRecipesFilteredAndPagedViewModel> CreateAllRecipesViewModelAsync(AllRecipesQueryModel queryModel, string userId)
        {
            
            var allRecipes = await recipeService.GetAllAsync(queryModel, userId);
            var categories = await categoryService.GetAllCategoryNamesAsync();
            
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

        /// <inheritdoc/>
        public async Task<RecipeAddFormModel> CreateRecipeAddFormModelAsync()
        {
            var addModel = await PreloadRecipeSelectOptionsToFormModel(new RecipeAddFormModel());

            if (addModel is RecipeAddFormModel model)
            {
                return model;
            }
            logger.LogError($"Type cast error: Unable to cast {addModel.GetType().Name} to {nameof(RecipeAddFormModel)}.");
            throw new InvalidCastException(InvalidCastExceptionMessages.RecipeAddOrEditModelUnsuccessfullyCasted);           
        }

        
        /// <inheritdoc/>
        public async Task<RecipeEditFormModel> CreateRecipeEditFormModelAsync(string recipeId, string userId, bool isAdmin)
        {
            RecipeEditFormModel editModel = await this.recipeService.GetForEditByIdAsync(recipeId, userId, isAdmin);
            
            var filledModel = await PreloadRecipeSelectOptionsToFormModel(editModel);
            if (filledModel is RecipeEditFormModel model)
            {
                return model;
            }

            logger.LogError($"Type cast error: Unable to cast {editModel.GetType().Name} to {nameof(RecipeEditFormModel)}.");
            throw new InvalidCastException(InvalidCastExceptionMessages.RecipeAddOrEditModelUnsuccessfullyCasted);
        }

        /// <summary>
        /// Generates a detailed view model for a recipe.
        /// </summary>
        public async Task<RecipeDetailsViewModel> CreateRecipeDetailsViewModelAsync(string recipeId, string userId)
        { 
            RecipeDetailsViewModel model = await this.recipeService.TryGetForDetailsByRecipeId(recipeId);
            model.IsLikedByUser = await this.recipeService.IsLikedByUserAsync(userId, recipeId);
            model.LikesCount = await this.recipeService.GetAllRecipeLikesAsync(recipeId);
            model.CookedCount = await this.recipeService.GetAllRecipeMealsCountAsync(recipeId);

            return model;
        }

        /// <summary>
        /// Generates a view model for a user's recipes, including owned and favorite recipes.
        /// </summary>
        public async Task<RecipeMineViewModel> CreateRecipeMineViewModelAsync(string userId)
        {
            RecipeMineViewModel model = new RecipeMineViewModel();

            model.FavouriteRecipes = await this.recipeService.AllLikedByUserAsync(userId);
            model.OwnedRecipes = await this.recipeService.GetAllAddedByUserIdAsync(userId);

            return model;
        }


        /// <inheritdoc/>
        public async Task<IRecipeFormModel> PreloadRecipeSelectOptionsToFormModel(IRecipeFormModel model)
        {
            model.Categories = await SafeExecuteAsync(
                async () => await categoryService.GetAllCategoriesAsync(),
                DataRetrievalExceptionMessages.CategoryDataRetrievalExceptionMessage
            );

            model.ServingsOptions = ServingsOptions;

            if (!model.RecipeIngredients.Any())
            {
                model.RecipeIngredients.Add(new RecipeIngredientFormModel());
            }

            if (!model.Steps.Any())
            {
                model.Steps.Add(new StepFormModel());
            }

            var measures = await SafeExecuteAsync(
                async () => await recipeIngredientService.GetRecipeIngredientMeasuresAsync(),
                DataRetrievalExceptionMessages.RecipeIngredientMeasuresDataRetrievalExceptionMessage
            );
            model.RecipeIngredients.First().Measures = measures;

            var specifications = await SafeExecuteAsync(
                async () => await recipeIngredientService.GetRecipeIngredientSpecificationsAsync(),
                DataRetrievalExceptionMessages.RecipeIngredientSpecificationsDataRetrievalExceptionMessage
            );
            model.RecipeIngredients.First().Specifications = specifications;

            return model;
        }

        /// <summary>
        /// A helper method to log errors upon asyncronous data retrieval from the database
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        /// <exception cref="DataRetrievalException"></exception>
        private async Task<T> SafeExecuteAsync<T>(Func<Task<T>> action, string errorMessage)
        {
            try
            {
                return await action();
            }
            catch (Exception ex)
            {
                logger.LogError($"{errorMessage} Error message: {ex.Message}. Error Stacktrace: {ex.StackTrace}");
                throw new DataRetrievalException(errorMessage, ex);
            }
        }
    }

    
}
