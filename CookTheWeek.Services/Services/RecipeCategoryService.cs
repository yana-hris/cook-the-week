﻿namespace CookTheWeek.Services.Data.Services
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

    public class RecipeCategoryService : ICategoryService<
        RecipeCategory,
        RecipeCategoryAddFormModel,
        RecipeCategoryEditFormModel,
        RecipeCategorySelectViewModel>
    {
        private readonly ICategoryRepository<RecipeCategory> categoryRepository;
        private readonly IRecipeService recipeService;
        private readonly IValidationService validationService;
        private readonly ILogger<RecipeCategoryService> logger;

        public RecipeCategoryService(ICategoryRepository<RecipeCategory> categoryRepository,
            IRecipeService recipeService,
            IValidationService validationService,
            ILogger<RecipeCategoryService> logger)
        {
            this.categoryRepository = categoryRepository;
            this.validationService = validationService;
            this.recipeService = recipeService;
            this.logger = logger;
        }
        
        /// <inheritdoc/>       
        public async Task<OperationResult> TryAddCategoryAsync(RecipeCategoryAddFormModel model)
        {
           
            ValidationResult result = await validationService.ValidateCategoryAsync(
                                               model,
                                               name => GetCategoryIdByNameAsync(name));

            if (!result.IsValid)
            {
                return OperationResult.Failure(result.Errors);
            }

            RecipeCategory category = new RecipeCategory()
            {
                Name = model.Name,
            };

            await categoryRepository.AddAsync(category);
            return OperationResult.Success();
        }

        /// <inheritdoc/>      
        public async Task<ICollection<RecipeCategorySelectViewModel>> GetAllCategoriesAsync()
        {
            ICollection<RecipeCategorySelectViewModel> all = await this.categoryRepository.GetAllQuery()
                .Select(c => new RecipeCategorySelectViewModel()
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
            bool canBeDeleted = await validationService.CanCategoryBeDeletedAsync(id,
                        async (id) => await categoryRepository.GetByIdAsync(id),
                        async (id) => await recipeService.HasAnyWithCategory(id));

           
            await categoryRepository.DeleteByIdAsync(id);
        }

        /// <inheritdoc/>      
        public async Task<OperationResult> TryEditCategoryAsync(RecipeCategoryEditFormModel model)
        {
            ValidationResult result = await validationService.ValidateCategoryAsync(
                                               model,
                                               name => GetCategoryIdByNameAsync(name),
                                               id => CategoryExistsByIdAsync(id));

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
        public async Task<int?> GetCategoryIdByNameAsync(string name)
        {
            return await this.categoryRepository.GetAllQuery()
                .Where(c => c.Name.ToLower() == name.ToLower())
                .Select(c => c.Id)
                .FirstOrDefaultAsync();
        }

        /// <inheritdoc/>  
        public async Task<RecipeCategoryEditFormModel> TryGetCategoryModelForEdit(int id)
        {
            var categoryToEdit = await this.categoryRepository
                .GetByIdAsync(id);

            if (categoryToEdit == null)
            {
                logger.LogError($"Record not found: {nameof(RecipeCategory)} with ID: {id} was not found.");
                throw new RecordNotFoundException(RecordNotFoundExceptionMessages.CategoryNotFoundExceptionMessage, null);
            }

            RecipeCategoryEditFormModel model = new RecipeCategoryEditFormModel()
            {
                Id = categoryToEdit!.Id,
                Name = categoryToEdit.Name,
            };

            return model;
        }
        
    }
}
