namespace CookTheWeek.Web.ViewModels.Home
{
    using System.ComponentModel.DataAnnotations;

    public class ContactFormModel
    {
        [Required]
        public string FullName { get; set; } = null!;

        [Required]
        [EmailAddress]
        public string EmailAddress { get; set; } = null!;

        [Required]
        public string Subject { get; set; } = null!;

        [Required]
        public string Message { get; set; } = null!;
    }
}
