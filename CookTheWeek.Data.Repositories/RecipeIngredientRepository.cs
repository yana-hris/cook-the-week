﻿namespace CookTheWeek.Data.Repositories
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    
    using CookTheWeek.Data.Models;

    public class RecipeIngredientRepository : IRecipeIngredientRepository
    {
        private readonly CookTheWeekDbContext dbContext;
        public RecipeIngredientRepository(CookTheWeekDbContext dbContext)
        {
            this.dbContext = dbContext;   
        }

        /// <inheritdoc/>
        public IQueryable<RecipeIngredient> GetAllTrackedQuery()
        {
            return dbContext.RecipesIngredients
                .AsQueryable();
        }

        
       
        /// <inheritdoc/>
        public async Task SaveChangesAsync()
        {
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
    }
}
