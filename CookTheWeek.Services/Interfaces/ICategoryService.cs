namespace CookTheWeek.Services.Interfaces
{
    using CookTheWeek.Web.ViewModels.Category;

    public interface ICategoryService
    {
        // Recipe Category
        Task<ICollection<RecipeCategorySelectViewModel>> AllRecipeCategoriesAsync();
        Task AddRecipeCategoryAsync(RecipeCategoryAddFormModel model);
        Task EditRecipeCategoryAsync(RecipeCategoryEditFormModel model);
        Task DeleteRecipeCategoryById(int id);
        Task<RecipeCategoryEditFormModel> GetRecipeCategoryForEditByIdAsync(int id);
        Task<bool> RecipeCategoryExistsByIdAsync(int recipeCategoryId);
        Task<bool> RecipeCategoryExistsByNameAsync(string name);
        Task<int> GetRecipeCategoryIdByNameAsync(string name);
        Task<ICollection<string>> AllRecipeCategoryNamesAsync();
        Task<int> AllRecipeCategoriesCountAsync();

        // Ingredient Category
        Task<ICollection<IngredientCategorySelectViewModel>> AllIngredientCategoriesAsync();

        //Add
        //Edit
        //Delete
        Task<bool> IngredientCategoryExistsByIdAsync(int ingredientCategoryId);
        Task<ICollection<string>> AllIngredientCategoryNamesAsync();
        Task<int> AllIngredientCategoriesCountAsync();
        Task<bool> IngredientCategoryExistsByNameAsync(string name);
        Task AddIngredientCategoryAsync(IngredientCategoryAddFormModel model);
        Task<IngredientCategoryEditFormModel> GetIngredientCategoryForEditByIdAsync(int id);
        Task<int> GetIngredientCategoryIdByNameAsync(string name);
        Task EditIngredientCategoryAsync(IngredientCategoryEditFormModel model);
        Task DeleteIngredientCategoryById(int id);
    }
}
