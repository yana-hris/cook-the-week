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
    using CookTheWeek.Web.ViewModels;
    using CookTheWeek.Web.ViewModels.Admin.CategoryAdmin;

    using static CookTheWeek.Common.ExceptionMessagesConstants;

    public class RecipeCategoryService : ICategoryService<
        RecipeCategory,
        RecipeCategoryAddFormModel,
        RecipeCategoryEditFormModel,
        SelectViewModel>
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

            this.recipeService = recipeService;
            this.validationService = validationService;

            this.logger = logger;
        }
        
        /// <inheritdoc/>       
        public async Task<OperationResult> TryAddCategoryAsync(RecipeCategoryAddFormModel model)
        {
           
            ValidationResult result = await validationService.ValidateCategoryAsync(
                                               model,
                                               categoryRepository);

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
        public async Task<ICollection<SelectViewModel>> GetAllCategoriesAsync()
        {
            try
            {
                ICollection<SelectViewModel> all = await this.categoryRepository
                .GetAllQuery()
                .Select(c => new SelectViewModel()
                {
                    Id = c.Id,
                    Name = c.Name,
                })
                .ToListAsync();

                return all;
            }
            catch (Exception ex)
            {
                logger.LogError($"Recipe Categories Select View Model loading failed. Error message: {ex.Message}. Error Stacktrace: {ex.StackTrace}");
                throw;
            }
        }

        /// <inheritdoc/>      
        public async Task<int?> GetAllCategoriesCountAsync()
        {
            return await this.categoryRepository.GetAllQuery().CountAsync();
        }

        /// <inheritdoc/>      
        public async Task<ICollection<string>> GetAllCategoryNamesAsync()
        {
            try
            {
                ICollection<string> names = await this.categoryRepository
                    .GetAllQuery()
                    .Select(c => c.Name)
                    .ToListAsync();

                return names;
            }
            catch (Exception ex)
            {
                logger.LogError($"Recipe Categories Names loading failed. Error message: {ex.Message}. Error Stacktrace: {ex.StackTrace}");
                throw;
            }
        }

       
        /// <inheritdoc/>      
        public async Task TryDeleteCategoryAsync(int id)
        {            
            await validationService
                .CanCategoryBeDeletedAsync<RecipeCategory, Recipe>
                (id,
                 categoryRepository);

            await categoryRepository.DeleteByIdAsync(id);
        }

        /// <inheritdoc/>      
        public async Task<OperationResult> TryEditCategoryAsync(RecipeCategoryEditFormModel model)
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
        public async Task<RecipeCategoryEditFormModel> TryGetCategoryModelForEditAsync(int id)
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
