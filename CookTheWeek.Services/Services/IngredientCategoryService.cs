namespace CookTheWeek.Services.Data.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using CookTheWeek.Data.Models;
    using CookTheWeek.Services.Data.Services.Interfaces;
    using CookTheWeek.Web.ViewModels.Admin.IngredientAdmin;
    using CookTheWeek.Web.ViewModels.Category;

    public class IngredientCategoryService : ICategoryService<
        IngredientCategory,
        IngredientAddFormModel,
        IngredientEditFormModel,
        IngredientCategorySelectViewModel>
    {
        public Task AddCategoryAsync(IngredientAddFormModel model)
        {
            throw new NotImplementedException();
        }

        public Task<bool> CategoryExistsByIdAsync(int categoryId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> CategoryExistsByNameAsync(string name)
        {
            throw new NotImplementedException();
        }

        public Task DeleteCategoryByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task EditCategoryAsync(IngredientEditFormModel model)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<IngredientCategorySelectViewModel>> GetAllCategoriesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<int> GetAllCategoriesCountAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<string>> GetAllCategoryNamesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IngredientEditFormModel> GetCategoryForEditByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetCategoryIdByNameAsync(string name)
        {
            throw new NotImplementedException();
        }
    }
}
