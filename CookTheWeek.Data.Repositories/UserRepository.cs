
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

        /// <summary>
        /// Returns if a user by a specified id exists in the database
        /// </summary>
        /// <param name="id"></param>
        /// <returns>true or false</returns>
        public async Task<bool> ExistsByIdAsync(string id)
        {
            var user = await this.userManager.FindByIdAsync(id);

            if (user != null)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Gets the Application user if exists by email
        /// </summary>
        /// <param name="email"></param>
        /// <returns>Application User</returns>
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

        /// <summary>
        /// Returns the user, if existing or throws an exception
        /// </summary>
        /// <param name="id"></param>
        /// <returns>User or null</returns>
        /// <exception cref="RecordNotFoundException">If a user is not found, throws an exception</exception>
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
        public async Task<IdentityResult> CreateUserAsync(ApplicationUser user, string password)
        {
            return await this.userManager.CreateAsync(user, password);
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
        public async Task<bool> IsUserEmailConfirmedAsync(ApplicationUser user)
        {
            return await this.userManager.IsEmailConfirmedAsync(user);
        }

        /// <summary>
        /// Generates a password reset token for the specified user
        /// </summary>
        /// <param name="user"></param>
        /// <returns>token</returns>
        public async Task<string> GeneratePasswordResetTokenAsync(ApplicationUser user)
        {
            return await this.userManager.GeneratePasswordResetTokenAsync(user);
        }


        /// <summary>
        /// Returns a boolean, indicating if the current user has a password or not
        /// </summary>
        /// <param name="user"></param>
        /// <returns>true or false</returns>
        public Task<bool> HasPasswordAsync(ApplicationUser user)
        {
            return this.userManager.HasPasswordAsync(user);
        }

        /// <summary>
        /// Adds a password to an existing user
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <returns>The Identity Result</returns>
        public async Task<IdentityResult> AddPasswordAsync(ApplicationUser user, string password)
        {
            var result = await this.userManager.AddPasswordAsync(user, password);
            return result;
        }

        /// <summary>
        /// Returns is the current user`s password is true or false
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <returns>true or false</returns>
        public async Task<bool> CheckPasswordAsync(ApplicationUser user, string password)
        {
            var result = await this.userManager.CheckPasswordAsync(user, password);
            return result;
        }

        /// <summary>
        /// Changes the user`s current password with a new one
        /// </summary>
        /// <param name="user"></param>
        /// <param name="currentPass"></param>
        /// <param name="newPass"></param>
        /// <returns>The Identity Result</returns>
        public async Task<IdentityResult> ChangePassAsync(ApplicationUser user, string currentPass, string newPass)
        {
            var result = await this.userManager.ChangePasswordAsync(user, currentPass, newPass);
            return result;
        }

        /// <summary>
        /// Resets the user password with the new password if the token for the user is verified
        /// </summary>
        /// <param name="user"></param>
        /// <param name="token"></param>
        /// <param name="newPassword"></param>
        /// <returns>Identity Result</returns>
        public async Task<IdentityResult> ResetPasswordAsync(ApplicationUser user, string token, string newPassword)
        {
            return await this.userManager.ResetPasswordAsync(user, token, newPassword);
        }

        /// <summary>
        /// Deletes the user from the database
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task DeleteAsync(ApplicationUser user)
        {
            await this.userManager.DeleteAsync(user);
        }

        public async Task SignInUserAsync(ApplicationUser user)
        {
            await this.signInManager.SignInAsync(user, isPersistent: false);
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
        public string? GetCurrentUserId()
        {
            var user = httpContextAccessor.HttpContext?.User;
            return this.userManager.GetUserId(user);
        }
    }
}
