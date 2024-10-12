namespace CookTheWeek.Services.Data.Factories
{
    using System.Globalization;
    using System.Threading.Tasks;

    using Microsoft.Extensions.Logging;

    using CookTheWeek.Common.Exceptions;
    using CookTheWeek.Common.Extensions;
    using CookTheWeek.Common.HelperMethods;
    using CookTheWeek.Data.Models;
    using CookTheWeek.Services.Data.Services.Interfaces;
    using CookTheWeek.Services.Data.Helpers;
    using CookTheWeek.Services.Data.Models.MealPlan;
    using CookTheWeek.Services.Data.Models.SupplyItem;
    using CookTheWeek.Web.ViewModels.Admin.CategoryAdmin;
    using CookTheWeek.Web.ViewModels.Admin.MealPlanAdmin;
    using CookTheWeek.Web.ViewModels.Category;
    using CookTheWeek.Web.ViewModels.Interfaces;
    using CookTheWeek.Web.ViewModels.Meal;
    using CookTheWeek.Web.ViewModels.MealPlan;
    using CookTheWeek.Web.ViewModels.Recipe;
    using CookTheWeek.Web.ViewModels.Recipe.Enums;
    using CookTheWeek.Web.ViewModels.RecipeIngredient;
    using CookTheWeek.Web.ViewModels.Step;

    using static CookTheWeek.Common.EntityValidationConstants.RecipeValidation;
    using static CookTheWeek.Common.ExceptionMessagesConstants;
    using static CookTheWeek.Common.GeneralApplicationConstants;
    using static CookTheWeek.Common.HelperMethods.CookingTimeHelper;
    using static CookTheWeek.Common.HelperMethods.EnumHelper;

    public class ViewModelFactory : IViewModelFactory
    {
        private readonly IRecipeService recipeService;
        private readonly IFavouriteRecipeService favouriteRecipeService;
        private readonly ICategoryService<RecipeCategory, 
            RecipeCategoryAddFormModel, 
            RecipeCategoryEditFormModel, 
            RecipeCategorySelectViewModel> categoryService;
        private readonly IRecipeIngredientService recipeIngredientService;
        private readonly IMealService mealService;
        private readonly IMealPlanService mealplanService;
        private readonly IIngredientAggregatorHelper ingredientHelper;
        private readonly ILogger<ViewModelFactory> logger;    

        public ViewModelFactory(IRecipeService recipeService,
                                      ICategoryService<RecipeCategory, 
                                          RecipeCategoryAddFormModel, 
                                          RecipeCategoryEditFormModel, 
                                          RecipeCategorySelectViewModel> categoryService,
                                      IRecipeIngredientService recipeIngredientService,
                                      IFavouriteRecipeService favouriteRecipeService,
                                      ILogger<ViewModelFactory> logger,
                                      IMealPlanService mealplanService,
                                      IIngredientAggregatorHelper ingredientHelper,
                                      IMealService mealService)
        {
            this.recipeService = recipeService;
            this.favouriteRecipeService = favouriteRecipeService;
            this.categoryService = categoryService;
            this.recipeIngredientService = recipeIngredientService;
            this.mealplanService = mealplanService;
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
        public async Task<RecipeEditFormModel> CreateRecipeEditFormModelAsync(Guid recipeId)
        {
            RecipeEditFormModel editModel = await recipeService.GetForEditByIdAsync(recipeId);
            
            var filledModel = await PreloadRecipeSelectOptionsToFormModel(editModel);
            if (filledModel is RecipeEditFormModel model)
            {
                return model;
            }

            logger.LogError($"Type cast error: Unable to cast {editModel.GetType().Name} to {nameof(RecipeEditFormModel)}.");
            throw new InvalidCastException(InvalidCastExceptionMessages.RecipeAddOrEditModelUnsuccessfullyCasted);
        }

        /// <inheritdoc/>
        public async Task<RecipeDetailsViewModel> CreateRecipeDetailsViewModelAsync(Guid recipeId)
        {
            Recipe recipe = await recipeService.GetByIdForDetailsAsync(recipeId);

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

            var ingredients = ProcessIngredients(recipe.RecipesIngredients, 1);
            var measures = await SafelyPreLoadMeasuresAsync();
            var specifications = await SafelyPreloadSpecificationsAsync();


            // TODO: check if this works as expected
            IEnumerable<ISupplyItemListModel<RecipeIngredientDetailsViewModel>> ingredientsByCategories = ingredientHelper
                .AggregateIngredientsByCategory<RecipeIngredientDetailsViewModel>(ingredients, measures, specifications, RecipeAndMealDetailedProductListCategoryDictionary);

            model.RecipeIngredientsByCategories = ingredientsByCategories;


            model.IsLikedByUser = await SafeExecuteAsync(
            async () => await this.favouriteRecipeService.HasUserByIdLikedRecipeById(recipeId),
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
        public async Task<IRecipeFormModel> PreloadRecipeSelectOptionsToFormModel(IRecipeFormModel model)
        {
            model.Categories = await SafelyPreloadRecipeCategoriesAsync();

            model.ServingsOptions = ServingsOptions;

            if (!model.RecipeIngredients.Any())
            {
                model.RecipeIngredients.Add(new RecipeIngredientFormModel());
            }

            if (!model.Steps.Any())
            {
                model.Steps.Add(new StepFormModel());
            }

            model.RecipeIngredients.First().Measures = await SafelyPreLoadMeasuresAsync();

            model.RecipeIngredients.First().Specifications = await SafelyPreloadSpecificationsAsync();

            return model;
        }
        
        /// <inheritdoc/>
        public async Task<MealDetailsViewModel> CreateMealDetailsViewModelAsync(int mealId)
        {
            Meal meal = await mealService.GetByIdAsync(mealId);

            decimal servingSizeMultiplier = CalculateServingSizeMultiplier(meal.ServingSize, meal.Recipe.Servings);

            // Process ingredients
            var ingredients = ProcessIngredients(meal.Recipe.RecipesIngredients, servingSizeMultiplier);

            // Group ingredients by categories and build the model
            var model = await BuildMealDetailsViewModel(meal, ingredients);

            return model;
        }

        /// <inheritdoc/>
        public async Task<MealPlanAddFormModel> CreateMealPlanAddFormModelAsync(MealPlanServiceModel serviceModel)
        {
            MealPlanAddFormModel model = new MealPlanAddFormModel();

            // Ensure all fields are initiated and filled with correct data
            if (model.StartDate == default)
            {
                model.StartDate = DateTime.Now;
            }

            if (model.Meals == null)
            {
                model.Meals = new List<MealAddFormModel>();
            }

            foreach (var meal in serviceModel.Meals)
            {
                MealAddFormModel currentMeal = await CreateMealAddFormModelAsync(meal);
                model.Meals.Add(currentMeal);
            }

            return model;
        }

        /// <inheritdoc/>
        public async Task<MealAddFormModel> CreateMealAddFormModelAsync(MealServiceModel meal)
        {
            // Retrieve the recipe from database
            if (Guid.TryParse(meal.RecipeId, out Guid guidMealId))
            {
                Recipe recipe = await recipeService.GetForMealByIdAsync(guidMealId);

                MealAddFormModel model = new MealAddFormModel()
                {
                    RecipeId = recipe.Id,
                    Title = recipe.Title,
                    Servings = recipe.Servings,
                    ImageUrl = recipe.ImageUrl,
                    CategoryName = recipe.Category.Name,
                };

                // Make sure all select menus are filled with data
                if (model.SelectDates == null || model.SelectDates.Count() == 0)
                {
                    model.SelectDates = DateGenerator.GenerateNext7Days();
                }

                if (model.SelectServingOptions == null || model.SelectServingOptions.Count() == 0)
                {
                    model.SelectServingOptions = ServingsOptions;
                }
                model.Date = model.SelectDates!.First();

                return model;
            }
            
            throw new RecordNotFoundException(RecordNotFoundExceptionMessages.RecipeNotFoundExceptionMessage, null);

        }

        /// <inheritdoc/>
        public async Task<MealPlanDetailsViewModel> CreateMealPlanDetailsViewModelAsync(Guid mealplanId)
        {
            MealPlan mealPlan = await mealplanService.TryGetAsync(mealplanId);

            MealPlanDetailsViewModel model = new MealPlanDetailsViewModel()
            {
                Id = mealPlan.Id.ToString(),
                Name = mealPlan.Name,
                OwnerId = mealPlan.OwnerId,
                IsFinished = mealPlan.IsFinished,
                Meals = mealPlan.Meals.Select(mpm => new MealViewModel()
                {
                    Id = mpm.Id.ToString(),
                    RecipeId = mpm.RecipeId.ToString(),
                    Title = mpm.Recipe.Title,
                    Servings = mpm.ServingSize,
                    ImageUrl = mpm.Recipe.ImageUrl,
                    CategoryName = mpm.Recipe.Category.Name,
                    Date = mpm.CookDate.ToString(MealDateFormat),
                }).ToList(),
                TotalServings = mealPlan.Meals.Sum(mpm => mpm.ServingSize),
                TotalCookingDays = mealPlan.Meals.Select(mpm => mpm.CookDate.Date).Distinct().Count(),
                TotalIngredients = mealPlan.Meals.Sum(m => m.Recipe.RecipesIngredients.Count),
                TotalCookingTimeMinutes = mealPlan.Meals.Sum(m => (int)m.Recipe.TotalTime.TotalMinutes),
            };

            return model;
        }

        /// <inheritdoc/>
        public async Task<ICollection<MealPlanAllViewModel>> CreateMyMealPlansViewModelAsync()
        {
            var userMealplans = await mealplanService.GetAllMineAsync();

            var model = userMealplans
               .Select(mp => new MealPlanAllViewModel()
               {
                   Id = mp.Id.ToString(),
                   Name = mp.Name,
                   StartDate = mp.StartDate.ToString(MealDateFormat, CultureInfo.InvariantCulture),
                   EndDate = mp.StartDate.AddDays(6.00).ToString(MealDateFormat, CultureInfo.InvariantCulture),
                   MealsCount = mp.Meals.Count,
                   IsFinished = mp.IsFinished
               }).ToList();

            return model;
        }

        /// <inheritdoc/>
        public async Task<ICollection<MealPlanAllAdminViewModel>> CreateAllActiveMealPlansAdminViewModelAsync()
        {
            ICollection<MealPlan> allActiveUserMealplans = await mealplanService.GetAllActiveAsync();

            var model = allActiveUserMealplans
                .Select(mp => new MealPlanAllAdminViewModel()
                {
                    Id = mp.Id.ToString(),
                    Name = mp.Name.TrimToChar(30),
                    OwnerUsername = mp.Owner.UserName!,
                    StartDate = mp.StartDate.ToString(MealDateFormat, CultureInfo.InvariantCulture),
                    EndDate = mp.StartDate.AddDays(6.00).ToString(MealDateFormat, CultureInfo.InvariantCulture),
                    MealsCount = mp.Meals.Count
                }).ToList();

            return model;
        }

        /// <inheritdoc/>
        public async Task<ICollection<MealPlanAllAdminViewModel>> CreateAllFinishedMealPlansAdminViewModelAsync()
        {
            ICollection<MealPlan> allFinishedUserMealplans = await mealplanService.GetAllFinishedAsync();

            var model = allFinishedUserMealplans
                .Select(mp => new MealPlanAllAdminViewModel()
                {
                    Id = mp.Id.ToString(),
                    Name = mp.Name.TrimToChar(30),
                    OwnerUsername = mp.Owner.UserName!,
                    StartDate = mp.StartDate.ToString(MealDateFormat, CultureInfo.InvariantCulture),
                    EndDate = mp.StartDate.AddDays(6.00).ToString(MealDateFormat, CultureInfo.InvariantCulture),
                    MealsCount = mp.Meals.Count
                }).ToList();

            return model;
        }

        /// <inheritdoc/>
        public async Task<TFormModel> CreateMealPlanFormModelAsync<TFormModel>(Guid id)
            where TFormModel : IMealPlanFormModel, new()
        {
            MealPlan mealplan = await mealplanService.TryGetAsync(id);

            if (typeof(TFormModel) == typeof(MealPlanAddFormModel))
            {
                return (TFormModel)(object)MapMealPlanToAddModel(mealplan);
            }
            else if (typeof(TFormModel) == typeof(MealPlanEditFormModel))
            {
                return (TFormModel)(object)MapMealPlanToEditModel(mealplan);
            }

            throw new InvalidOperationException("Unsupported form model type.");
        }

       

        // HELPER METHODS:

        /// <summary>
        /// Utility generic common mapping method
        /// </summary>
        /// <param name="mealplan"></param>
        /// <returns>T => An implementation of IMealPlanFormModel</returns>
        private static T MapMealPlanToFormModel<T>(MealPlan mealplan) where T : IMealPlanFormModel, new()
        {
            var model = new T()
            {
                Name = mealplan.Name,
                StartDate = mealplan.StartDate,
                Meals = mealplan.Meals.Select(mpm => new MealAddFormModel()
                {
                    RecipeId = mpm.RecipeId,
                    Title = mpm.Recipe.Title,
                    Servings = mpm.ServingSize,
                    ImageUrl = mpm.Recipe.ImageUrl,
                    CategoryName = mpm.Recipe.Category.Name,
                    Date = mpm.CookDate.ToString(MealDateFormat),
                    SelectServingOptions = ServingsOptions
                }).ToList(),
            };

            model.Meals.First().SelectDates = DateGenerator.GenerateNext7Days(model.StartDate);
            return model;
        }

        /// <summary>
        /// Utility method for mapping the Edit Form model (for edit case)
        /// </summary>
        /// <param name="mealPlan"></param>
        /// <returns></returns>
        private static MealPlanEditFormModel MapMealPlanToEditModel(MealPlan mealPlan)
        {
            var model = MapMealPlanToFormModel<MealPlanEditFormModel>(mealPlan);
            model.Id = mealPlan.Id;
            return model;
        }


        /// <summary>
        /// Utility method for mapping the Add Form model (for copy case)
        /// </summary>
        /// <param name="mealPlan"></param>
        /// <returns></returns>
        private static MealPlanAddFormModel MapMealPlanToAddModel(MealPlan mealPlan)
        {
            var model = MapMealPlanToFormModel<MealPlanAddFormModel>(mealPlan);

            model.StartDate = DateTime.Today;
            model.Name = $"{mealPlan.Name} (Copy)";

            model.Meals.First().SelectDates = DateGenerator.GenerateNext7Days();

            foreach (var meal in model.Meals)
            {
                meal.Date = model.StartDate.ToString(MealDateFormat);
            }

            return model;
        }

        /// <summary>
        /// A helper method which preloads a collection with view model of all existing Recipe Categories, used in select option menus.
        /// </summary>
        /// <returns>A collection of Recipe Categories of type RecipeCategorySelectViewModel</returns>
        /// <exception cref="DataRetrievalException"></exception>
        private async Task<ICollection<RecipeCategorySelectViewModel>> SafelyPreloadRecipeCategoriesAsync()
        {
            return await SafeExecuteAsync(
                async () => await categoryService.GetAllCategoriesAsync(),
                DataRetrievalExceptionMessages.CategoryDataRetrievalExceptionMessage
            );
        }


        /// <summary>
        /// A helper method which preloads a collection with view model of all existing Recipe Ingredient Specifications, used in select option menus.
        /// </summary>
        /// <returns>A collection of Specifications of type RecipeIngredientSelectSpecificationViewModel</returns>
        /// <exception cref="DataRetrievalException"></exception>
        private async Task<ICollection<RecipeIngredientSelectSpecificationViewModel>> SafelyPreloadSpecificationsAsync()
        {
            return await SafeExecuteAsync(
                async () => await recipeIngredientService.GetRecipeIngredientSpecificationsAsync(),
                DataRetrievalExceptionMessages.RecipeIngredientSpecificationsDataRetrievalExceptionMessage
            );
        }

        /// <summary>
        /// A helper method which preloads a collection of view model of all existing Recipe Ingredient Measures, used in select option menus.
        /// </summary>
        /// <returns>A collection of Measures of type RecipeIngredientSelectMeasureViewModel</returns>
        /// <exception cref="DataRetrievalException"></exception>
        private async Task<ICollection<RecipeIngredientSelectMeasureViewModel>> SafelyPreLoadMeasuresAsync()
        {
            return await SafeExecuteAsync(
                async () => await recipeIngredientService.GetRecipeIngredientMeasuresAsync(),
                DataRetrievalExceptionMessages.RecipeIngredientMeasuresDataRetrievalExceptionMessage
            );
        }


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
        /// Builds the viewmodel to return to Details view
        /// </summary>
        /// <param name="meal"></param>
        /// <param name="recipe"></param>
        /// <param name="ingredients"></param>
        /// <returns>A MealDetailsViewModel</returns>
        private async Task<MealDetailsViewModel> BuildMealDetailsViewModel(Meal meal, List<SupplyItemServiceModel> ingredients)
        {
            var measures = await SafelyPreLoadMeasuresAsync();
            var specifications = await SafelyPreloadSpecificationsAsync();


            // TODO: check if this works as expected
            IEnumerable<ISupplyItemListModel<MealIngredientDetailsViewModel>> ingredientsByCategories = ingredientHelper
                .AggregateIngredientsByCategory<MealIngredientDetailsViewModel>(ingredients, measures, specifications, RecipeAndMealDetailedProductListCategoryDictionary);
                

            return new MealDetailsViewModel
            {
                Id = meal.Id,
                Title = meal.Recipe.Title,
                ImageUrl = meal.Recipe.ImageUrl,
                Description = meal.Recipe.Description,
                CookingTime = meal.Recipe.TotalTime,
                CategoryName = meal.Recipe.Category.Name,
                CookingSteps = meal.Recipe.Steps.Select(st => new StepViewModel
                {
                    Id = st.Id,
                    Description = st.Description
                }).ToList(),
                ServingSize = meal.ServingSize,
                IngredientsByCategories = ingredientsByCategories
            };
        }

        /// <summary>
        /// Helper method that calculates the serving size multiplier for a given meal 
        /// </summary>
        /// <param name="mealServingSize"></param>
        /// <param name="recipeServings"></param>
        /// <returns>decimal</returns>
        private decimal CalculateServingSizeMultiplier(int mealServingSize, int recipeServings)
        {
            return mealServingSize * 1.0m / recipeServings * 1.0m;
        }

        /// <summary>
        /// Processes the ingredient qty for each meal ingredient according to a serving size multiplier and returns a collection of Service Model
        /// </summary>
        /// <param name="recipeIngredients"></param>
        /// <param name="servingSizeMultiplier"></param>
        /// <returns>A collection of ingredients of type ProductServiceModel</returns>
        private List<SupplyItemServiceModel> ProcessIngredients(IEnumerable<RecipeIngredient> recipeIngredients, decimal servingSizeMultiplier)
        {
            var ingredients = recipeIngredients.Select(ri => new SupplyItemServiceModel()
            {
                CategoryId = ri.Ingredient.CategoryId,
                Name = ri.Ingredient.Name,
                Qty = ri.Qty * servingSizeMultiplier,
                MeasureId = ri.MeasureId,
                SpecificationId = ri.SpecificationId
            }).ToList();

            return ingredients;
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
                Title = recipe.Title,
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

        /// <summary>
        /// Processes the ingredients by groups and returns a collection of collections of ingredients, grouped by categories, defined in a constant dictionary
        /// </summary>
        /// <param name="ingredients"></param>
        /// <param name="measures"></param>
        /// <param name="specifications"></param>
        /// <returns>A collection of ProductListViewModels</returns>
        /// 
        // TODO: Check if the newly implemented method AggregateIngredientsByCategory works right before deleting the old logic
        //private ICollection<ProductListViewModel> BuildIngredientsByCategories(List<ProductServiceModel> ingredients, ICollection<RecipeIngredientSelectMeasureViewModel> measures, ICollection<RecipeIngredientSelectSpecificationViewModel> specifications)
        //{
        //    var ingredientsByCategories = new List<ProductListViewModel>();

        //    for (int i = 0; i < ProductListCategoryNames.Length; i++)
        //    {
        //        int[] categoriesArr = ProductListCategoryIds[i];

        //        ProductListViewModel productListViewModel = new ProductListViewModel
        //        {
        //            Title = ProductListCategoryNames[i],
        //            Products = ingredients
        //                .Where(p => categoriesArr.Contains(p.CategoryId))
        //                .Select(p => new ProductViewModel
        //                {
        //                    Qty = FormatIngredientQty(p.Qty),
        //                    Measure = measures.First(m => m.Id == p.MeasureId).Name,
        //                    Name = p.Name,
        //                    Specification = specifications.First(sp => sp.Id == p.SpecificationId).Name ?? ""
        //                }).ToList()
        //        };

        //        ingredientsByCategories.Add(productListViewModel);
        //    }

        //    return ingredientsByCategories;
        //}
    }


}
