namespace CookTheWeek.Services
{
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using CookTheWeek.Data;
    using CookTheWeek.Services.Interfaces;
    using CookTheWeek.Web.ViewModels.Ingredient;
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

    }
}
