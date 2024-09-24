namespace CookTheWeek.Data.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using CookTheWeek.Common.HelperMethods;
    using CookTheWeek.Data.Models;
    using Microsoft.EntityFrameworkCore;

    public class RecipeIngredientRepository : IRecipeIngredientRepository
    {
        private readonly CookTheWeekDbContext dbContext;
        public RecipeIngredientRepository(CookTheWeekDbContext dbContext)
        {
            this.dbContext = dbContext;   
        }
        /// <inheritdoc/>
        public async Task AddAllAsync(ICollection<RecipeIngredient> recipeIngredients)
        {
            await this.dbContext.RecipesIngredients.AddRangeAsync(recipeIngredients);
            await this.dbContext.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task UpdateAllByRecipeIdAsync(string recipeId, ICollection<RecipeIngredient> recipeIngredients)
        {
            var oldIngredients = await this.dbContext.RecipesIngredients
                .Where(ri => GuidHelper.CompareGuidStringWithGuid(recipeId, ri.RecipeId))
                .ToListAsync();

            this.dbContext.RecipesIngredients.RemoveRange(oldIngredients);
            await this.dbContext.RecipesIngredients.AddRangeAsync(recipeIngredients);
            await this.dbContext.SaveChangesAsync();
        }

        
        /// <inheritdoc/>
        public async Task DeleteAllByRecipeIdAsync(string recipeId)
        {
            var ingredientsToDelete = await this.dbContext
                .RecipesIngredients
                .Where(ri => GuidHelper.CompareGuidStringWithGuid(recipeId, ri.RecipeId))
                .ToListAsync();

            if (ingredientsToDelete.Any())
            {
                this.dbContext.RecipesIngredients.RemoveRange(ingredientsToDelete);
            }
            
            await this.dbContext.SaveChangesAsync();
        }

        
    }
}
