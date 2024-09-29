namespace CookTheWeek.Services.Data.Services
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    using CookTheWeek.Data.Models;
    using CookTheWeek.Data.Repositories;
    using Interfaces;
    using Web.ViewModels.Admin.UserAdmin;
    using Web.ViewModels.User;

    using static Common.GeneralApplicationConstants;
    using static Common.ExceptionMessagesConstants;
    using Microsoft.AspNetCore.Http;

    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;
        private readonly IRecipeRepository recipeRepository;
        private readonly IRecipeService recipeService;
        private readonly IEmailSender emailSender;
        private readonly IMealPlanService mealPlanService;
        private readonly IFavouriteRecipeRepository favouriteRecipeRepository;
        private readonly IHttpContextAccessor httpContextAccessor;

        public UserService(IUserRepository userRepository,
            IRecipeRepository recipeRepository,
            IRecipeService recipeService,
            IEmailSender emailSender,
            IMealPlanService mealPlanService,
            IHttpContextAccessor httpContextAccessor,
            IFavouriteRecipeRepository favouriteRecipeRepository)
        {
            this.userRepository = userRepository;
            this.recipeRepository = recipeRepository;
            this.mealPlanService = mealPlanService;
            this.emailSender = emailSender;
            this.favouriteRecipeRepository = favouriteRecipeRepository;
            this.httpContextAccessor = httpContextAccessor;
        }
        public async Task<UserProfileViewModel> DetailsByIdAsync(string userId)
        {
            // This Throws RecordNotFoundException
            var user = await userRepository.GetByIdAsync(userId);

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

            IdentityResult identityResult = await userRepository.AddAsync(user, model.Password);

            if (!identityResult.Succeeded)
            {
                // Collect error messages from the IdentityResult and throw an exception
                var errorMessages = string.Join("; ", identityResult.Errors.Select(e => e.Description));
                throw new InvalidOperationException($"{InvalidOperationExceptionMessages.UserUnsuccessfullyCreatedExceptionMessage} Errors: {errorMessages}");  // Custom exception
            }

            return user;
        }
        public async Task<IdentityResult> ChangePasswordAsync(string userId, ChangePasswordFormModel model)
        {
            var user = await userRepository.GetByIdAsync(userId);

            if (user == null)
            {
                // Returning an error result if the user is not found
                return IdentityResult.Failed(new IdentityError { Description = UserNotFoundErrorMessage });
            }

            // Check if the current password matches
            // userRepository.CheckPasswordAsync
            bool oldPasswordMatches = await userRepository.CheckPasswordAsync(user, model.CurrentPassword);

            if (!oldPasswordMatches)
            {
                return IdentityResult.Failed(new IdentityError { Description = IncorrectPasswordErrorMessage });
            }

            var result = await userRepository.ChangePassAsync(user, model.CurrentPassword, model.NewPassword);

            return result;
        }

        public async Task<IdentityResult> SetPasswordAsync(string userId, SetPasswordFormModel model)
        {
            var user = await userRepository.GetByIdAsync(userId);

            if (user == null)
            {
                // Returning an error result if the user is not found
                return IdentityResult.Failed(new IdentityError { Description = UserNotFoundErrorMessage });
            }

            var result = await userRepository.AddPasswordAsync(user, model.NewPassword);

            return result;
        }

        /// <summary>
        /// Deletes the user and all user data: meal plans (and related meals), added recipes, favourite recipes.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task DeleteUserAndUserDataAsync(string userId)
        {
            // Delete related data first
            await mealPlanService.DeleteAllByUserIdAsync(userId);
            await recipeService.DeleteAllByUserIdAsync(userId);
            await favouriteRecipeRepository.DeleteAllByUserIdAsync(userId);

            var user = await userRepository.GetByIdAsync(userId);

            if (user != null)
            {
                await userRepository.DeleteAsync(user);
            }
        }

        /// <summary>
        /// Returns the sum of all users, registered in the database
        /// </summary>
        /// <returns>int or 0</returns>
        public async Task<int?> AllCountAsync()
        {
            return await userRepository.AllCountAsync();
        }

        /// <summary>
        /// Returns a collection of UserAllViewModel which contains all users, registered in the database
        /// </summary>
        /// <returns>A collection of UserAllViewModel</returns>
        public async Task<ICollection<UserAllViewModel>> AllAsync()
        {
            ICollection<UserAllViewModel> users = await userRepository
                .GetAllQuery()
                .Select(u => new UserAllViewModel()
                {
                    Id = u.Id.ToString(),
                    Username = u.UserName,
                    Email = u.Email

                }).ToListAsync();

            foreach (var user in users)
            {
                user.TotalRecipes = await recipeService.GetMineCountAsync(user.Id.ToString());
                user.TotalMealPlans = await mealPlanService.MineCountAsync(user.Id);
            }

            return users;
        }

        public async Task<string> GenerateTokenForEmailConfirmationAsync(ApplicationUser user)
        {
            if (user != null)
            {
                string? token = await userRepository.GenerateEmailConfirmationTokenAsync(user);

                if (token == null)
                {
                    throw new InvalidOperationException(InvalidOperationExceptionMessages.TokenGenerationUnsuccessfullExceptionMessage);
                }
            }

            throw new ArgumentNullException(ArgumentNullExceptionMessages.UserNullExceptionMessage);
        }


        /// <inheritdoc/>
        public async Task DeleteUserAsync(ApplicationUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(ArgumentNullExceptionMessages.UserNullExceptionMessage);
            }

            try
            {
                await userRepository.DeleteAsync(user);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(InvalidOperationExceptionMessages
                    .UserUnsuccessfullyDeletedExceptionMessage);
            }
        }

        /// <summary>
        /// Gets the currently logged in user id
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public string? GetCurrentUserId()
        {
            var user = httpContextAccessor.HttpContext?.User;
            string? userId = userRepository.GetUserId(user);

            return userId;
        }
    }
}
