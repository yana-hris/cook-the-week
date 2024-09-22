namespace CookTheWeek.Services.Data
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    
    using CookTheWeek.Data.Models;
    using CookTheWeek.Data.Repositories;
    using Web.ViewModels.Admin.UserAdmin;
    using Web.ViewModels.User;
    using Interfaces;

    using static Common.GeneralApplicationConstants;
    using static Common.ExceptionMessagesConstants.InvalidOperationExceptionMessages;

    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;
        private readonly IRecipeRepository recipeRepository;
        private readonly IRecipeService recipeService;
        private readonly IEmailSender emailSender;
        private readonly IFavouriteRecipeRepository favouriteRecipeRepository;
        
        public UserService(IUserRepository userRepository,
            IRecipeRepository recipeRepository,
            IRecipeService recipeService,
            IEmailSender emailSender,
            IFavouriteRecipeRepository favouriteRecipeRepository)
        {
            this.userRepository = userRepository;
            this.recipeRepository = recipeRepository;
            this.recipeService = recipeService;
            this.emailSender = emailSender;
            this.favouriteRecipeRepository = favouriteRecipeRepository;
        }
        public async Task<UserProfileViewModel> DetailsByIdAsync(string userId)
        {
            // This Throws RecordNotFoundException
            var user = await this.userRepository.GetUserByIdAsync(userId);
            
            bool hasPassword = await userRepository.HasPasswordAsync(user);

            return new UserProfileViewModel
            {
                UserName = user.UserName!,
                Email = user.Email!,
                HasPassword = hasPassword
            };
        }
        public async Task<ApplicationUser> CreateUserAsync(RegisterFormModel model)
        {
            ApplicationUser user = new ApplicationUser()
            {
                UserName = model.Username,
                Email = model.Email
            };

            IdentityResult identityResult = await userRepository.CreateUserAsync(user, model.Password);

            if (!identityResult.Succeeded)
            {
                // Collect error messages from the IdentityResult and throw an exception
                var errorMessages = string.Join("; ", identityResult.Errors.Select(e => e.Description));
                throw new InvalidOperationException($"{UserUnsuccessfullyCreatedException} Errors: {errorMessages}");  // Custom exception
            }

            return user;
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
                // Assign a pre-defined "Dleted" user id to existing use recipes
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

        /// <summary>
        /// Returns the sum of all users, registered in the database
        /// </summary>
        /// <returns>int or 0</returns>
        public async Task<int?> AllCountAsync()
        {
            return await this.userRepository.AllCountAsync();
        }

        /// <summary>
        /// Returns a collection of all users, registered in the database
        /// </summary>
        /// <returns>A collection of UserAllViewModel</returns>
        public async Task<ICollection<UserAllViewModel>> AllAsync()
        {
            var users = await this.userRepository.GetAll();
            var model = users.Select(u => new UserAllViewModel()
            {
                Id = u.Id.ToString(),
                Username = u.UserName,
                Email = u.Email
            }).ToList();

            foreach (var user in model)
            {
                user.TotalRecipes = await 
            }
        }


    }
}
