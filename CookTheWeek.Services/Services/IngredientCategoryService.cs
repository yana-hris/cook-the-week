namespace CookTheWeek.Services.Data.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using CookTheWeek.Common.Exceptions;
    using CookTheWeek.Data.Models;
    using CookTheWeek.Data.Repositories;
    using CookTheWeek.Services.Data.Services.Interfaces;
    using CookTheWeek.Web.ViewModels.Admin.CategoryAdmin;
    using CookTheWeek.Web.ViewModels.Category;

    using static CookTheWeek.Common.ExceptionMessagesConstants;

    public class IngredientCategoryService : ICategoryService<
        IngredientCategory,
        IngredientCategoryAddFormModel,
        IngredientCategoryEditFormModel,
        IngredientCategorySelectViewModel>
    {
        private readonly ICategoryRepository<IngredientCategory> categoryRepository;
        public IngredientCategoryService(ICategoryRepository<IngredientCategory> categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }
        public async Task AddCategoryAsync(IngredientCategoryAddFormModel model)
        {
            IngredientCategory category = new IngredientCategory()
            {
                Name = model.Name,
            };

            await this.categoryRepository.AddAsync(category);
        }

        /// <inheritdoc/>      
        public async Task<bool> CategoryExistsByIdAsync(int categoryId)
        {
            return await this.categoryRepository.ExistsByIdAsync(categoryId);
        }

        /// <inheritdoc/>      
        public async Task<bool> CategoryExistsByNameAsync(string name)
        {
            return await this.categoryRepository.GetAllQuery()
                .AnyAsync(c => c.Name.ToLower() == name.ToLower());
        }

        /// <inheritdoc/>      
        public async Task DeleteCategoryByIdAsync(int id)
        {
            var categoryToDelete = await this.categoryRepository.GetByIdAsync(id);

            if (categoryToDelete == null)
            {
                throw new RecordNotFoundException(RecordNotFoundExceptionMessages.CategoryNotFoundExceptionMessage, null);
            }

            await this.categoryRepository.DeleteByIdAsync(id);
        }

        /// <inheritdoc/>      
        public async Task EditCategoryAsync(IngredientCategoryEditFormModel model)
        {
            var categoryToEdit = await this.categoryRepository.GetByIdAsync(model.Id);

            if (categoryToEdit == null)
            {
                throw new RecordNotFoundException(RecordNotFoundExceptionMessages.CategoryNotFoundExceptionMessage, null);
            }
            categoryToEdit.Name = model.Name;

            await this.categoryRepository.EditAsync(categoryToEdit);
        }

        /// <inheritdoc/>      
        public async Task<ICollection<IngredientCategorySelectViewModel>> GetAllCategoriesAsync()
        {
            ICollection<IngredientCategorySelectViewModel> all = await this.categoryRepository.GetAllQuery()
                .Select(c => new IngredientCategorySelectViewModel()
                {
                    Id = c.Id,
                    Name = c.Name,
                })
                .ToListAsync();

            return all;
        }

        /// <inheritdoc/>      
        public async Task<int?> GetAllCategoriesCountAsync()
        {
            return await this.categoryRepository.GetAllQuery().CountAsync();
        }

        /// <inheritdoc/>      
        public async Task<ICollection<string>> GetAllCategoryNamesAsync()
        {
            ICollection<string> names = await this.categoryRepository.GetAllQuery()
                .Select(c => c.Name)
                .ToListAsync();

            return names;
        }

        /// <inheritdoc/>      
        public async Task<IngredientCategoryEditFormModel> GetCategoryForEditByIdAsync(int id)
        {
            var categoryToEdit = await this.categoryRepository
                .GetByIdAsync(id);

            if (categoryToEdit == null)
            {
                throw new RecordNotFoundException(RecordNotFoundExceptionMessages.CategoryNotFoundExceptionMessage, null);
            }

            IngredientCategoryEditFormModel model = new IngredientCategoryEditFormModel()
            {
                Id = categoryToEdit.Id,
                Name = categoryToEdit.Name,
            };

            return model;
        }

        /// <inheritdoc/>      
        public async Task<int?> GetCategoryIdByNameAsync(string name)
        {
            return await this.categoryRepository.GetAllQuery()
                .Where(c => c.Name.ToLower() == name.ToLower())
                .Select(c => c.Id)
                .FirstOrDefaultAsync();
        }
    }
}
