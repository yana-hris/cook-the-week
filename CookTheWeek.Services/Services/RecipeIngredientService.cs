namespace CookTheWeek.Services.Data.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
  
    using CookTheWeek.Common.Exceptions;
    using CookTheWeek.Data.Models;
    using CookTheWeek.Data.Repositories;
    using CookTheWeek.Services.Data.Services.Interfaces;
    using CookTheWeek.Web.ViewModels.RecipeIngredient;

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
            await recipeIngredientRepository.AddRangeAsync(ingredients);
        }

        /// <inheritdoc/>    
        public async Task EditAsync(Guid id, ICollection<RecipeIngredient> ingredients)
        {
            var oldRecipeIngredients = await GetAllByRecipeIdAsync(id);

            await recipeIngredientRepository.DeleteRangeAsync(oldRecipeIngredients);
            await recipeIngredientRepository.AddRangeAsync(ingredients);
        }

        /// <inheritdoc/>    // TODO: Implement custom ingredient creation here!
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
        public async Task SoftDeleteAllByRecipeIdAsync(Guid id)
        {
            var recipeIngredients = await GetAllByRecipeIdAsync(id);

            foreach (var ri in recipeIngredients)
            {
                ri.IsDeleted = true;
                await recipeIngredientRepository.UpdateAsync(ri);
            }
        }

        /// <inheritdoc/>     
        public async Task HardDeleteAllByRecipeIdAsync(Guid id)
        {
            var recipeIngredients = await GetAllByRecipeIdAsync(id);
            
            await recipeIngredientRepository.DeleteRangeAsync(recipeIngredients);
        }

        

        /// <summary>
        /// Helper method to retrieve all recipe ingredients by a given recipe ID.
        /// </summary>
        /// <param name="recipeId"></param>
        /// <returns>A collection of Recipe Ingredients</returns>
        private async Task<ICollection<RecipeIngredient>> GetAllByRecipeIdAsync(Guid recipeId)
        {
            return await this.recipeIngredientRepository
                .GetAllQuery()
                .Where(ri => ri.RecipeId == recipeId)
                .ToListAsync();
        }
        
    }
}
