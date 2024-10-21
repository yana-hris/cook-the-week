namespace CookTheWeek.Services.Data.Services.Interfaces
{
    
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Identity;

    using CookTheWeek.Common;
    using CookTheWeek.Web.ViewModels.Admin.UserAdmin;
    using CookTheWeek.Web.ViewModels.User;

    public interface IUserService
    {
        /// <summary>
        /// Gets All users View Model collection
        /// </summary>
        /// <returns>ICollection<UserAllViewModel></returns>
        Task<ICollection<UserAllViewModel>> GetAllAsync();

        /// <summary>
        /// Gets the total count of all users
        /// </summary>
        /// <returns>int or 0</returns>
        Task<int?> AllCountAsync();

        /// <summary>
        /// Gets the currently logged in user details or throws an exception
        /// </summary>
        /// <returns>UserProfileViewModel</returns>
        /// <exception cref="RecordNotFoundException"></remarks>
        Task<UserProfileViewModel> GetUserProfileDetailsAsync();
        Task<IdentityResult> ChangePasswordAsync(ChangePasswordFormModel model);
        Task<IdentityResult> SetPasswordAsync(SetPasswordFormModel model);
        
        /// <summary>
        /// Takes a RegisterFormModel and tries to create an ApplicationUser in the database. If creation is successful, proceeds to generating a Token for Email confirmation. If token generation is successful, sends an email to the user with the confirmation link.
        /// Logs errors and catches exceptions on the way. In case of failure, returns a dictionary with Errors.
        /// </summary>
        /// <param name="model"></param>
        /// <returns>The result of the Operation (Success or Failure)</returns>
        Task<OperationResult> TryRegisterUserAsync(RegisterFormModel model);

        /// <summary>
        /// Finds the user by the userId and if not null, proceeds to email confirmation using the given token. If successful, logs the user in.
        /// If token or user are null, a Failure result is returned.
        /// Does not throw any exceptions, catches RecordNotFound and ArgumentNullExceptions from previous methods.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="code"></param>
        /// <returns>The Operation Result (Success or Failure)</returns>
        Task<OperationResult> TyrConfirmEmailAsync(string userId, string code);

        /// <summary>
        /// Attempts to log the user in with a given password and handles the cases when the user is locked out
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<OperationResult> TryLoginUserAsync(LoginFormModel model);
        
        /// <summary>
        /// Creates a new user, using the external login credentials or logs in the user if it already exists in the database (using the user`s email)
        /// </summary>
        /// <returns>The result of the operation or throws exceptions</returns>
        /// <exception cref="InvalidOperationException"></exception>
        Task<OperationResult> TryLoginOrRegisterUserWithExternalLoginProvider();

        /// <summary>
        /// Tries to sign out the current user and deletes the CookieConsent Cookie or throws an exception
        /// </summary>
        /// <returns></returns>
        Task TryLogOutUserAsync();

        /// <summary>
        /// Attempts to generate a password reset token for a given user by email.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<OperationResult> TryGetPasswordResetTokenAsync(string email);

        /// <summary>
        /// Tries to reset the user`s password after validating his existence and token with the database.
        /// </summary>
        /// <param name="model"></param>
        /// <returns>The Result of the Operation</returns>
        Task<OperationResult> TryResetPasswordAsync(ResetPasswordFormModel model);

        /// <summary>
        /// Attempts to change the password for the specified user.
        /// 
        /// This method validates the user's current password, ensures the user exists,
        /// and processes the password change. If successful, it returns an 
        /// OperationResult indicating success; otherwise, it returns the appropriate 
        /// errors, such as incorrect current password or user not found.
        /// </summary>
        /// <param name="model">The model containing the current and new passwords.</param>
        /// <returns>
        /// An OperationResult indicating whether the password change was successful, 
        /// along with any errors encountered during the process.
        /// </returns>
        Task<OperationResult> TryChangePasswordAsync(ChangePasswordFormModel model);

        /// <summary>
        /// Attempts to set a new password for the specified user.
        /// 
        /// This method verifies the existence of the user and sets a new password
        /// if the user does not already have one. If successful, it returns an 
        /// OperationResult indicating success. If the user is not found or the operation 
        /// fails, it returns an OperationResult with the appropriate errors.
        /// </summary>
        /// <param name="model">The model containing the new password to be set.</param>
        /// <returns>
        /// An OperationResult indicating whether the password setting was successful, 
        /// along with any errors encountered during the process.
        /// </returns>

        Task<OperationResult> TrySetPasswordAsync(SetPasswordFormModel model);

        /// <summary>
        /// Tries to delete the account of the user and all relevant user data (recipes, mealplans, etc.).
        /// </summary>
        /// <returns>The Result of the Operation</returns>
        Task<OperationResult> TryDeleteUserAccountAsync();
        AuthenticationProperties? GetExternalLoginProperties(string schemeProvider, string? redirectUrl);
        
        /// <summary>
        /// Sends a confirmation email to the given email and includes the given callback url or throws an exception if email sending fails
        /// </summary>
        /// <param name="email"></param>
        /// <param name="callbackUrl"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        Task SendConfirmationEmailAsync(string email, string? callbackUrl);

        /// <summary>
        /// Deletes a user upon accepting user id. If the user does not exist, does nothing.
        /// </summary>
        /// <param name="user">The user ID</param>
        /// <returns></returns>
        Task DeleteUserByIdIfExistsAsync(Guid userId);
        
        /// <summary>
        /// Sends the given email a password reset token (link) or throws an exception
        /// </summary>
        /// <param name="email"></param>
        /// <param name="callbackUrl"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        Task SendPasswordResetEmailAsync(string email, string? callbackUrl);
    }
}
