﻿namespace CookTheWeek.Data.Repositories
{
    using System.Linq;
    using System.Threading.Tasks;

    using CookTheWeek.Data.Models;
    using Microsoft.EntityFrameworkCore;

    public class MealRepository : IMealRepository
    {
        private readonly CookTheWeekDbContext dbContext;
        public MealRepository(CookTheWeekDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
       
        /// <inheritdoc/>
        public IQueryable<Meal> GetAllQuery()
        {
            return dbContext
                .Meals
                .AsNoTracking()
                .AsQueryable();
        }

        /// <inheritdoc/>
        public async Task<Meal?> GetByIdAsync(int id)
        {
            Meal? meal = await dbContext
                .Meals
                .Where(m => m.Id == id)
                    .Include(m => m.Recipe)
                        .ThenInclude(r => r.Category)
                    .Include(m => m.Recipe)
                        .ThenInclude(r => r.Steps)
                    .Include(m => m.Recipe)
                        .ThenInclude(r => r.RecipesIngredients)
                            .ThenInclude(ri => ri.Ingredient)
                .FirstOrDefaultAsync(m => m.Id == id);

            return meal;
        }

        /// <inheritdoc/>
        public async Task AddRangeAsync(ICollection<Meal> meals)
        {
            await dbContext.Meals.AddRangeAsync(meals);
            await dbContext.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task UpdateRangeAsync(ICollection<Meal> meals)
        {
            dbContext.Meals.UpdateRange(meals);
            await dbContext.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task RemoveRangeAsync(ICollection<Meal> meals)
        {
            dbContext.Meals.RemoveRange(meals);
            await dbContext.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task SaveChangesAsync()
        {
            await dbContext.SaveChangesAsync();
        }
    }
}
