namespace CookTheWeek.Services.Data.Services
{
    
    using System.Threading.Tasks;

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using SendGrid;
    using SendGrid.Helpers.Mail;

    using CookTheWeek.Common;
    using CookTheWeek.Services.Data.Services.Interfaces;
    using CookTheWeek.Web.ViewModels.Home;
    using CookTheWeek.Services.Data.Helpers;
    using CookTheWeek.Web.ViewModels.ShoppingList;

    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration configuration;
        private readonly ILogger<EmailSender> logger;
        private readonly SendGridClient client;
        private readonly EmailFormatter formatter;

        public EmailSender(ILogger<EmailSender> logger,    
            IConfiguration configuration)
        {
            this.logger = logger;
            this.formatter = new EmailFormatter();

            this.configuration = configuration;
            this.client = new SendGridClient(configuration["SendGrid:ApiKey"]);
        }

        

        /// <inheritdoc/>
        public async Task<OperationResult> SendEmailConfirmationAsync(string email, string? callbackUrl)
        {
            if (string.IsNullOrEmpty(callbackUrl))
            {
                return OperationResult.Failure(new Dictionary<string, string>
                {
                    { "CallbackUrlError", "Empty callbackurl." }
                });
            }

            string subject = "❗️Confirm your registration";
            string htmlContent = formatter.GetEmailConfirmationHtml(email, callbackUrl);
            string plainTextContent = $"Please confirm your account by clicking this link: {callbackUrl}";

            var result = await SendEmailAsync(
                email,
                subject,
                plainTextContent,
                htmlContent);

            if (result.Succeeded)
            {
                return OperationResult.Success();
            }

            logger.LogError("Email Confirmation Send error. Action: {Action}, From: {FromEmail}, To: {ToEmail}, Errors: {Errors}",
                nameof(SendEmailConfirmationAsync),
                null,
                email,
                result.Errors);

            return OperationResult.Failure(result.Errors);
        }

        /// <inheritdoc/>
        public async Task<OperationResult> SendPasswordResetEmailAsync(string email, string username, string? callbackUrl, string tokenExpirationTime)
        {
            if (string.IsNullOrEmpty(callbackUrl))
            {
                return OperationResult.Failure(new Dictionary<string, string>
                {
                    { "CallbackUrlError", "Empty callbackurl." }
                });
            }

            string subject = "Password reset for CookTheWeek";
            string htmlContent = formatter.GetPasswordResetHtmlContent(username, callbackUrl, tokenExpirationTime);
            string plainTextContent = $"Please reset your password by clicking here. The link will be active until {tokenExpirationTime}";

            var result = await SendEmailAsync(
                email,
                subject,
                plainTextContent,
                htmlContent);

            if (result.Succeeded)
            {
                return OperationResult.Success();
            }

            logger.LogError("Password Reset Email sending failed. Action: {Action}, From: {FromEmail}, To: {ToEmail}, Errors: {Errors}",
                nameof(SendPasswordResetEmailAsync),
                null,
                email,
                result.Errors);

            return OperationResult.Failure(result.Errors);
        }

        /// <inheritdoc/>
        public async Task<OperationResult> SendContactFormEmailAsync(ContactFormModel model)
        {
            var to = new EmailAddress(configuration["EmailSettings:ToEmail"], "My personal Mail");
            var plainTextContent = $"Name: {model.FullName}\nEmail: {model.EmailAddress}\nMessage: {model.Message}";
            var htmlContent = $"<strong>Name:</strong> {model.FullName}<br><strong>Email:</strong> {model.EmailAddress}<br><strong>Message:</strong> {model.Message}";

            var result = await SendEmailAsync(
                model.EmailAddress, 
                model.Subject, 
                plainTextContent, 
                htmlContent);

            if (result.Succeeded)
            {
                return OperationResult.Success();
            }

            logger.LogError("Email Send error. Action: {Action}, From: {FromEmail}, To: {ToEmail}, Errors: {Errors}",
                nameof(SendContactFormEmailAsync),
                model.EmailAddress,
                to.Email,
                result.Errors);

            return OperationResult.Failure(result.Errors);
        }

        /// <inheritdoc/>
        public async Task<OperationResult> SendShoppingListEmailAsync(ShoppingListViewModel model)
        {
            
            var emailContent = formatter.GetShoppingListHtmlContent(model);
            var plainTextContent = "Here is your shopping List";
            var subject = $"Shopping List for {model.StartDate:MMMM d, yyyy} - {model.EndDate:MMMM d, yyyy}";

            var result = await SendEmailAsync(
                email: model.UserEmail,
                subject: subject,
                plainTextContent: plainTextContent,
                htmlMessage: emailContent);

            if (result.Succeeded)
            {
                return OperationResult.Success();
            }

            logger.LogError("Failed to send shopping list email. Action: {Action}, From: {FromEmail}, Errors: {Errors}",
                nameof(SendShoppingListEmailAsync),
                model.UserEmail,
                string.Join(", ", result.Errors.Select(e => $"{e.Key}: {e.Value}")));

            return OperationResult.Failure(result.Errors);
        }


        /// <summary>
        /// Utility method for sending emails reused in the Email Service only, handling exceptions by logging and appending errors to the Operation Result object
        /// </summary>
        /// <param name="email"></param>
        /// <param name="subject"></param>
        /// <param name="plainTextContent"></param>
        /// <param name="htmlMessage"></param>
        /// <returns>Operation Result (Success or Failure, with Errors Dictionary)</returns>
        private async Task<OperationResult> SendEmailAsync(string email, string subject, string plainTextContent, string htmlMessage)
        {
            string fromString = configuration["EmailSettings:FromEmail"];

            // Validate both emails
            if (string.IsNullOrEmpty(email) || !IsValidEmail(email) || string.IsNullOrEmpty(fromString) || !IsValidEmail(fromString))
            {
                logger.LogError("Invalid or missing email address. From: {FromEmail}, To: {ToEmail}", fromString ?? "null", email ?? "null");
                return OperationResult.Failure(new Dictionary<string, string>
                {
                    { "EmailError", "Invalid or missing email address." }
                });
            }

            try
            {
                var from = new EmailAddress(fromString, "CookTheWeek Support");
                var to = new EmailAddress(email);

                var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlMessage);
                var response = await client.SendEmailAsync(msg);

                if (response.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    return OperationResult.Success();
                }

                var responseBody = await response.Body.ReadAsStringAsync();
                logger.LogError("Failed to send email: {StatusCode}, {ResponseBody}", response.StatusCode, responseBody);

                return OperationResult.Failure(new Dictionary<string, string>
                {
                    { "EmailError", "Failed to send email." },
                    { "StatusCode", response.StatusCode.ToString() },
                    { "Response", responseBody }
                });
            }
            catch (FormatException ex)
            {
                logger.LogError(ex, "Invalid email address format: {Email}", email);
                return OperationResult.Failure(new Dictionary<string, string>
                {
                    { "EmailError", "Invalid email address format." }
                });
            }
            catch (HttpRequestException ex)
            {
                logger.LogError(ex, "Network error while sending email to {Email}", email);
                return OperationResult.Failure(new Dictionary<string, string>
                {
                    { "EmailError", "Network error occurred while sending email." }
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Unexpected error while sending email to {Email}", email);
                return OperationResult.Failure(new Dictionary<string, string>
                {
                    { "EmailError", "Unexpected error occurred." }
                });
            }
        }


        /// <summary>
        /// Utility method for validating email addresses
        /// </summary>
        /// <param name="email"></param>
        /// <returns>true or false</returns>
        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
