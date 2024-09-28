namespace CookTheWeek.Services.Data.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
  
    using CookTheWeek.Data.Models;
    using Interfaces;
    using Web.ViewModels.RecipeIngredient;
    using CookTheWeek.Data.Repositories;
    using CookTheWeek.Common.Exceptions;

    using static CookTheWeek.Common.ExceptionMessagesConstants;

    public class RecipeIngredientService : IRecipeIngredientService
    {
        
        private readonly IRecipeIngredientRepository recipeIngredientRepository;

        private readonly IIngredientService ingredientService;

        public RecipeIngredientService(IRecipeIngredientRepository recipeIngredientRepository, 
            IIngredientService ingredientService)
        {
            this.recipeIngredientRepository = recipeIngredientRepository;
            this.ingredientService = ingredientService;
        }

        /// <inheritdoc/>    
        public async Task AddAsync(ICollection<RecipeIngredient> ingredients)
        {
            await recipeIngredientRepository.AddAllAsync(ingredients);
        }

        /// <inheritdoc/>    
        public async Task EditAsync(string id, ICollection<RecipeIngredient> ingredients)
        {
            await recipeIngredientRepository.UpdateAllByRecipeIdAsync(id, ingredients);
        }

        /// <inheritdoc/>    
        public async Task<RecipeIngredient> CreateRecipeIngredientForAddRecipeAsync(RecipeIngredientFormModel model)
        {
            bool exists = await ingredientService.ExistsByIdAsync(model.IngredientId.Value);

            if (!exists)
            {
                throw new RecordNotFoundException(RecordNotFoundExceptionMessages.IngredientNotFoundExceptionMessage, null);
            }

            return new RecipeIngredient
            {
                IngredientId = model.IngredientId.Value,
                Qty = model.Qty.GetDecimalQtyValue(),
                MeasureId = model.MeasureId.Value,
                SpecificationId = model.SpecificationId.Value
            };
        }
                

        /// <inheritdoc/>        
        public async Task<ICollection<RecipeIngredientSelectMeasureViewModel>> GetRecipeIngredientMeasuresAsync()
        {
            ICollection<Measure> allMeasures = await this.recipeIngredientRepository
                                                            .GetAllMeasuresQuery()
                                                            .ToListAsync();
            ICollection<RecipeIngredientSelectMeasureViewModel> model = 
                allMeasures
                    .Select(m => new RecipeIngredientSelectMeasureViewModel()
                    {
                        Id = m.Id,
                        Name = m.Name,

                    }).ToList();

            return model;
        }

        /// <inheritdoc/>     
        public async Task<ICollection<RecipeIngredientSelectSpecificationViewModel>> GetRecipeIngredientSpecificationsAsync()
        {
            ICollection<Specification> allSpecs = await recipeIngredientRepository
                .GetAllSpecsQuery()
                .ToListAsync();

            var model = allSpecs
                .Select(sp => new RecipeIngredientSelectSpecificationViewModel()
                {
                    Id = sp.Id,
                    Name = sp.Description
                }).ToList();

            return model;
        }


        /// <inheritdoc/>     
        public async Task<bool> IngredientMeasureExistsAsync(int measureId)
        {
            return await recipeIngredientRepository.MeasureExistsByIdAsync(measureId);
        }

        /// <inheritdoc/>     
        public async Task<bool> IngredientSpecificationExistsAsync(int specificationId)
        {
            return await recipeIngredientRepository.SpecificationExistsByIdAsync(specificationId);
        }

        /// <inheritdoc/>     
        public async Task DeleteByRecipeIdAsync(string id)
        {
            await recipeIngredientRepository.DeleteAllByRecipeIdAsync(id);
        }
        
    }
}
