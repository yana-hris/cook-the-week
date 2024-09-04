namespace CookTheWeek.Web.ViewModels.User
{
    using System.ComponentModel.DataAnnotations;

    public class ForgotPasswordFormModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;
    }
}
