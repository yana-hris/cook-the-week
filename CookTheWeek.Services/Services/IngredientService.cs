﻿namespace CookTheWeek.Services.Data.Services
{
    using System.Threading.Tasks;
    using System.Collections.Generic;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    using CookTheWeek.Common;
    using CookTheWeek.Common.Exceptions;
    using CookTheWeek.Data.Models;
    using CookTheWeek.Data.Repositories;
    using CookTheWeek.Services.Data.Services.Interfaces;
    using CookTheWeek.Services.Data.Models.RecipeIngredient;
    using CookTheWeek.Services.Data.Models.Validation;
    using CookTheWeek.Web.ViewModels.Admin.IngredientAdmin;
    using CookTheWeek.Web.ViewModels.Admin.IngredientAdmin.Enums;

    using static CookTheWeek.Common.ExceptionMessagesConstants;
    using static CookTheWeek.Services.Data.Helpers.EnumHelper;
    using static CookTheWeek.Common.GeneralApplicationConstants;

    public class IngredientService : IIngredientService
    {
        private readonly IIngredientRepository ingredientRepository;

        private readonly IValidationService validationService;

        private readonly ILogger<IngredientService> logger;

        public IngredientService(IIngredientRepository ingredientRepository,
            IValidationService validationService,
            ILogger<IngredientService> logger)
        {
            this.ingredientRepository = ingredientRepository;
            this.validationService = validationService;
            this.logger = logger;
        }

        /// <inheritdoc/>
        public async Task<AllIngredientsQueryModel> AllAsync(AllIngredientsQueryModel queryModel)
        {
            IQueryable<Ingredient> ingredientsQuery = 
                                ingredientRepository.GetAllQuery();

            if (!ingredientsQuery.Any())
            {
                logger.LogError("No ingredients found in the database");
                throw new RecordNotFoundException(RecordNotFoundExceptionMessages.NoIngredientsFoundExceptionMessage, null);
            }

            if (!string.IsNullOrWhiteSpace(queryModel.SearchString))
            {
                string wildCard = $"%{queryModel.SearchString.ToLower()}%";

                ingredientsQuery = ingredientsQuery
                    .Where(i => EF.Functions.Like(i.Name, wildCard));
            }

            if (queryModel.CategoryId.HasValue)
            {
                ingredientsQuery = ingredientsQuery
                    .Where(i => i.CategoryId == queryModel.CategoryId.Value);
            }

            IngredientSorting ingredientSorting = queryModel.IngredientSorting.HasValue &&
                Enum.IsDefined(typeof(IngredientSorting), queryModel.IngredientSorting) ?
                (IngredientSorting)queryModel.IngredientSorting :
                IngredientSorting.NameAscending;

            ingredientsQuery = ingredientSorting switch
            {
                IngredientSorting.NameDescending => ingredientsQuery
                    .OrderByDescending(i => i.Name),
                IngredientSorting.NameAscending => ingredientsQuery
                    .OrderBy(i => i.Name),
                IngredientSorting.IdDescending => ingredientsQuery
                    .OrderByDescending(i => i.Id),
                IngredientSorting.IdAscending => ingredientsQuery
                    .OrderBy(i => i.Id),
                _ => ingredientsQuery.OrderBy(i => i.Name)
            };

            if (queryModel.CurrentPage == default)
            {
                queryModel.CurrentPage = DefaultPage;
            }

            if (queryModel.IngredientsPerPage ==  default)
            {
                queryModel.IngredientsPerPage = AdminDefaultPerPage;
            }

            queryModel.TotalResults = ingredientsQuery.Count();            

            var ingredients = await ingredientsQuery
                .Skip((queryModel.CurrentPage - 1) * queryModel.IngredientsPerPage)
                .Take(queryModel.IngredientsPerPage)
                .Select(i => new IngredientAllViewModel()
                {
                    Id = i.Id,
                    Name = i.Name,
                    Category = i.Category.Name,
                })
                .ToListAsync();

            if (ingredients.Count == 0)
            {
                logger.LogError("No ingredients found by these criteria.");
                throw new RecordNotFoundException(RecordNotFoundExceptionMessages.NoIngredientsFoundExceptionMessage, null);
            }

            queryModel.Ingredients = ingredients;
            queryModel.IngredientSortings = GetEnumAsSelectViewModel<IngredientSorting>();

            return queryModel;
        }

        /// <inheritdoc/>
        public async Task<OperationResult> TryAddIngredientAsync(IngredientAddFormModel model)
        {
            var result = await validationService.ValidateIngredientFormModelAsync(model);

            if(!result.IsValid)
            {
                return OperationResult.Failure(result.Errors);
            }

            Ingredient ingredient = new Ingredient()
            {
                Name = model.Name,
                CategoryId = model.CategoryId
            };

            await ingredientRepository.AddAsync(ingredient);
            return OperationResult.Success();
        }

        /// <inheritdoc/>
        public async Task<OperationResult> TryEditIngredientAsync(IngredientEditFormModel model)
        {
            ValidationResult result = await validationService.ValidateIngredientFormModelAsync(model);

            if(!result.IsValid)
            {
                return OperationResult.Failure(result.Errors);
            }

            Ingredient ingredientToEdit = await ingredientRepository.GetByIdAsync(model.Id);

            ingredientToEdit.Name = model.Name;
            ingredientToEdit.CategoryId = model.CategoryId;

            await ingredientRepository.UpdateAsync(ingredientToEdit);
            return OperationResult.Success();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<RecipeIngredientSuggestionServiceModel>> GenerateIngredientSuggestionsAsync(string input)
        {
            IQueryable<Ingredient> suggestions = ingredientRepository
                .GetAllBySearchStringQuery(input);

            var model = await suggestions.
                Select(s => new RecipeIngredientSuggestionServiceModel()
                {
                    Id = s.Id,
                    Name = s.Name,  
                }).ToListAsync();

            return model;
        }

        /// <inheritdoc/>
        public async Task<IngredientEditFormModel> TryGetIngredientModelForEditAsync(int id)
        {
            try
            {
                Ingredient ingredient = await ingredientRepository.GetByIdAsync(id);

                IngredientEditFormModel model = new IngredientEditFormModel()
                {
                    Id = ingredient.Id,
                    Name = ingredient.Name,
                    CategoryId = ingredient.CategoryId
                };

                return model;
            }
            catch (RecordNotFoundException)
            {
                logger.LogError($"Record not found: {nameof(Ingredient)} with ID: {id} was not found.");
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<bool> ExistsByIdAsync(int id)
        {
            return await ingredientRepository.ExistsByIdAsync(id);
        }

        
        /// <inheritdoc/>
        public async Task<int?> AllCountAsync()
        {
            return await ingredientRepository.GetAllQuery()
                .CountAsync();
        }

        /// <inheritdoc/>
        public async Task TryDeleteByIdAsync(int id)
        {
            Ingredient ingredient = await GetByIdAsync(id);

            bool validationResult = await validationService.CanIngredientBeDeletedAsync(id);

            if (validationResult)
            {
                await ingredientRepository.DeleteAsync(ingredient);
            }
            else
            {
                logger.LogError($"Invalid operation while trying to delete an ingredient with id {id}. {InvalidOperationExceptionMessages.IngredientCannotBeDeletedExceptionMessage}");
                throw new InvalidOperationException(InvalidOperationExceptionMessages.IngredientCannotBeDeletedExceptionMessage);
            }            
        }

        /// <inheritdoc/>
        public async Task<Ingredient> GetByIdAsync(int ingredientId)
        {
            try
            {
                Ingredient ingredient = await ingredientRepository.GetByIdAsync(ingredientId);
                return ingredient;
            }
            catch (RecordNotFoundException)
            {
                logger.LogError($"Record not found: {nameof(Ingredient)} with ID {ingredientId} was not found.");
                throw;
            }

        }

        /// <inheritdoc/>
        public async Task<bool> HasAnyWithCategory(int id)
        {
            return await ingredientRepository.GetAllQuery()
                .AnyAsync(i => i.CategoryId == id);
        }

        
    }
}
