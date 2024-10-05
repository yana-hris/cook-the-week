namespace CookTheWeek.Services.Data.Services.Interfaces
{
    using CookTheWeek.Common;
    using CookTheWeek.Web.ViewModels.Home;

    public interface IEmailSender
    {
        /// <summary>
        /// Sends an email from the contact form of the website to the support email address.
        /// </summary>
        /// <param name="model"></param>
        /// <returns>The result from the operation</returns>
        Task<OperationResult> SendContactFormEmailAsync(ContactFormModel model);
        
        /// <summary>
        /// Sends an email with a confirmation token to the user for further email confirmation
        /// </summary>
        /// <param name="email"></param>
        /// <param name="callBackUrl">The Url, sent to the user</param>
        /// <returns></returns>
        Task<OperationResult> SendEmailConfirmationAsync(string email, string? callBackUrl);

        /// <summary>
        /// Sends an email with a confirmation token to a given email for password reset
        /// </summary>
        /// <param name="email"></param>
        /// <param name="callbackUrl"></param>
        /// <param name="tokenExpirationTime"></param>
        /// <returns></returns>
        Task<OperationResult> SendPasswordResetEmailAsync(string email, string callbackUrl, string tokenExpirationTime);
    }
}
