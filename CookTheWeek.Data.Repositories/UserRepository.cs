
namespace CookTheWeek.Data.Repositories
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    using CookTheWeek.Data.Models;
    using CookTheWeek.Common.Exceptions;

    using static CookTheWeek.Common.ExceptionMessagesConstants.RecordNotFoundExceptionMessages;
    using static CookTheWeek.Common.ExceptionMessagesConstants.ArgumentNullExceptionMessages;

    class UserRepository : IUserRepository
    {
        private readonly CookTheWeekDbContext dbContext;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IHttpContextAccessor httpContextAccessor;

        public UserRepository(CookTheWeekDbContext dbContext,
            IHttpContextAccessor httpContextAccessor,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            this.dbContext = dbContext;
            this.httpContextAccessor = httpContextAccessor;
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
        public string? GetCurrentUserId()
        {
            var user = httpContextAccessor.HttpContext?.User;
            return this.userManager.GetUserId(user);
        }

        /// <inheritdoc/>
        public async Task<ApplicationUser?> FindByEmailAsync(string email)
        {
            var user = await this.userManager.FindByEmailAsync(email);
            return user;
        }

        /// <inheritdoc/>       
        public async Task<ApplicationUser?> FindByNameAsync(string userName)
        {
            return await this.userManager.FindByNameAsync(userName);
        }

        /// <inheritdoc/>
        public async Task<ApplicationUser> GetUserByIdAsync(string id)
        {
            var user = await this.userManager.FindByIdAsync(id);

            if (user == null)
            {
                throw new RecordNotFoundException(UserNotFoundExceptionMessage, null);
            }

            return user;
        }

        /// <inheritdoc/>
        public async Task SignInUserAsync(ApplicationUser user)
        {
            await this.signInManager.SignInAsync(user, isPersistent: false);
        }

        /// <inheritdoc/>
        public async Task<IdentityResult> CreateUserAsync(ApplicationUser user, string password)
        {
            return await this.userManager.CreateAsync(user, password);
        }

        /// <inheritdoc/>
        public async Task<bool> IsUserEmailConfirmedAsync(ApplicationUser user)
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
                throw new ArgumentNullException(TokenNullExceptionMessage);
            }

            if (user == null)
            {
                throw new RecordNotFoundException(UserNotFoundExceptionMessage, null);
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

        
    }
}
