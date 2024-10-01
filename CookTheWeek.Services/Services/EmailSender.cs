namespace CookTheWeek.Services.Data.Services
{
    using System.Net.Mail;
    using System.Threading.Tasks;

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using SendGrid;
    using SendGrid.Helpers.Mail;

    using CookTheWeek.Services.Data.Services.Interfaces;

    using static CookTheWeek.Common.ExceptionMessagesConstants;
    using CookTheWeek.Web.ViewModels.Home;
    using CookTheWeek.Common;

    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration configuration;
        private readonly ILogger<EmailSender> logger;
        private readonly SendGridClient client;

        public EmailSender(ILogger<EmailSender> logger,    
            IConfiguration configuration)
        {
            this.logger = logger;
            this.configuration = configuration;
            this.client = new SendGridClient(configuration["SendGrid:ApiKey"]);
        }

        

        /// <inheritdoc/>
        public async Task<OperationResult> SendEmailConfirmationAsync(string email, string callBackUrl)
        {
            var result = await SendEmailAsync(
                email,
                "Confirm your email with CookTheWeek",
                $"Please confirm your account by clicking this link: {callBackUrl}",
                $"Please confirm your account by clicking this link: <a href='{callBackUrl}'>link</a>");

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

        public async Task<OperationResult> SendContactFormEmailAsync(ContactFormModel model)
        {
            var to = new EmailAddress(configuration["EmailSettings:ToEmail"], "My personal Mail");
            var plainTextContent = $"Name: {model.FullName}\nEmail: {model.EmailAddress}\nMessage: {model.Message}";
            var htmlContent = $"<strong>Name:</strong> {model.FullName}<br><strong>Email:</strong> {model.EmailAddress}<br><strong>Message:</strong> {model.Message}";

            var result = await SendEmailAsync(model.EmailAddress, model.Subject, plainTextContent, htmlContent);

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
            try
            {
                var from = new EmailAddress(configuration["EmailSettings:FromEmail"], "CookTheWeek Support");
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

    }
}
