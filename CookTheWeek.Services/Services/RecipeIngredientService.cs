namespace CookTheWeek.Services.Data.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using CookTheWeek.Common.HelperMethods;
    using CookTheWeek.Data;
    using CookTheWeek.Data.Models;
    using Interfaces;
    using Web.ViewModels.RecipeIngredient;
    using CookTheWeek.Data.Repositories;

    public class RecipeIngredientService : IRecipeIngredientService
    {
        private readonly CookTheWeekDbContext dbContext;
        private readonly IRecipeIngredientRepository recipeIngredientRepository;

        public RecipeIngredientService(CookTheWeekDbContext dbContext,
            IRecipeIngredientRepository recipeIngredientRepository)
        {
            this.dbContext = dbContext;
            this.recipeIngredientRepository = recipeIngredientRepository;
        }

        public async Task<ICollection<RecipeIngredientSelectMeasureViewModel>> GetRecipeIngredientMeasuresAsync()
        {
            ICollection<RecipeIngredientSelectMeasureViewModel> allMeasures = await this.recipeIngredientRepository.GetAllMeasuresQuery();
                
                dbContext
                .Measures
                .AsNoTracking()
                .Select(m => new RecipeIngredientSelectMeasureViewModel()
                {
                    Id = m.Id,
                    Name = m.Name,

                }).ToListAsync();

            return allMeasures;
        }

        public async Task<ICollection<RecipeIngredientSelectSpecificationViewModel>> GetRecipeIngredientSpecificationsAsync()
        {
            ICollection<RecipeIngredientSelectSpecificationViewModel> allSpecs = await dbContext
                .Specifications
                .AsNoTracking()
                .Select(sp => new RecipeIngredientSelectSpecificationViewModel()
                {
                    Id = sp.Id,
                    Name = sp.Description
                }).ToListAsync();

            return allSpecs;
        }

        public async Task<int> AddAsync(RecipeIngredientFormModel model, string recipeId)
        {
            int ingredientId = await dbContext.Ingredients
                .AsNoTracking()
                .Where(i => GuidHelper.CompareTwoGuidStrings(i.Name, model.Name))
                .Select(i => i.Id)
                .FirstOrDefaultAsync();


            RecipeIngredient recipeIngredient = new RecipeIngredient()
            {
                IngredientId = ingredientId,
                RecipeId = Guid.Parse(recipeId),
                MeasureId = model.MeasureId!.Value,
                Qty = model.Qty.GetDecimalQtyValue(),
                SpecificationId = model.SpecificationId!.Value
            };

            await dbContext.RecipesIngredients
                .AddAsync(recipeIngredient);
            await dbContext.SaveChangesAsync();

            return recipeIngredient.IngredientId;
        }

        public async Task<bool> IsAlreadyAddedAsync(string ingredientName, string recipeId)
        {
            return await dbContext.RecipesIngredients
                .AsNoTracking()
                .AnyAsync(ri => ri.RecipeId.ToString() == recipeId && ri.Ingredient.Name == ingredientName);
        }

        public async Task<bool> IngredientMeasureExistsAsync(int measureId)
        {
            return await dbContext.Measures
                .AsNoTracking()
                .AnyAsync(m => m.Id == measureId);
        }

        public async Task<bool> IngredientSpecificationExistsAsync(int specificationId)
        {
            return await dbContext.Specifications
                .AsNoTracking()
                .AnyAsync(sp => sp.Id == specificationId);
        }

        public async Task RemoveAsync(int ingredientId, string recipeId)
        {
            RecipeIngredient? recipeIngredient = await dbContext.RecipesIngredients
                .FirstOrDefaultAsync(ri => ri.IngredientId == ingredientId && ri.RecipeId.ToString() == recipeId);

            if (recipeIngredient == null)
            {
                throw new InvalidOperationException("No such ingredient exists for this recipe!");
            }

            dbContext.RecipesIngredients.Remove(recipeIngredient);
            await dbContext.SaveChangesAsync();
        }
    }
}
