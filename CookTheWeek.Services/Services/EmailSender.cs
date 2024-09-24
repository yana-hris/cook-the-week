namespace CookTheWeek.Services.Data.Services
{
    using System.Threading.Tasks;

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Options;
    using SendGrid;
    using SendGrid.Helpers.Mail;

    using static CookTheWeek.Common.ExceptionMessagesConstants.SmtpExceptionMessages;
    using System.Net.Mail;
    using CookTheWeek.Data.Repositories;
    using CookTheWeek.Services.Data.Services.Interfaces;

    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration configuration;
        private readonly SendGridClientOptions options;

        public EmailSender(IOptions<SendGridClientOptions> options,
            IConfiguration configuration)
        {
            this.options = options.Value;
            this.configuration = configuration;
        }
        public async Task<Response?> SendEmailAsync(string email, string subject, string plainTextContent, string htmlMessage)
        {
            var client = new SendGridClient(options.ApiKey);
            var from = new EmailAddress(configuration["EmailSettings:FromEmail"], "CookTheWeek Support");
            var to = new EmailAddress(email);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlMessage);
            Response? response = await client.SendEmailAsync(msg);

            return response;
        }


        /// <inheritdoc/>
        public async Task SendEmailConfirmationAsync(string email, string callBackUrl)
        {
            var responseResult = await SendEmailAsync(
                email,
                "Confirm your email with CookTheWeek",
                $"Please confirm your account by clicking this link: {callBackUrl}",
                $"Please confirm your account by clicking this link: <a href='{callBackUrl}'>link</a>");

            if (responseResult != null && !responseResult.IsSuccessStatusCode)
            {
                throw new SmtpException(EmailConfirmationUnsuccessfullySentToUser);
            }
        }
    }
}
