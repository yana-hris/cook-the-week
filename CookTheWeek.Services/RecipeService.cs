using CookTheWeek.Data;
using CookTheWeek.Services.Interfaces;

namespace CookTheWeek.Services
{
    public class RecipeService : IRecipeService
    {
        private readonly CookTheWeekDbContext dbContext;
        public RecipeService(CookTheWeekDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
    }
}
