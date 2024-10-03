namespace CookTheWeek.Services.Data.Services.Interfaces
{

    using Microsoft.AspNetCore.Identity;

    using CookTheWeek.Common;
    using CookTheWeek.Web.ViewModels.Admin.UserAdmin;
    using CookTheWeek.Web.ViewModels.User;
    using Microsoft.AspNetCore.Authentication;

    public interface IUserService
    {
        Task<ICollection<UserAllViewModel>> AllAsync();
        Task<int?> AllCountAsync();
        Task<UserProfileViewModel> GetDetailsModelByIdAsync(string userId);
        Task<IdentityResult> ChangePasswordAsync(string userId, ChangePasswordFormModel model);
        Task<IdentityResult> SetPasswordAsync(string userId, SetPasswordFormModel model);
        
        /// <summary>
        /// Gets the currently logged in user and returns his/her id
        /// </summary>
        /// <returns>string or null</returns>
        string? GetCurrentUserId();


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
        /// Attempts to send a password reset email to the specified user. 
        /// If the user exists and their email is confirmed, a reset token is generated, 
        /// and an email is sent with the reset link.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<OperationResult> TrySendPasswordResetEmailAsync(ForgotPasswordFormModel model);

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
        /// <param name="userId">The ID of the user whose password is being changed.</param>
        /// <param name="model">The model containing the current and new passwords.</param>
        /// <returns>
        /// An OperationResult indicating whether the password change was successful, 
        /// along with any errors encountered during the process.
        /// </returns>
        Task<OperationResult> TryChangePasswordAsync(string userId, ChangePasswordFormModel model);

        /// <summary>
        /// Attempts to set a new password for the specified user.
        /// 
        /// This method verifies the existence of the user and sets a new password
        /// if the user does not already have one. If successful, it returns an 
        /// OperationResult indicating success. If the user is not found or the operation 
        /// fails, it returns an OperationResult with the appropriate errors.
        /// </summary>
        /// <param name="userId">The ID of the user whose password is being set.</param>
        /// <param name="model">The model containing the new password to be set.</param>
        /// <returns>
        /// An OperationResult indicating whether the password setting was successful, 
        /// along with any errors encountered during the process.
        /// </returns>

        Task<OperationResult> TrySetPasswordAsync(string userId, SetPasswordFormModel model);

        /// <summary>
        /// Tries to delete the account of the user and all relevant user data (recipes, mealplans, etc.).
        /// </summary>
        /// <returns>The Result of the Operation</returns>
        Task<OperationResult> TryDeleteUserAccountAsync();
        AuthenticationProperties? GetExternalLoginProperties(string schemeProvider, string? redirectUrl);
    }
}
