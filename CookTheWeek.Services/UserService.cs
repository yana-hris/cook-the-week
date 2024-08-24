namespace CookTheWeek.Services.Data
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    using CookTheWeek.Data;
    using CookTheWeek.Data.Models;
    using Interfaces;
    using static CookTheWeek.Common.GeneralApplicationConstants;

    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly CookTheWeekDbContext dbContext;
        private readonly IRecipeService recipeService;
        private readonly IMealPlanService mealPlanService;
        
        public UserService(UserManager<ApplicationUser> userManager, 
            CookTheWeekDbContext dbContext, 
            IRecipeService recipeService,
            IMealPlanService mealPlanService)
        {
            this.userManager = userManager;
            this.dbContext = dbContext;
            this.recipeService = recipeService;
            this.mealPlanService = mealPlanService;
        }
        public async Task<bool> ExistsByIdAsync(string id)
        {
            return await this.dbContext.Users
                .AnyAsync(u => u.Id.ToString() == id);
        }

        public async Task<bool> IsOwnerByRecipeIdAsync(string recipeId, string userId)
        {
            return await this.dbContext
                .Recipes
                .Where(r => r.Id.ToString() == recipeId && r.OwnerId.ToString() == userId)
                .AnyAsync();
        }

        public async Task<bool> IsOwnerByMealPlanIdAsync(string id, string userId)
        {
            return await this.dbContext
                .MealPlans
                .AnyAsync(mp => mp.Id.ToString() == id &&
                          mp.OwnerId.ToString() == userId);
        }
               
        public async Task DeleteUserAsync(string userId)
        {
            // Delete related data first
            ICollection<Recipe> userRecipes = await this.dbContext.Recipes
                .Where(r => r.OwnerId.ToString() == userId)
                .ToListAsync();

            foreach (var recipe in userRecipes)
            {
                recipe.OwnerId = Guid.Parse(DeletedUserId.ToLower());
                await this.recipeService.DeleteByIdAsync(recipe.Id.ToString());
            }

            ICollection<MealPlan> userMealPlans = await this.dbContext.MealPlans
                .Where(mp => mp.OwnerId.ToString() == userId)
                .ToListAsync();

            foreach (var mealPlan in userMealPlans)
            {
                await this.mealPlanService.DeleteById(mealPlan.Id.ToString());
            }

            ICollection<FavouriteRecipe> userLikedRecipes = await this.dbContext
                .FavoriteRecipes
                .Where(fr => fr.UserId.ToString() == userId)
                .ToListAsync();

            this.dbContext.FavoriteRecipes.RemoveRange(userLikedRecipes);

            var user = await userManager.FindByIdAsync(userId);
            if (user != null)
            {
                await userManager.DeleteAsync(user);
            }

            await this.dbContext.SaveChangesAsync();

        }
    }
}
