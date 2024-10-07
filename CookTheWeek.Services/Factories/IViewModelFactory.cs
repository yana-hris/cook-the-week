namespace CookTheWeek.Services.Data.Factories
{
    using CookTheWeek.Services.Data.Models.MealPlan;
    using CookTheWeek.Web.ViewModels.Admin.MealPlanAdmin;
    using CookTheWeek.Web.ViewModels.Interfaces;
    using CookTheWeek.Web.ViewModels.Meal;
    using CookTheWeek.Web.ViewModels.MealPlan;
    using CookTheWeek.Web.ViewModels.Recipe;

    public interface IViewModelFactory
    {
        /// <summary>
        /// Generates a view model for displaying All Recipes View.
        /// </summary>
        /// <returns>AllRecipesFilteredAndPagedViewModel</returns>
        /// <remarks>May throw RecordNotFoundException in case of an empty result collection</remarks>
        Task<AllRecipesFilteredAndPagedViewModel> CreateAllRecipesViewModelAsync(AllRecipesQueryModel queryModel);

        /// <summary>
        /// Generates a form model for adding a new recipe with populated categories and ingredient options.
        /// Throws an exception if model casting goes wrong.
        /// </summary>
        /// <returns>RecipeAddFormModel</returns>
        /// <remarks>May throw a DataRetrievalException due to usage of PreloadRecipeSelectOptions method</remarks>
        /// <exception cref="InvalidCastException"></exception>
        Task<RecipeAddFormModel> CreateRecipeAddFormModelAsync();

        /// <summary>
        /// Generates a form model for editing an existing recipe, including categories and ingredient options, 
        /// recipe ingredient measures and specifications or throws an exception.
        /// </summary>
        /// <remarks>May throw RecordNotFoundException or UnauthorizedUserException due to GetForEditById method.
        /// May throw also DataRetrievalException due to preloading Categories, Measures and Specifications.</remarks>
        /// <exception cref="InvalidCastException"></exception>
        Task<RecipeEditFormModel> CreateRecipeEditFormModelAsync(string recipeId);

        /// <summary>
        /// Generates a detailed view model for a recipe or throws an exception
        /// </summary>
        /// <remarks>May throw a RecordNotFoundException due to usage of TryGetForDetails and GetById methods</remarks>
        /// <exception cref="DataRetrievalException"></exception>
        Task<RecipeDetailsViewModel> CreateRecipeDetailsViewModelAsync(string recipeId);

        /// <summary>
        /// Generates a view model for a user's recipes, including owned and favorite recipes or throws an Exception if collections are empty.
        /// </summary>
        /// <exception cref="RecordNotFoundException"></exception>
        Task<RecipeMineViewModel> CreateRecipeMineViewModelAsync();

        /// <summary>
        /// Generates a view model for a user`s meal by a given meal ID. May throw exceptions in case of missing data or database retrieval problem.
        /// </summary>
        /// <param name="mealId">Meal Id (int)</param>
        /// <returns>MealDetailsViewModel</returns>
        /// <exception cref="RecordNotFoundException"></exception>
        /// <exception cref="DataRetrievalException"></exception>
        Task<MealDetailsViewModel> CreateMealDetailsViewModelAsync(int mealId);

        /// <summary>
        /// Creates the model for meal plan Add View from the received Service model. 
        /// </summary>
        /// <param name="model"></param>
        /// <returns>MealPlanAddFormModel</returns>
        /// <remarks>May throw a RecordNotFound exception from the GetRecipeById method.</remarks>
        Task<MealPlanAddFormModel> CreateMealPlanAddFormModelAsync(MealPlanServiceModel model);


        /// <summary>
        /// A helper method that fills an existing IRecipeFormModel with the pre-defined select options for Recipe Categories and Serving Options (from the databse).
        /// Adds to the first RecipeIngredient the pre-defined select options for Measures and Specifications.
        /// If operations fail may throw an exception.
        /// </summary>
        /// <param name="model"></param>
        /// <returns>RecipeAddFormModel or RecipeEditFormModel (any IRecipeFormModel)</returns>
        /// <exception cref="DataRetrievalException"></exception>
        Task<IRecipeFormModel> PreloadRecipeSelectOptionsToFormModel(IRecipeFormModel model);

        /// <summary>
        /// Creates a MealAddFormModel from a service model. Throws an exception if the specific recipe is not found in the database
        /// </summary>
        /// <param name="meal"></param>
        /// <remarks>May throw a RecordNotFoundException due to usage of GetByIdAsync method.</remarks>
        /// <returns>MealAddFormModel</returns>
        Task<MealAddFormModel> CreateMealAddFormModelAsync(MealServiceModel meal);

        /// <summary>
        /// Creates a view model for editing an existing meal plan by ID. May throws exceptions
        /// </summary>
        /// <param name="mealplanId"></param>
        /// <returns>A single MealPlanEditFormModel</returns>
        /// <exception cref="RecordNotFoundException"></exception>
        Task<MealPlanEditFormModel> CreateMealPlanEditFormModelAsync(string mealplanId);

        /// <summary>
        /// Returns a single meal plan for Details View or throws an exception if the mealplan is not found
        /// </summary>
        /// <param name="id"></param>
        /// <returns>MealPlanDetailsViewModel</returns>
        /// <exception cref="RecordNotFoundException"></exception>
        Task<MealPlanDetailsViewModel> CreateMealPlanDetailsViewModelAsync(string mealplanId);

        /// <summary>
        /// Returns a collection of all user`s mealplans MealPlanAllViewModel or throws an exception if no meal plans found (collection is empty)
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>A collection of MealPlanAllViewModel</returns>
        /// <exception cref="RecordNotFoundException"></exception>
        Task<ICollection<MealPlanAllViewModel>> CreateMyMealPlansViewModelAsync();

        /// <summary>
        /// Returns a Viewmodel collection for Admin view of all active user meal plans at the moment
        /// </summary>
        /// <returns>A collection of MealPlanAllAdminViewModel</returns>
        Task<ICollection<MealPlanAllAdminViewModel>> CreateAllActiveMealPlansAdminViewModelAsync();

        /// <summary>
        /// Returns a Viewmodel collection for Admin view of all finished user meal plans at the moment
        /// </summary>
        /// <returns>A collection of MealPlanAllAdminViewModel</returns>
        Task<ICollection<MealPlanAllAdminViewModel>> CreateAllFinishedMealPlansAdminViewModelAsync();

        /// <summary>
        /// Creates an Add meal plan view model for Copy meal plan functionality (prefilled with the copied meal plan data and validated)
        /// </summary>
        /// <param name="mealplanId"></param>
        /// <returns></returns>
        Task<MealPlanAddFormModel> CreateMealPlanAddFormModelAsync(string mealplanId);

    }
}
