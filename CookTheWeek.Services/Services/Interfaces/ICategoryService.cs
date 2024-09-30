namespace CookTheWeek.Services.Data.Services.Interfaces
{
    using CookTheWeek.Common;
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
        Task<OperationResult> TryAddCategoryAsync(TCategoryAddFormModel model);

        /// <summary>
        /// Validates a category and if valid, persists changes in the database.
        /// If not, returns a Failure OperationResult with Errors for Modelstate
        /// </summary>
        /// <param name="model"></param>
        /// <returns>The result of the Operation (Success or Failure + Errors)</returns>
        /// <remarks>May throw RecordNotFoundException due to usage of GetById method in validation</remarks>
        Task<OperationResult> TryEditCategoryAsync(TCategoryEditFormModel model);

        /// <summary>
        /// Tries to retrieve and validates a category, throwing exceptions on the way. If the category has any dependencies, will throw an exception.
        /// If all goes well, deletes the category.
        /// </summary>
        /// <param name="id">the category id</param>
        /// <returns>Task</returns>
        /// <remarks>May throw RecordNotFoundException or InvalidOperationException</remarks>
        Task TryDeleteCategoryAsync(int id);

        /// <summary>
        /// Retrieves a category by its Id and returns the correct model or throws an exception
        /// </summary>
        /// <param name="id"></param>
        /// <returns>The TCategoryEditFormModel</returns>
        /// <exception cref="RecordNotFoundException"></exception>
        Task<TCategoryEditFormModel> TryGetCategoryModelForEdit(int id);

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
}
