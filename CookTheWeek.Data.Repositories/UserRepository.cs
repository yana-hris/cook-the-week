
namespace CookTheWeek.Data.Repositories
{
    using System.Security.Claims;

    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    using CookTheWeek.Data.Models;
    using CookTheWeek.Common.Exceptions;

    using static CookTheWeek.Common.ExceptionMessagesConstants;
    using static CookTheWeek.Common.GeneralApplicationConstants;

    class UserRepository : IUserRepository
    {
        private readonly CookTheWeekDbContext dbContext;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;

        public UserRepository(CookTheWeekDbContext dbContext,            
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            this.dbContext = dbContext;
            
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        /// <inheritdoc/>   
        public async Task<bool> ExistsByIdAsync(string id)
        {
            var user = await this.userManager.FindByIdAsync(id);

            if (user != null)
            {
                return true;
            }

            return false;
        }
        
        /// <inheritdoc/>
        public async Task<ApplicationUser?> GetByEmailAsync(string email)
        {
            var user = await this.userManager.FindByEmailAsync(email);
            return user;
        }

        /// <inheritdoc/>       
        public async Task<ApplicationUser?> GetByUsernameAsync(string userName)
        {
            return await this.userManager.FindByNameAsync(userName);
        }

        /// <inheritdoc/>
        public async Task<ApplicationUser?> GetByIdAsync(string id)
        {
            return await this.userManager.FindByIdAsync(id);
        }

        /// <inheritdoc/>
        public async Task SignInAsync(ApplicationUser user)
        {
            await this.signInManager.SignInAsync(user, isPersistent: false);
        }

        /// <inheritdoc/>
        public async Task<IdentityResult> CreateUserWithPasswordAsync(ApplicationUser user, string password)
        {
            return await this.userManager.CreateAsync(user, password);
        }

        /// <inheritdoc/>
        public async Task<bool> IsEmailConfirmedAsync(ApplicationUser user)
        {
            return await this.userManager.IsEmailConfirmedAsync(user);
        }

        /// <inheritdoc/>
        public async Task<string> GenerateEmailConfirmationTokenAsync(ApplicationUser user)
        {
            return await this.userManager.GenerateEmailConfirmationTokenAsync(user);
        }

        /// <inheritdoc/>
        public async Task<IdentityResult> ConfirmEmailAsync(ApplicationUser user, string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                throw new ArgumentNullException(ArgumentNullExceptionMessages.TokenNullExceptionMessage);
            }

            if (user == null)
            {
                throw new RecordNotFoundException(RecordNotFoundExceptionMessages.UserNotFoundExceptionMessage, null);
            }

            return await this.userManager.ConfirmEmailAsync(user, code);
        }

        /// <inheritdoc/>
        public async Task<string?> GeneratePasswordResetTokenAsync(ApplicationUser user)
        {
            return await this.userManager.GeneratePasswordResetTokenAsync(user);
        }

        /// <inheritdoc/>
        public Task<bool> HasPasswordAsync(ApplicationUser user)
        {
            return this.userManager.HasPasswordAsync(user);
        }

        /// <inheritdoc/>
        public async Task<IdentityResult> AddPasswordAsync(ApplicationUser user, string password)
        {
            var result = await this.userManager.AddPasswordAsync(user, password);
            return result;
        }

        /// <inheritdoc/>
        public async Task<bool> CheckPasswordAsync(ApplicationUser user, string password)
        {
            var result = await this.userManager.CheckPasswordAsync(user, password);
            return result;
        }

        /// <inheritdoc/>
        public async Task<IdentityResult> ChangePassAsync(ApplicationUser user, string currentPass, string newPass)
        {
            var result = await this.userManager.ChangePasswordAsync(user, currentPass, newPass);
            return result;
        }

        /// <inheritdoc/>
        public async Task<IdentityResult> ResetPasswordAsync(ApplicationUser user, string token, string newPassword)
        {
            return await this.userManager.ResetPasswordAsync(user, token, newPassword);
        }

        /// <inheritdoc/>
        public async Task DeleteAsync(ApplicationUser user)
        {
            await this.userManager.DeleteAsync(user);
        }
       
        /// <inheritdoc/>
        public async Task<int?> AllCountAsync()
        {
            int? users = await this.dbContext.Users.AsNoTracking().CountAsync();
            return users;
        }

        /// <inheritdoc/>
        public IQueryable<ApplicationUser> GetAllQuery()
        {
            return this.dbContext.Users.AsQueryable();
                
        }

        /// <inheritdoc/>
        public string? GetUserId(ClaimsPrincipal? user)
        {
            string? userId = userManager.GetUserId(user);
            return userId;
        }


        /// <inheritdoc/>
        public async Task<SignInResult> PasswordSignInAsync(ApplicationUser user, string password, bool rememberMe)
        {
            return await signInManager.PasswordSignInAsync(user, password, isPersistent: false, lockoutOnFailure: true);
        }


        /// <inheritdoc/>
        public async Task AccessFailedAsync(ApplicationUser user)
        {
            await userManager.AccessFailedAsync(user);
        }

        /// <inheritdoc/>
        public async Task<bool> IsUserInAdminRoleAsync(ApplicationUser user)
        {
            return await userManager.IsInRoleAsync(user, AdminRoleName);
        }

        /// <inheritdoc/>
        public AuthenticationProperties? ConfigureExternalAuthenticationProperties(string schemeProvider, string? redirectUrl)
        {
            return signInManager.ConfigureExternalAuthenticationProperties(schemeProvider, redirectUrl);
        }

        /// <inheritdoc/>
        public async Task<ExternalLoginInfo?> GetExternalLoginInfoAsync()
        {
            return await signInManager.GetExternalLoginInfoAsync();
        }

        /// <inheritdoc/>
        public async Task<IdentityResult> CreateUserWithoutPasswordAsync(ApplicationUser user)
        {
            return await this.userManager.CreateAsync(user);
        }


        /// <inheritdoc/>
        public async Task<IdentityResult> AddLoginAsync(ApplicationUser user, ExternalLoginInfo info)
        {
            return await userManager.AddLoginAsync(user, info);
        }

        /// <inheritdoc/>
        public async Task SignOutAsync()
        {
            await signInManager.SignOutAsync();
        }

        /// <inheritdoc/>
        public async Task<bool> IsUserEmailConfirmedAsync(ApplicationUser user)
        {
            return await userManager.IsEmailConfirmedAsync(user);
        }

        /// <inheritdoc/>
        public async Task<IdentityResult> SetPasswordAsync(ApplicationUser user, string newPassword)
        {
            // Check if the user already has a password set
            var hasPassword = await userManager.HasPasswordAsync(user);

            if (hasPassword)
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Code = "PasswordAlreadySet",
                    Description = "The user already has a password set."
                });
            }

            // Add the new password
            var result = await userManager.AddPasswordAsync(user, newPassword);
            
            return result;
        }
    }
}
