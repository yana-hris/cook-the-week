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
        public IQueryable<RecipeIngredient> GetAllQuery()
        {
            return dbContext.RecipesIngredients
                .AsNoTracking()
                .AsQueryable();
        }

        /// <inheritdoc/>
        public async Task AddRangeAsync(ICollection<RecipeIngredient> recipeIngredients)
        {
            await this.dbContext.RecipesIngredients.AddRangeAsync(recipeIngredients);
            await this.dbContext.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task UpdateAsync(RecipeIngredient recipeIngredient)
        {
            dbContext.RecipesIngredients.Update(recipeIngredient);
            await dbContext.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task DeleteAsync(RecipeIngredient recipeIngredient)
        {
            dbContext.RecipesIngredients.Remove(recipeIngredient);
            await dbContext.SaveChangesAsync();
        }        
        
        /// <inheritdoc/>
        public async Task DeleteRangeAsync(ICollection<RecipeIngredient> recipeIngredients)
        {
            dbContext.RecipesIngredients.RemoveRange(recipeIngredients);
            await dbContext.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public IQueryable<Measure> GetAllMeasuresQuery()
        {
            return dbContext.Measures
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
            dbContext.Measures.Update(measure);
            await dbContext.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task DeleteMeasureAsync(Measure measure)
        {
            dbContext.Measures.Remove(measure);
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
            dbContext.Specifications.Update(spec);
            await dbContext.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task DeleteSpecAsync(Specification spec)
        {
            dbContext.Specifications.Remove(spec);
            await dbContext.SaveChangesAsync();
        }

        

        
    }
}
