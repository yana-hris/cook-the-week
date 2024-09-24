namespace CookTheWeek.Services.Data.Services.Interfaces
{
    using CookTheWeek.Data.Models.Interfaces;

    public interface ICategoryService<TCategory, TAddFormModel, TEditFormModel, TSelectViewModel>
    where TCategory : class, ICategory
    {
        Task<ICollection<TSelectViewModel>> GetAllCategoriesAsync();
        Task AddCategoryAsync(TAddFormModel model);
        Task EditCategoryAsync(TEditFormModel model);
        Task DeleteCategoryByIdAsync(int id);
        Task<TEditFormModel> GetCategoryForEditByIdAsync(int id);
        Task<bool> CategoryExistsByIdAsync(int categoryId);
        Task<bool> CategoryExistsByNameAsync(string name);
        Task<int> GetCategoryIdByNameAsync(string name);
        Task<ICollection<string>> GetAllCategoryNamesAsync();
        Task<int> GetAllCategoriesCountAsync();
    }

    // OLD CODE - DELETE when new code starts to work
    //// Recipe Category
    //    Task<ICollection<RecipeCategorySelectViewModel>> AllRecipeCategoriesAsync();
    //    Task AddRecipeCategoryAsync(RecipeCategoryAddFormModel model);
    //    Task EditRecipeCategoryAsync(RecipeCategoryEditFormModel model);
    //    Task DeleteRecipeCategoryById(int id);
    //    Task<RecipeCategoryEditFormModel> GetRecipeCategoryForEditByIdAsync(int id);
    //    Task<bool> RecipeCategoryExistsByIdAsync(int recipeCategoryId);
    //    Task<bool> RecipeCategoryExistsByNameAsync(string name);
    //    Task<int> GetRecipeCategoryIdByNameAsync(string name);
    //    Task<ICollection<string>> AllRecipeCategoryNamesAsync();
    //    Task<int> AllRecipeCategoriesCountAsync();

    //    // Ingredient Category
    //    Task<ICollection<IngredientCategorySelectViewModel>> AllIngredientCategoriesAsync();

    //    //Add
    //    //Edit
    //    //Delete
    //    Task<bool> IngredientCategoryExistsByIdAsync(int ingredientCategoryId);
    //    Task<ICollection<string>> AllIngredientCategoryNamesAsync();
    //    Task<int> AllIngredientCategoriesCountAsync();
    //    Task<bool> IngredientCategoryExistsByNameAsync(string name);
    //    Task AddIngredientCategoryAsync(IngredientCategoryAddFormModel model);
    //    Task<IngredientCategoryEditFormModel> GetIngredientCategoryForEditByIdAsync(int id);
    //    Task<int> GetIngredientCategoryIdByNameAsync(string name);
    //    Task EditIngredientCategoryAsync(IngredientCategoryEditFormModel model);
    //    Task DeleteIngredientCategoryById(int id);
    //}
}
