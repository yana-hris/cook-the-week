namespace CookTheWeek.Services.Data
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    using CookTheWeek.Data;
    using CookTheWeek.Data.Models;
    using CookTheWeek.Web.ViewModels.User;
    using Interfaces;

    using static CookTheWeek.Common.GeneralApplicationConstants;
    using CookTheWeek.Data.Repositories;

    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;
        private readonly IRecipeRepository recipeRepository;
        private readonly IRecipeService recipeService;
        private readonly IFavouriteRecipeRepository favouriteRecipeRepository;
        
        public UserService(IUserRepository userRepository,
            IRecipeRepository recipeRepository,
            IRecipeService recipeService,
            IFavouriteRecipeRepository favouriteRecipeRepository)
        {
            this.userRepository = userRepository;
            this.recipeRepository = recipeRepository;
            this.recipeService = recipeService;
            this.favouriteRecipeRepository = favouriteRecipeRepository;
        }
        public async Task<UserProfileViewModel?> GetProfileDetailsAync(string userId)
        {
            // userRepository.GetUserByIdAsync()
            var user = await this.userRepository.GetUserByIdAsync(userId);

            if (user == null)
            {
                return null;
            }

            bool hasPassword = hasPassword = await userRepository.HasPasswordAsync(user);

            return new UserProfileViewModel
            {
                UserName = user.UserName!,
                Email = user.Email!,
                HasPassword = hasPassword
            };
        }
        public async Task<IdentityResult> ChangePasswordAsync(string userId, ChangePasswordFormModel model)
        {
            var user = await userRepository.GetUserByIdAsync(userId);

            if (user == null)
            {
                // Returning an error result if the user is not found
                return IdentityResult.Failed(new IdentityError { Description = UserNotFoundErrorMessage });
            }

            // Check if the current password matches
            // userRepository.CheckPasswordAsync
            bool oldPasswordMatches = await userRepository.CheckPasswordAsync(user, model.CurrentPassword);

            if(!oldPasswordMatches)
            {
                return IdentityResult.Failed(new IdentityError { Description = IncorrectPasswordErrorMessage });
            }

            var result = await userRepository.ChangePassAsync(user, model.CurrentPassword, model.NewPassword);

            return result;
        }

        public async Task<IdentityResult> SetPasswordAsync(string userId, SetPasswordFormModel model)
        {
            var user = await userRepository.GetUserByIdAsync(userId);

            if (user == null)
            {
                // Returning an error result if the user is not found
                return IdentityResult.Failed(new IdentityError { Description = UserNotFoundErrorMessage });
            }

            var result = await userRepository.AddPasswordAsync(user, model.NewPassword);

            return result;
        }

        public async Task DeleteUserAndUserDataAsync(string userId)
        {
            // Delete related data first
            ICollection<Recipe> userRecipes = await this.recipeRepository.GetAllByUserIdAsync(userId);

            foreach (var recipe in userRecipes)
            {
                recipe.OwnerId = Guid.Parse(DeletedUserId.ToLower());
                await this.recipeService.DeleteByIdAsync(recipe.Id.ToString(), userId, false);
            }

            ICollection<MealPlan> userMealPlans = await this.dbContext.MealPlans
                .Where(mp => mp.OwnerId.ToString() == userId)
                .ToListAsync();

            foreach (var mealPlan in userMealPlans)
            {
                await this.mealPlanService.DeleteById(mealPlan.Id.ToString());
            }

            await this.favouriteRecipeRepository.DeleteAllByUserIdAsync(userId);

            var user = await userRepository.GetUserByIdAsync(userId);
            if (user != null)
            {
                await userRepository.DeleteAsync(user);
            }

            await this.dbContext.SaveChangesAsync();

        }

    }
}
