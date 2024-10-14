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
    using System.Runtime.InteropServices;
    using System.Data.SqlTypes;

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
        public HashSet<RecipeIngredient> CreateAll(ICollection<RecipeIngredientFormModel> ingredients)
        {
           
            HashSet<RecipeIngredient> ingredientsToAdd = new HashSet<RecipeIngredient>();

            foreach (var modelRecipeIngredient in ingredients)
            {
                var alreadyAddedIngredient = FindExistingRecipeIngredient(ingredientsToAdd, modelRecipeIngredient);

                if (alreadyAddedIngredient != null)
                {
                    alreadyAddedIngredient.Qty += modelRecipeIngredient.Qty.GetDecimalQtyValue();
                }
                else
                {
                    RecipeIngredient newIngredient = Create(null, modelRecipeIngredient);
                    ingredientsToAdd.Add(newIngredient);
                }
            }

            return ingredientsToAdd;
        }

        /// <inheritdoc/>  
        public async Task<HashSet<RecipeIngredient>> UpdateAll(Guid id, ICollection<RecipeIngredientFormModel> ingredientsModelCollection)
        {
            var oldIngredients = await GetAllByRecipeIdAsync(id);

            HashSet<RecipeIngredient> updatedIngredients = new HashSet<RecipeIngredient>();

            foreach (var modelIngredient in ingredientsModelCollection)
            {
                // first check if the model ingredient is not a doubled line of the same ingredient. If yes => update the Qty
                var alreadyAddedIngredient = FindExistingRecipeIngredient(updatedIngredients, modelIngredient);

                if (alreadyAddedIngredient != null)
                {
                    alreadyAddedIngredient.Qty += modelIngredient.Qty.GetDecimalQtyValue();
                }
                else
                {
                    var matchingIngredient = FindExistingRecipeIngredient(oldIngredients, modelIngredient);

                    if (matchingIngredient != null)
                    {
                        // this will update the ingredients upon calling SaveChangesAsync();
                        matchingIngredient.Qty = modelIngredient.Qty.GetDecimalQtyValue();
                        updatedIngredients.Add(matchingIngredient);
                    }
                    else
                    {
                        // Add non-existing ingredients to the add list
                        RecipeIngredient missingIngredient = Create(id, modelIngredient);
                        updatedIngredients.Add(missingIngredient);
                    }
                }
            }

            return updatedIngredients;
        }
                
        /// <inheritdoc/>        
        public async Task<ICollection<RecipeIngredientSelectMeasureViewModel>> GetRecipeIngredientMeasuresAsync()
        {
            try
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
            catch (Exception ex)
            {
                logger.LogError($"Loading recipe ingredient measures faild. Error message: {ex.Message}. Error Stacktrace: {ex.StackTrace}");
                throw;
            }
        }

        /// <inheritdoc/>     
        public async Task<ICollection<RecipeIngredientSelectSpecificationViewModel>> GetRecipeIngredientSpecificationsAsync()
        {
            try
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
            catch (Exception ex)
            {
                logger.LogError($"Loading recipe ingredient specifications faild. Error message: {ex.Message}. Error Stacktrace: {ex.StackTrace}");
                throw;
            }
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
        /// The found <see cref="RecipeIngredient"/> if the ingredient already exists, otherwise null.
        /// </returns>
        /// <remarks>
        /// The search checks for ingredients with the same <see cref="IngredientId"/>, <see cref="MeasureId"/>,
        /// and <see cref="SpecificationId"/> (if applicable). 
        /// </remarks>
        private static RecipeIngredient? FindExistingRecipeIngredient(ICollection<RecipeIngredient> alreadyAdded,
                                                             RecipeIngredientFormModel ingredient)
        {
            // Check if the ingredient is already added with the same measure and specification
            return alreadyAdded
                .FirstOrDefault(ri => ri.IngredientId == ingredient.IngredientId &&
                                      ri.MeasureId == ingredient.MeasureId &&
                                      (ri.SpecificationId == ingredient.SpecificationId ||
                                      (ri.SpecificationId == null && ingredient.SpecificationId == null)));

            
        }

        /// <summary>
        /// A helper method that creates a new RecipeIngredient both for new and existing recipes. 
        /// </summary>
        /// <param name="recipeId">Must be passed to create recipe ingredients for existing recipes (Edit scenario)</param>
        /// <param name="model"></param>
        /// <returns>RecipeIngredient</returns>
        private RecipeIngredient Create(Guid? recipeId, RecipeIngredientFormModel model)
        {
            // Check for nullable values and handle them safely
            RecipeIngredient newRecipeIngredient = new RecipeIngredient
            {
                IngredientId = model.IngredientId.HasValue ? model.IngredientId.Value : throw new ArgumentNullException(nameof(model.IngredientId)),
                Qty = model.Qty.GetDecimalQtyValue(),
                MeasureId = model.MeasureId.HasValue ? model.MeasureId.Value : throw new ArgumentNullException(nameof(model.MeasureId)),
                SpecificationId = model.SpecificationId.HasValue ? model.SpecificationId.Value : (int?)null // SpecificationId is nullable
            };

            // Check for null and assign RecipeId only if it's provided (after the Recipe is saved)
            if (recipeId.HasValue && recipeId.Value != Guid.Empty)
            {
                newRecipeIngredient.RecipeId = recipeId.Value;
            }

            return newRecipeIngredient;
        }
               
       
    }
}
