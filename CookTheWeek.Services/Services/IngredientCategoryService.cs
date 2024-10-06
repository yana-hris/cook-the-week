namespace CookTheWeek.Services.Data.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    using CookTheWeek.Common;
    using CookTheWeek.Common.Exceptions;
    using CookTheWeek.Data.Models;
    using CookTheWeek.Data.Repositories;
    using CookTheWeek.Services.Data.Models.Validation;
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

        private readonly IValidationService validationService;
        private readonly IIngredientService ingredientService;

        private readonly ILogger<IngredientCategoryService> logger;
        public IngredientCategoryService(
            ICategoryRepository<IngredientCategory> categoryRepository,
            IIngredientService ingredientService,
            ILogger<IngredientCategoryService> logger,
            IValidationService validationService)
        {
            this.categoryRepository = categoryRepository;
            this.validationService = validationService;
            this.ingredientService = ingredientService;
            this.logger = logger;
        }
        public async Task<OperationResult> TryAddCategoryAsync(IngredientCategoryAddFormModel model)
        {
            ValidationResult result = await validationService.ValidateCategoryAsync(
                                                model,
                                                categoryRepository);

            if (!result.IsValid)
            {
                return OperationResult.Failure(result.Errors);
            }

            IngredientCategory category = new IngredientCategory()
            {
                Name = model.Name,
            };

            await this.categoryRepository.AddAsync(category);
            return OperationResult.Success();
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
        public async Task TryDeleteCategoryAsync(int id)
        {
            await validationService.CanCategoryBeDeletedAsync<IngredientCategory, Ingredient>(id,
                categoryRepository);

            await categoryRepository.DeleteByIdAsync(id);
        }

        /// <inheritdoc/>      
        public async Task<OperationResult> TryEditCategoryAsync(IngredientCategoryEditFormModel model)
        {
            ValidationResult result = await validationService.ValidateCategoryAsync(
                                               model,
                                               categoryRepository);

            if (!result.IsValid)
            {
                return OperationResult.Failure(result.Errors);
            }

            var categoryToEdit = await this.categoryRepository.GetByIdAsync(model.Id);
            categoryToEdit.Name = model.Name;

            await this.categoryRepository.UpdateAsync(categoryToEdit);
            return OperationResult.Success();
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
        public async Task<IngredientCategoryEditFormModel> TryGetCategoryModelForEditAsync(int id)
        {
            var categoryToEdit = await this.categoryRepository
                .GetByIdAsync(id);

            if (categoryToEdit == null)
            {
                logger.LogError($"Record not found: {nameof(IngredientCategory)} with ID: {id} was not found.");
                throw new RecordNotFoundException(RecordNotFoundExceptionMessages.CategoryNotFoundExceptionMessage, null);
            }

            IngredientCategoryEditFormModel model = new IngredientCategoryEditFormModel()
            {
                Id = categoryToEdit!.Id,
                Name = categoryToEdit.Name,
            };

            return model;
        }
    }
}
