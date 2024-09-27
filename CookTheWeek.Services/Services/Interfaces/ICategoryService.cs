namespace CookTheWeek.Services.Data.Services.Interfaces
{
    using CookTheWeek.Data.Models.Interfaces;

    public interface ICategoryService<TCategory, TCategoryAddFormModel, TCategoryEditFormModel, TCategorySelectViewModel>
    where TCategory : class, ICategory, new()
    {
        /// <summary>
        /// Returns a select-menus usable collection of all categories
        /// </summary>
        /// <returns>A collection of TCategorySelectViewModel</returns>
        Task<ICollection<TCategorySelectViewModel>> GetAllCategoriesAsync();

        /// <summary>
        /// Adds a single category to the database
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task AddCategoryAsync(TCategoryAddFormModel model);

        /// <summary>
        /// Edits an existing category
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <exception cref="RecordNotFoundException"></exception>
        Task EditCategoryAsync(TCategoryEditFormModel model);

        /// <summary>
        /// Deleted a category by its id or throws and exception if not found
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="RecordNotFoundException"></exception>
        Task DeleteCategoryByIdAsync(int id);

        /// <summary>
        /// Retrieves a category by its Id and returns the correct model or throws an exception
        /// </summary>
        /// <param name="id"></param>
        /// <returns>The TCategoryEditFormModel</returns>
        /// <exception cref="RecordNotFoundException"></exception>
        Task<TCategoryEditFormModel> TryGetCategoryForEdit(int id);

        /// <summary>
        /// Checks if a category exists by id
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns>true or false</returns>
        Task<bool> CategoryExistsByIdAsync(int categoryId);

        /// <summary>
        /// Checks if a category exists by a given name
        /// </summary>
        /// <param name="name"></param>
        /// <returns>true or false</returns>
        Task<bool> CategoryExistsByNameAsync(string name);

        /// <summary>
        /// Gets the category id by its name
        /// </summary>
        /// <param name="name"></param>
        /// <returns>int or null</returns>
        Task<int?> GetCategoryIdByNameAsync(string name);

        /// <summary>
        /// Gets all category names as a list
        /// </summary>
        /// <returns>A collection of all category names</returns>
        Task<ICollection<string>> GetAllCategoryNamesAsync();

        /// <summary>
        /// Gets the total number of all categories
        /// </summary>
        /// <returns>int or 0</returns>
        Task<int?> GetAllCategoriesCountAsync();

        
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
