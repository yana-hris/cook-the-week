namespace CookTheWeek.Services.Data.Interfaces
{
    using SendGrid;

    public interface IEmailSender
    {
        Task<Response?> SendEmailAsync(string email, string subject, string plainTextContent, string htmlMessage);

        /// <summary>
        /// Sends an email with a confirmation token to the user for further email confirmation
        /// </summary>
        /// <param name="email"></param>
        /// <param name="callBackUrl">The Url, sent to the user</param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">Throws an exception if email confirmation was not successfully sent to the user</exception>
        Task SendEmailConfirmationAsync(string email, string callBackUrl);
    }
}
