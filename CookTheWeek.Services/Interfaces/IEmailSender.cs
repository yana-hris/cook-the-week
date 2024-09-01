namespace CookTheWeek.Services.Data.Interfaces
{
    using SendGrid;

    public interface IEmailSender
    {
        Task<Response?> SendEmailAsync(string email, string subject, string plainTextContent, string htmlMessage);
    }
}
