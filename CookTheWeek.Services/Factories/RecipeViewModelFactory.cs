namespace CookTheWeek.Services.Data.Factories
{
    using System.Threading.Tasks;

    using Microsoft.Extensions.Logging;

    using CookTheWeek.Common.Exceptions;
    using CookTheWeek.Data.Models;
    using CookTheWeek.Services.Data.Services.Interfaces;
    using CookTheWeek.Services.Data.Helpers;
    using CookTheWeek.Web.ViewModels.Admin.CategoryAdmin;
    using CookTheWeek.Web.ViewModels.Category;
    using CookTheWeek.Web.ViewModels.Interfaces;
    using CookTheWeek.Web.ViewModels.Recipe;
    using CookTheWeek.Web.ViewModels.Recipe.Enums;
    using CookTheWeek.Web.ViewModels.RecipeIngredient;
    using CookTheWeek.Web.ViewModels.Step;

    using static CookTheWeek.Common.EntityValidationConstants.RecipeValidation;
    using static CookTheWeek.Common.ExceptionMessagesConstants;
    using static CookTheWeek.Common.GeneralApplicationConstants;
    using static CookTheWeek.Common.HelperMethods.CookingTimeHelper;
    using static CookTheWeek.Common.HelperMethods.EnumHelper;

    public class RecipeViewModelFactory : IRecipeViewModelFactory
    {
        private readonly IRecipeService recipeService;
        private readonly IFavouriteRecipeService favouriteRecipeService;
        private readonly ICategoryService<RecipeCategory, 
            RecipeCategoryAddFormModel, 
            RecipeCategoryEditFormModel, 
            RecipeCategorySelectViewModel> categoryService;
        private readonly IRecipeIngredientService recipeIngredientService;
        private readonly IMealService mealService;
        private readonly IIngredientAggregatorHelper ingredientHelper;
        private readonly ILogger<RecipeViewModelFactory> logger;    

        public RecipeViewModelFactory(IRecipeService recipeService,
                                      ICategoryService<RecipeCategory, 
                                          RecipeCategoryAddFormModel, 
                                          RecipeCategoryEditFormModel, 
                                          RecipeCategorySelectViewModel> categoryService,
                                      IRecipeIngredientService recipeIngredientService,
                                      IFavouriteRecipeService favouriteRecipeService,
                                      ILogger<RecipeViewModelFactory> logger,
                                      IIngredientAggregatorHelper ingredientHelper,
                                      IMealService mealService)
        {
            this.recipeService = recipeService;
            this.favouriteRecipeService = favouriteRecipeService;
            this.categoryService = categoryService;
            this.recipeIngredientService = recipeIngredientService;
            this.logger = logger;
            this.mealService = mealService;
            this.ingredientHelper = ingredientHelper;
        }

        /// <inheritdoc/>
        public async Task<AllRecipesFilteredAndPagedViewModel> CreateAllRecipesViewModelAsync(AllRecipesQueryModel queryModel)
        {
            
            var allRecipes = await recipeService.GetAllAsync(queryModel);
            var categories = await categoryService.GetAllCategoryNamesAsync();
            
            var viewModel = new AllRecipesFilteredAndPagedViewModel
            {
                Category = queryModel.Category,
                SearchString = queryModel.SearchString,
                RecipeSorting = queryModel.RecipeSorting,
                RecipesPerPage = queryModel.RecipesPerPage,
                CurrentPage = queryModel.CurrentPage,
                TotalRecipes = queryModel.TotalRecipes,
                Recipes = MapRecipeCollectionToRecipeAllViewModelCollection(allRecipes),
                Categories = categories,
                RecipeSortings = GetEnumValuesDictionary<RecipeSorting>()
            };

            return viewModel;
        }

        /// <inheritdoc/>
        public async Task<RecipeAddFormModel> CreateRecipeAddFormModelAsync()
        {
            var addModel = await PopulateRecipeFormModelAsync(new RecipeAddFormModel());

            if (addModel is RecipeAddFormModel model)
            {
                return model;
            }
            logger.LogError($"Type cast error: Unable to cast {addModel.GetType().Name} to {nameof(RecipeAddFormModel)}.");
            throw new InvalidCastException(InvalidCastExceptionMessages.RecipeAddOrEditModelUnsuccessfullyCasted);           
        }
        
        /// <inheritdoc/>
        public async Task<RecipeEditFormModel> CreateRecipeEditFormModelAsync(Guid recipeId)
        {
            Recipe recipe = await recipeService.GetForEditByIdAsync(recipeId);
            
            RecipeEditFormModel model = new RecipeEditFormModel()
            {
                Id = recipe.Id,
                Title = recipe.Title,
                Description = recipe.Description,
                Steps = recipe.Steps.Select(s => new StepFormModel()
                {
                    Id = s.Id,
                    Description = s.Description

                }).ToList(),
                Servings = recipe.Servings,
                CookingTimeMinutes = (int)recipe.TotalTime.TotalMinutes,
                ImageUrl = recipe.ImageUrl,
                RecipeCategoryId = recipe.CategoryId,
                RecipeIngredients = recipe.RecipesIngredients.Select(ri => new RecipeIngredientFormModel()
                {
                    IngredientId = ri.IngredientId,
                    Name = ri.Ingredient.Name,
                    Qty = RecipeIngredientQtyFormModel.ConvertFromDecimalQty(ri.Qty, ri.Measure.Name),
                    MeasureId = ri.MeasureId,
                    SpecificationId = ri.SpecificationId
                }).ToList()
            };

            model = (RecipeEditFormModel)await PopulateRecipeFormModelAsync(model);
            return model;
        }

        /// <inheritdoc/>
        public async Task<RecipeDetailsViewModel> CreateRecipeDetailsViewModelAsync(Guid recipeId)
        {
            Recipe recipe = await recipeService.GetForDetailsByIdAsync(recipeId);

            RecipeDetailsViewModel model = new RecipeDetailsViewModel()
            {
                Id = recipe.Id.ToString(),
                Title = recipe.Title,
                Description = recipe.Description,
                Steps = recipe.Steps.Select(s => new StepViewModel()
                {
                    Id = s.Id,
                    Description = s.Description
                }).ToList(),
                Servings = recipe.Servings,
                IsSiteRecipe = recipe.IsSiteRecipe,
                TotalTime = FormatCookingTime(recipe.TotalTime),
                ImageUrl = recipe.ImageUrl,
                CreatedOn = recipe.CreatedOn.ToString("dd-MM-yyyy"),
                CreatedBy = recipe.Owner.UserName!,
                CategoryName = recipe.Category.Name,
            };

            decimal servingSizeMultiplier = ingredientHelper
                .CalculateServingSizeMultiplier(recipe.Servings, recipe.Servings); // here is 1, but for clarity

            var adjustedIngredients = ingredientHelper.CreateAdjustedIngredientCollection(recipe.RecipesIngredients, servingSizeMultiplier);
            
            var ingredientsByCategories = await ingredientHelper
                .AggregateIngredientsByCategory<RecipeIngredientDetailsViewModel>(adjustedIngredients, RecipeAndMealDetailedProductListCategoryDictionary);

            model.RecipeIngredientsByCategories = ingredientsByCategories;


            model.IsLikedByUser = await SafeExecuteAsync(
            async () => (await this.favouriteRecipeService.GetRecipeLikeIfExistsAsync(recipeId) != null),
            DataRetrievalExceptionMessages.FavouriteRecipeDataRetrievalExceptionMessage
            );

            model.LikesCount = await SafeExecuteAsync(
                async () => await this.favouriteRecipeService.GetRecipeTotalLikesAsync(recipeId),
                DataRetrievalExceptionMessages.RecipeTotalLikesDataRetrievalExceptionMessage
                );

            model.CookedCount = await SafeExecuteAsync(
                async () => await this.mealService.GetAllMealsCountByRecipeIdAsync(recipeId),
                DataRetrievalExceptionMessages.MealsTotalCountDataRetrievalExceptionMessage
                );

            return model;
           
        }

        /// <inheritdoc/>
        public async Task<RecipeMineViewModel> CreateRecipeMineViewModelAsync()
        {
            RecipeMineViewModel model = new RecipeMineViewModel()
            {
                FavouriteRecipes = new HashSet<RecipeAllViewModel>(),
                OwnedRecipes = new HashSet<RecipeAllViewModel>(),
            };

            var allLikedByUserIds = await favouriteRecipeService.GetAllRecipeIdsLikedByCurrentUserAsync();
            var allAddedByUserIds = await recipeService.GetAllRecipeIdsAddedByCurrentUserAsync();

            if (allLikedByUserIds.Count ==  0 || allAddedByUserIds.Count == 0)
            {
                throw new RecordNotFoundException(RecordNotFoundExceptionMessages.NoRecipesFoundExceptionMessage, null);
            }

            var likedCollection = await recipeService.GetAllByIds(allLikedByUserIds);
            var addedCollection = await recipeService.GetAllByIds(allAddedByUserIds);

            model.FavouriteRecipes = MapRecipeCollectionToRecipeAllViewModelCollection(likedCollection);
            model.OwnedRecipes = MapRecipeCollectionToRecipeAllViewModelCollection(addedCollection);

            return model;
        }

        /// <inheritdoc/>
        public async Task<IRecipeFormModel> PopulateRecipeFormModelAsync(IRecipeFormModel model)
        {
            model.Categories = await categoryService.GetAllCategoriesAsync();

            model.ServingsOptions = ServingsOptions;

            if (!model.RecipeIngredients.Any())
            {
                model.RecipeIngredients.Add(new RecipeIngredientFormModel());
            }

            if (!model.Steps.Any())
            {
                model.Steps.Add(new StepFormModel());
            }

            model.RecipeIngredients.First().Measures = await recipeIngredientService.GetRecipeIngredientMeasuresAsync();

            model.RecipeIngredients.First().Specifications = await recipeIngredientService.GetRecipeIngredientSpecificationsAsync();

            return model;
        }

        /// <inheritdoc/>
        public async Task<RecipeMineAdminViewModel> CreateAdminAllRecipesViewModelAsync()
        {
            RecipeMineAdminViewModel model = new RecipeMineAdminViewModel();

            ICollection<Recipe> siteRecipes = await recipeService.GetAllSiteAsync();
            ICollection<Recipe> userRecipes = await recipeService.GetAllNonSiteAsync();

            model.SiteRecipes = MapRecipeCollectionToRecipeAllViewModelCollection(siteRecipes);
            model.UserRecipes = MapRecipeCollectionToRecipeAllViewModelCollection(userRecipes);

            return model;
        }

        /// <inheritdoc/>
        public async Task<ICollection<RecipeAllViewModel>> CreateAdminSiteRecipesViewModelAsync()
        {
            ICollection<Recipe> siteRecipes = await recipeService.GetAllSiteAsync();
            var model = MapRecipeCollectionToRecipeAllViewModelCollection(siteRecipes);

            return model;
        }

        // HELPER METHODS:

        /// <summary>
        /// A helper method to log errors upon asyncronous data retrieval from the database and throw exceptions if needed
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action">the asynchronous function for data retrieval</param>
        /// <param name="errorMessage">The message to log and pass in case of DataRetrievalException</param>
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

       
        /// <summary>
        /// A helper method that maps a Recipe collection to a RecipeAllViewModel collection
        /// </summary>
        /// <param name="recipe">A given collection of Recipes</param>
        /// <returns>A collection of RecipeAllViewModel</returns>
        private ICollection<RecipeAllViewModel> MapRecipeCollectionToRecipeAllViewModelCollection(ICollection<Recipe> recipes)
        {
            return recipes.Select(recipe => new RecipeAllViewModel
            {
                Id = recipe.Id,
                OwnerId = recipe.OwnerId,
                Title = recipe.Title,
                ImageUrl = recipe.ImageUrl,
                Description = recipe.Description,
                Category = new RecipeCategorySelectViewModel
                {
                    Id = recipe.CategoryId,
                    Name = recipe.Category.Name
                },
                Servings = recipe.Servings,
                CookingTime = FormatCookingTime(recipe.TotalTime)
            }).ToList();
        }

        
    }

}
