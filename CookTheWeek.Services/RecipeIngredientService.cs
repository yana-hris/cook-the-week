namespace CookTheWeek.Services
{
    
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using CookTheWeek.Data;
    using CookTheWeek.Services.Interfaces;
    using CookTheWeek.Web.ViewModels.RecipeIngredient;
    using CookTheWeek.Data.Models;

    public class RecipeIngredientService : IRecipeIngredientService
    {
        private readonly CookTheWeekDbContext dbContext;

        public RecipeIngredientService(CookTheWeekDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<ICollection<RecipeIngredientSelectMeasureViewModel>> GetRecipeIngredientMeasuresAsync()
        {
            ICollection<RecipeIngredientSelectMeasureViewModel> allMeasures = await this.dbContext
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
            ICollection<RecipeIngredientSelectSpecificationViewModel> allSpecs = await this.dbContext
                .Specifications
                .AsNoTracking()
                .Select(sp => new RecipeIngredientSelectSpecificationViewModel()
                {
                    Id = sp.Id,
                    Description = sp.Description
                }).ToListAsync();

            return allSpecs;
        }

        public async Task<int> AddAsync(RecipeIngredientFormViewModel model, string recipeId)
        {
            RecipeIngredient recipeIngredient = new RecipeIngredient()
            {
                RecipeId = Guid.Parse(recipeId),
                Qty = model.Qty,
                MeasureId = model.MeasureId,
                SpecificationId = model.SpecificationId
            };

            await this.dbContext.RecipesIngredients
                .AddAsync(recipeIngredient);
            await this.dbContext.SaveChangesAsync();

            return recipeIngredient.IngredientId;
        }

        public async Task<bool> IngredientIsAlreadyAddedAsync(string ingredientName, string recipeId)
        {
            return await this.dbContext.RecipesIngredients
                .AsNoTracking()
                .AnyAsync(ri => ri.RecipeId.ToString() == recipeId && ri.Ingredient.Name == ingredientName);
        }

        public async Task<bool> IngredientMeasureExistsAsync(int measureId)
        {
            return await this.dbContext.Measures
                .AsNoTracking()
                .AnyAsync(m => m.Id == measureId);
        }

        public async Task<bool> IngredientSpecificationExistsAsync(int specificationId)
        {
            return await this.dbContext.Specifications
                .AsNoTracking()
                .AnyAsync(sp => sp.Id == specificationId);
        }

        public async Task RemoveAsync(int ingredientId, string recipeId)
        {
            RecipeIngredient? recipeIngredient = await this.dbContext.RecipesIngredients
                .FirstOrDefaultAsync(ri => ri.IngredientId == ingredientId && ri.RecipeId.ToString() == recipeId);

            if(recipeIngredient == null)
            {
                throw new InvalidOperationException("No such ingredient exists for this recipe!");
            }

            this.dbContext.RecipesIngredients.Remove(recipeIngredient);
            await this.dbContext.SaveChangesAsync();
        }
    }
}
