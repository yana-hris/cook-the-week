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
        /// <exception cref="InvalidOperationException">Throws an exception if email confirmation was not successfully sent to the user</exception>
        Task<OperationResult> SendEmailConfirmationAsync(string email, string callBackUrl);


    }
}
