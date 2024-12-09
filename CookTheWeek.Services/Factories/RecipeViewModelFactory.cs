﻿namespace CookTheWeek.Services.Data.Factories
{
    using System.Threading.Tasks;

    using Microsoft.Extensions.Logging;

    using CookTheWeek.Common.Enums;
    using CookTheWeek.Common.Exceptions;
    using CookTheWeek.Data.Models;
    using CookTheWeek.Services.Data.Helpers;
    using CookTheWeek.Services.Data.Models.Recipe;
    using CookTheWeek.Services.Data.Services.Interfaces;
    using CookTheWeek.Web.ViewModels;
    using CookTheWeek.Web.ViewModels.Admin.CategoryAdmin;
    using CookTheWeek.Web.ViewModels.Interfaces;
    using CookTheWeek.Web.ViewModels.Recipe;
    using CookTheWeek.Web.ViewModels.Recipe.Enums;
    using CookTheWeek.Web.ViewModels.RecipeIngredient;
    using CookTheWeek.Web.ViewModels.Step;

    using static CookTheWeek.Common.EntityValidationConstants.RecipeValidation;
    using static CookTheWeek.Common.ExceptionMessagesConstants;
    using static CookTheWeek.Common.GeneralApplicationConstants;
    using static CookTheWeek.Common.HelperMethods.CookingTimeHelper;
    using static CookTheWeek.Services.Data.Helpers.EnumHelper;
   

    public class RecipeViewModelFactory : IRecipeViewModelFactory
    {
        private readonly IRecipeService recipeService;
        private readonly ITagService tagService;
        private readonly IMealPlanViewModelFactory mealplanFactory;
        private readonly ICategoryService<RecipeCategory, 
            RecipeCategoryAddFormModel, 
            RecipeCategoryEditFormModel, 
            SelectViewModel> categoryService;
        private readonly IRecipeIngredientService recipeIngredientService;
        private readonly IIngredientAggregatorHelper ingredientHelper;
        private readonly ILogger<RecipeViewModelFactory> logger;    
        private readonly bool hasActiveMealPlan;
        private readonly Guid userId;

        public RecipeViewModelFactory(IRecipeService recipeService,
                                      ICategoryService<RecipeCategory, 
                                          RecipeCategoryAddFormModel, 
                                          RecipeCategoryEditFormModel, 
                                          SelectViewModel> categoryService,
                                      IRecipeIngredientService recipeIngredientService,
                                      ILogger<RecipeViewModelFactory> logger,
                                      IMealPlanViewModelFactory mealplanFactory,
                                      IUserContext userContext,
                                      ITagService tagService,
                                      IIngredientAggregatorHelper ingredientHelper)
        {
            this.recipeService = recipeService;
            this.categoryService = categoryService;
            this.recipeIngredientService = recipeIngredientService;
            this.tagService = tagService;
            this.logger = logger;
            this.mealplanFactory = mealplanFactory;
            this.ingredientHelper = ingredientHelper;
            this.hasActiveMealPlan = userContext.HasActiveMealplan;
            this.userId = userContext.UserId;

        }

        /// <inheritdoc/>
        public async Task<AllRecipesFilteredAndPagedViewModel> CreateAllRecipesViewModelAsync(AllRecipesQueryModel queryModel, bool justLoggedIn)
        {
            
            ICollection<Recipe> allRecipes = await recipeService.GetAllAsync(queryModel);
            ICollection<SelectViewModel> mealTypes = await categoryService.GetAllCategoriesAsync();
            ICollection<SelectViewModel> allTags = await tagService.GetAllTagsAsync();

            var viewModel = new AllRecipesFilteredAndPagedViewModel
            {
                SearchString = queryModel.SearchString,
                MealTypeId = queryModel.MealTypeId,
                MaxPreparationTime = queryModel.MaxPreparationTime,
                DifficultyLevel = queryModel.DifficultyLevel,   
                SelectedTagIds = queryModel.SelectedTagIds,
                RecipeSorting = queryModel.RecipeSorting,
                RecipesPerPage = queryModel.RecipesPerPage,
                CurrentPage = queryModel.CurrentPage,
                TotalResults = queryModel.TotalResults,
                Recipes = MapRecipeCollectionToRecipeAllViewModelCollection(allRecipes),
                MealTypes = mealTypes,
                DifficultyLevels = GetEnumAsSelectViewModel<DifficultyLevel>(), 
                AvailableTags = allTags,
                RecipeSortings = GetEnumAsSelectViewModel<RecipeSorting>()
            };

            if (hasActiveMealPlan)
            {
                viewModel.ActiveMealPlan = await mealplanFactory.GetActiveDetails();
            }

            if (justLoggedIn)
            {
                viewModel.ActiveMealPlan.JustLoggedIn = true;
                viewModel.ActiveMealPlan.UserId = userId;
            }            

            return viewModel;
        }

        /// <inheritdoc/>
        public async Task<AllRecipesFilteredAndPagedViewModel> CreateCustomRecipesViewModelAsync(string? tagType, string? mealType, AllRecipesQueryModel queryModel)
        {
            ICollection<Recipe> recipes = new List<Recipe>();
            ICollection<SelectViewModel> mealTypes = await categoryService.GetAllCategoriesAsync();
            ICollection<SelectViewModel> allTags = await tagService.GetAllTagsAsync();

            if (!String.IsNullOrEmpty(mealType))
            {
                int customMealTypeId = mealTypes.FirstOrDefault(mt => mt.Name == mealType).Id;
                queryModel.MealTypeId = customMealTypeId;
            }

            if (!String.IsNullOrEmpty(tagType))
            {
                int customTagId = allTags.FirstOrDefault(tag => tag.Name == tagType).Id;

                if (queryModel.SelectedTagIds == null)
                {
                    queryModel.SelectedTagIds = new List<int>();
                }
                queryModel.SelectedTagIds.Add(customTagId);
            }

            
            recipes = await recipeService.GetAllAsync(queryModel);

            var viewModel = new AllRecipesFilteredAndPagedViewModel
            {
                SearchString = queryModel.SearchString,
                MealTypeId = queryModel.MealTypeId,
                MaxPreparationTime = queryModel.MaxPreparationTime,
                DifficultyLevel = queryModel.DifficultyLevel,
                SelectedTagIds = queryModel.SelectedTagIds,
                RecipeSorting = queryModel.RecipeSorting,
                RecipesPerPage = queryModel.RecipesPerPage,
                CurrentPage = queryModel.CurrentPage,
                TotalResults = queryModel.TotalResults,
                Recipes = MapRecipeCollectionToRecipeAllViewModelCollection(recipes),
                MealTypes = mealTypes,
                DifficultyLevels = GetEnumAsSelectViewModel<DifficultyLevel>(),
                AvailableTags = allTags,
                RecipeSortings = GetEnumAsSelectViewModel<RecipeSorting>()
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
                ImageUrl = recipe.ImageUrl,
                CookingTimeMinutes = recipe.TotalTimeMinutes,
                SelectedTagIds = recipe.RecipeTags.Select(s => s.TagId).ToList(),
                DifficultyLevelId = recipe.DifficultyLevel != null ? (int)recipe.DifficultyLevel : default,
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
            var serviceModel = await recipeService.GetForDetailsByIdAsync(recipeId);
            Recipe recipe = serviceModel.Recipe;

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
                DifficultyLevel = recipe.DifficultyLevel.ToString() ?? "N/A",
                TotalTime = FormatCookingTime(recipe.TotalTimeMinutes),
                ImageUrl = recipe.ImageUrl,
                CreatedOn = recipe.CreatedOn.ToString("dd-MM-yyyy"),
                CreatedBy = recipe.Owner.UserName!,
                CategoryName = recipe.Category.Name,
                LikesCount = serviceModel.LikesCount,
                CookedCount = serviceModel.CookedCount,
                IsLikedByUser = serviceModel.IsLikedByUser,
            };

            decimal servingSizeMultiplier = ingredientHelper
                .CalculateServingSizeMultiplier(recipe.Servings, recipe.Servings); // here is 1, but for clarity

            var adjustedIngredients = ingredientHelper.CreateAdjustedIngredientCollection(recipe.RecipesIngredients, servingSizeMultiplier);
            
            var ingredientsByCategories = await ingredientHelper
                .AggregateIngredientsByCategory<RecipeIngredientDetailsViewModel>(adjustedIngredients, RecipeAndMealDetailedProductListCategoryDictionary);

            model.RecipeIngredientsByCategories = ingredientsByCategories;

            return model;
           
        }

        /// <inheritdoc/>
        public async Task<RecipeMineViewModel> CreateRecipeMineViewModelAsync()
        {
            RecipeAllMineServiceModel serviceModel = await recipeService.GetAllMineAsync();

            RecipeMineViewModel model = new RecipeMineViewModel()
            {
                FavouriteRecipes = new HashSet<RecipeAllViewModel>(),
                OwnedRecipes = new HashSet<RecipeAllViewModel>()
            };
           
            if (serviceModel.LikedRecipeIds.Count ==  0 || serviceModel.AddedRecipeIds.Count == 0)
            {
                throw new RecordNotFoundException(RecordNotFoundExceptionMessages.NoRecipesFoundExceptionMessage, null);
            }

            var likedCollection = await recipeService.GetAllByIds(serviceModel.LikedRecipeIds);
            var addedCollection = await recipeService.GetAllByIds(serviceModel.AddedRecipeIds);

            model.FavouriteRecipes = MapRecipeCollectionToRecipeAllViewModelCollection(likedCollection);
            model.OwnedRecipes = MapRecipeCollectionToRecipeAllViewModelCollection(addedCollection);

            return model;
        }

        /// <inheritdoc/>
        public async Task<IRecipeFormModel> PopulateRecipeFormModelAsync(IRecipeFormModel model)
        {
            model.Categories = await categoryService.GetAllCategoriesAsync();

            model.AvailableTags = await tagService.GetAllTagsAsync();

            model.ServingsOptions = ServingsOptions;

            model.DifficultyLevels = GetEnumAsSelectViewModel<DifficultyLevel>();

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
        public async Task<ICollection<RecipeAllViewModel>> CreateAdminSiteRecipesViewModelAsync()
        {
            ICollection<Recipe> siteRecipes = await recipeService.GetAllSiteAsync();
            var model = MapRecipeCollectionToRecipeAllViewModelCollection(siteRecipes);

            return model;
        }

        // HELPER METHODS:
        
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
                IsSiteRecipe = recipe.IsSiteRecipe,
                ImageUrl = recipe.ImageUrl,
                Description = recipe.Description,
                MealType = recipe.Category.Name,
                Servings = recipe.Servings,
                CookingTime = FormatCookingTime(recipe.TotalTimeMinutes)
            }).ToList();
        }

       
    }

}
