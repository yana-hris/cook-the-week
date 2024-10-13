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
    using Microsoft.Extensions.Logging;

    public class RecipeIngredientService : IRecipeIngredientService
    {
        
        private readonly IRecipeIngredientRepository recipeIngredientRepository;
        private readonly ILogger<RecipeIngredientService> logger;
        private readonly IIngredientService ingredientService;

        public RecipeIngredientService(IRecipeIngredientRepository recipeIngredientRepository, 
            IIngredientService ingredientService,
            ILogger<RecipeIngredientService> logger)
        {
            this.recipeIngredientRepository = recipeIngredientRepository;
            this.ingredientService = ingredientService;
            this.logger = logger;
        }

        /// <inheritdoc/>    
        public async Task AddAsync(ICollection<RecipeIngredientFormModel> ingredients)
        {
            try
            {
                ICollection<RecipeIngredient> ingredientsToAdd = new HashSet<RecipeIngredient>();

                foreach (var newRecipeIngredient in ingredients)
                {
                    RecipeIngredient recipeIngredient = await CreateAsync(newRecipeIngredient);
                    ingredientsToAdd.Add(recipeIngredient);
                }

                recipeIngredientRepository.AddRangeWithoutSaveAsync(ingredientsToAdd);
                await recipeIngredientRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                logger.LogError($"Adding recipe ingredients failed. Error message: {ex.Message}. Error stacktrace: {ex.StackTrace}");
                throw;
            }
        }

        /// <inheritdoc/>    
        public async Task EditAsync(Guid id, ICollection<RecipeIngredientFormModel> newIngredients)
        {
            var existingIngredients = await GetAllByRecipeIdAsync(id);

           
            var ingredientsToAdd = new List<RecipeIngredient>();
            var ingredientsToDelete = new List<RecipeIngredient>();

            foreach (var newIngredient in newIngredients)
            {
                var (matchingIngredient, exists) = FindExistingRecipeIngredient(existingIngredients, newIngredient);

                if (matchingIngredient != null)
                {
                    // this will update the ingredients upon calling SaveChangesAsync();
                    matchingIngredient.Qty = newIngredient.Qty.GetDecimalQtyValue();
                }
                else
                {
                    // Add non-existing ingredients to the add list
                    RecipeIngredient missingIngredient = await CreateAsync(newIngredient);
                    ingredientsToAdd.Add(missingIngredient);
                }
            }

            foreach (var probablyRemovedIngredient in existingIngredients)
            {
                var matchingNewIngredient = newIngredients
                    .FirstOrDefault(n => n.IngredientId == probablyRemovedIngredient.IngredientId &&
                                    n.MeasureId == probablyRemovedIngredient.MeasureId &&
                                    n.SpecificationId == probablyRemovedIngredient.SpecificationId);

                if (matchingNewIngredient == null)
                {
                    // If no match found, mark this old ingredient for deletion
                    ingredientsToDelete.Add(probablyRemovedIngredient);
                }
            }

            try
            {
                if (ingredientsToDelete.Count > 0)
                {
                    recipeIngredientRepository.DeleteRangeWithoutSave(ingredientsToDelete);
                }

                if (ingredientsToAdd.Count > 0)
                {
                    recipeIngredientRepository.AddRangeWithoutSaveAsync(ingredientsToAdd);
                }

                await recipeIngredientRepository.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                logger.LogError($"Database update of recipe ingredients for recipe with id {id} failed." +
                    $"Error Message: {ex.Message}. Error Stacktrace: {ex.StackTrace}");
                throw;
            }
        }

        /// <inheritdoc/>    // TODO: Implement custom ingredient creation here!
        public async Task<RecipeIngredient> CreateAsync(RecipeIngredientFormModel model)
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
            }

            await recipeIngredientRepository.SaveChangesAsync();
        }

        /// <summary>
        /// Helper method to retrieve all recipe ingredients by a given recipe ID, tracked by the change tracker
        /// </summary>
        /// <param name="recipeId"></param>
        /// <returns>A collection of Recipe Ingredients</returns>
        private async Task<ICollection<RecipeIngredient>> GetAllByRecipeIdAsync(Guid recipeId)
        {
            return await this.recipeIngredientRepository
                .GetAllTrackedQuery()
                .Where(ri => ri.RecipeId == recipeId)
                .ToListAsync();
        }

        /// <summary>
        /// Searches for an existing recipe ingredient in the provided collection of  recipe ingredients.
        /// </summary>
        /// <param name="alreadyAdded">The collection of recipe ingredients that have already been added.</param>
        /// <param name="ingredient">The form model of the ingredient to search for, including its measure and specification details.</param>
        /// <returns>
        /// A tuple containing:
        /// - The found <see cref="RecipeIngredient"/> if the ingredient already exists, otherwise null.
        /// - A boolean value indicating whether the ingredient already exists in the collection.
        /// </returns>
        /// <remarks>
        /// The search checks for ingredients with the same <see cref="IngredientId"/>, <see cref="MeasureId"/>,
        /// and <see cref="SpecificationId"/> (if applicable). 
        /// </remarks>
        private static (RecipeIngredient? ingredient, bool exists) FindExistingRecipeIngredient(ICollection<RecipeIngredient> alreadyAdded,
                                                             RecipeIngredientFormModel ingredient)
        {
            // Check if the ingredient is already added with the same measure and specification
            var existingIngredient = alreadyAdded
                .FirstOrDefault(ri => ri.IngredientId == ingredient.IngredientId &&
                                      ri.MeasureId == ingredient.MeasureId &&
                                      (ri.SpecificationId == ingredient.SpecificationId ||
                                      (ri.SpecificationId == null && ingredient.SpecificationId == null)));

            // Update the quantity if found
            if (existingIngredient != null)
            {
                return (existingIngredient, true);
            }

            return (null, false);
        }
    }
}
