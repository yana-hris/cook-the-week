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
        Task<bool> RecipeCategoryExistsByNameAsync(string name);
        Task AddRecipeCategoryAsync(RecipeCategoryAddFormModel model);
        Task<RecipeCategoryEditFormModel> GetRecipeCategoryForEditById(int id);
        Task<int> GetRecipeCategoryIdByNameAsync(string name);
        Task EditRecipeCategoryAsync(RecipeCategoryEditFormModel model);
        Task DeleteRecipeCategoryById(int id);
    }
}
