namespace CookTheWeek.Data.Repositories
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using CookTheWeek.Common.HelperMethods;
    using CookTheWeek.Data.Models;

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

        /// <inheritdoc/>
        public IQueryable<Measure> GetAllMeasuresQuery()
        {
            return dbContext
                .Measures
                .AsNoTracking()
                .AsQueryable();
        }

        /// <inheritdoc/>
        public async Task<bool> MeasureExistsByIdAsync(int id)
        {
            return await dbContext.Measures.AnyAsync(m => m.Id == id);
        }

        /// <inheritdoc/>
        public async Task<bool> MeasureExistsByNameAsync(string name)
        {
            return await dbContext.Measures
                .AnyAsync(m => m.Name.ToLower() == name.ToLower());
        }

        /// <inheritdoc/>
        public async Task AddMeasureAsync(Measure measure)
        {
            await dbContext.Measures.AddAsync(measure);
            await dbContext.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task UpdateMeasureAsync(Measure measure)
        {
            dbContext.Update(measure);
            await dbContext.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task DeleteMeasureAsync(Measure measure)
        {
            dbContext.Remove(measure);
            await dbContext.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public IQueryable<Specification> GetAllSpecsQuery()
        {
            return dbContext
                .Specifications
                .AsNoTracking()
                .AsQueryable();
        }

        /// <inheritdoc/>
        public async Task<bool> SpecificationExistsByIdAsync(int id)
        {
            return await dbContext.Specifications.AnyAsync(sp => sp.Id == id);
        }

        /// <inheritdoc/>
        public async Task<bool> SpecificationExistsByNameAsync(string name)
        {
            return await dbContext.Specifications
                .AnyAsync(sp => sp.Description.ToLower() == name.ToLower());
        }

        /// <inheritdoc/>
        public async Task AddSpecAsync(Specification spec)
        {
            await dbContext.Specifications.AddAsync(spec);
            await dbContext.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task UpdateSpecAsync(Specification spec)
        {
            dbContext.Update(spec);
            await dbContext.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task DeleteSpecAsync(Specification spec)
        {
            dbContext.Remove(spec);
            await dbContext.SaveChangesAsync();
        }
        
    }
}
