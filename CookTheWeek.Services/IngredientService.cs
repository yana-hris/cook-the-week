namespace CookTheWeek.Services
{
    using System.Threading.Tasks;
    using System.Collections.Generic;

    using Microsoft.EntityFrameworkCore;

    using CookTheWeek.Data;
    using Interfaces;
    using Web.ViewModels.Ingredient;
    using Data.Models.RecipeIngredient;
    using CookTheWeek.Data.Models.IgredientEntities;

    public class IngredientService : IIngredientService
    {
        private readonly CookTheWeekDbContext dbContext;

        public IngredientService(CookTheWeekDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
       
        public async Task<bool> existsByNameAsync(string name)
        {
            bool exists = await this.dbContext
                .Ingredients
                .AsNoTracking()
                .AnyAsync(i => i.Name.ToLower() == name.ToLower());

            return exists;
        }
        public async Task<int> AddIngredientAsync(IngredientFormViewModel model)
        {
            Ingredient ingredient = new Ingredient()
            {
                Name = model.Name,
                IngredientCategoryId = model.IngredientCategoryId
            };

            await this.dbContext.Ingredients.AddAsync(ingredient);
            await this.dbContext.SaveChangesAsync();

            return ingredient.Id;
        }

        public async Task<IEnumerable<RecipeIngredientServiceModel>> GetIngredientSuggestionsAsync(string input)
        {
            string wildCard = $"%{input.ToLower()}%";

            return  await this.dbContext
                .Ingredients
                .AsNoTracking()
                .Where(i => EF.Functions.Like(i.Name.ToLower(), wildCard))
                .Select(i => new RecipeIngredientServiceModel()
                {
                    Id = i.Id,
                    Name = i.Name,
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<RecipeIngredientServiceModel>> GetAllIngredientsAsync()
        {
            return await this.dbContext
                .Ingredients
                .AsNoTracking()
                .Select(i => new RecipeIngredientServiceModel()
                {
                    Id = i.Id,
                    Name = i.Name,
                }).ToListAsync();
        }
    }
}
