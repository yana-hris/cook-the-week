namespace CookTheWeek.Web.ViewModels.User
{
    using System.ComponentModel.DataAnnotations;
    using static Common.EntityValidationConstants.ApplicationUserValidation;

    public class SetPasswordFormModel
    {
        
        [Required]
        [DataType(DataType.Password)]
        [StringLength(PasswordMaxLength, MinimumLength = PasswordMinLength, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.")]
        [Display(Name = "New Password")]
        public string NewPassword { get; set; } = null!;

        [DataType(DataType.Password)]
        [Display(Name = "Confirm New Password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmNewPassword { get; set; } = null!;

    }
}
