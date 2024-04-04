namespace CookTheWeek.Services.Interfaces
{
    using CookTheWeek.Web.ViewModels.Category;

    public interface ICategoryService
    {
        // Recipe Category
        Task<ICollection<RecipeCategorySelectViewModel>> AllRecipeCategoriesAsync();
        Task<ICollection<string>> AllRecipeCategoryNamesAsync();
        Task<bool> RecipeCategoryExistsByIdAsync(int recipeCategoryId);
        Task<int> AllRecipeCategoriesCountAsync();

        // Ingredient Category
        Task<ICollection<IngredientCategorySelectViewModel>> AllIngredientCategoriesAsync();
        Task<ICollection<string>> AllIngredientCategoryNamesAsync();
        Task<bool> IngredientCategoryExistsByIdAsync(int ingredientCategoryId);
        Task<int> AllIngredientCategoriesCountAsync();
    }
}
