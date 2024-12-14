namespace CookTheWeek.Services.Data.Factories
{
    using CookTheWeek.Web.ViewModels.Interfaces;
    using CookTheWeek.Web.ViewModels.Recipe;

    public interface IRecipeViewModelFactory
    {
        /// <summary>
        /// Generates a view model for displaying All Recipes View.
        /// </summary>
        /// <returns>AllRecipesFilteredAndPagedViewModel</returns>
        /// <remarks>May throw RecordNotFoundException in case of an empty result collection</remarks>
        Task<AllRecipesFilteredAndPagedViewModel> CreateAllRecipesViewModelAsync(AllRecipesQueryModel queryModel, bool justLoggedIn);

        /// <summary>
        /// Generates a view model for displaying custom Recipe views (Quick Dinners, Kids-friendly, etc.)
        /// </summary>
        /// <param name="queryModel"></param>
        /// <returns></returns>
        Task<AllRecipesFilteredAndPagedViewModel> CreateCustomRecipesViewModelAsync(string? queryType, string? mealType, AllRecipesQueryModel queryModel);

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
        Task<RecipeEditFormModel> CreateRecipeEditFormModelAsync(Guid recipeId);

        /// <summary>
        /// Generates a detailed view model for a recipe or throws an exception
        /// </summary>
        /// <remarks>May throw a RecordNotFoundException due to usage of TryGetForDetails and GetById methods</remarks>
        /// <exception cref="DataRetrievalException"></exception>
        Task<RecipeDetailsViewModel> CreateRecipeDetailsViewModelAsync(Guid recipeId);

        /// <summary>
        /// Generates a view model for a user's recipes, including owned and favorite recipes or throws an Exception if collections are empty.
        /// </summary>
        /// <exception cref="RecordNotFoundException"></exception>
        Task<RecipeMineViewModel> CreateRecipeMineViewModelAsync();

        /// <summary>
        /// A helper method that fills an existing IRecipeFormModel with the pre-defined select options for Recipe Categories and Serving Options (from the databse).
        /// Adds to the first RecipeIngredient the pre-defined select options for Measures and Specifications.
        /// If operations fail may throw an exception.
        /// </summary>
        /// <param name="model"></param>
        /// <returns>RecipeAddFormModel or RecipeEditFormModel (any IRecipeFormModel)</returns>
        /// <exception cref="DataRetrievalException"></exception>
        Task<IRecipeFormModel> PopulateRecipeFormModelAsync(IRecipeFormModel model);
    }
}
