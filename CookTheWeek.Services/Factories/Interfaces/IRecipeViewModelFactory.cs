namespace CookTheWeek.Services.Data.Factories.Interfaces
{
    using CookTheWeek.Web.ViewModels.Recipe;

    public interface IRecipeViewModelFactory
    {
        /// <summary>
        /// Generates a view model for displaying all recipes with filtering and sorting.
        /// </summary>
        Task<AllRecipesFilteredAndPagedViewModel> CreateAllRecipesViewModelAsync(AllRecipesQueryModel queryModel, string userId);

        /// <summary>
        /// Generates a form model for adding a new recipe with populated categories and ingredient options.
        /// </summary>
        Task<RecipeAddFormModel> CreateRecipeAddFormModelAsync();

        /// <summary>
        /// Generates a form model for editing an existing recipe, including categories and ingredient options.
        /// </summary>
        Task<RecipeEditFormModel> CreateRecipeEditFormModelAsync(string recipeId, string userId, bool isAdmin);

        /// <summary>
        /// Generates a detailed view model for a recipe.
        /// </summary>
        Task<RecipeDetailsViewModel> CreateRecipeDetailsViewModelAsync(string recipeId, string userId);

        /// <summary>
        /// Generates a view model for a user's recipes, including owned and favorite recipes.
        /// </summary>
        Task<RecipeMineViewModel> CreateRecipeMineViewModelAsync(string userId);
    }
}
