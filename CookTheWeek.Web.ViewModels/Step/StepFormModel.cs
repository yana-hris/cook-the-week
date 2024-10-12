namespace CookTheWeek.Web.ViewModels.Step
{
    using System.ComponentModel.DataAnnotations;

    using static CookTheWeek.Common.EntityValidationConstants.StepValidation;
    public class StepFormModel
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = StepRequiredErrorMessage)]
        [Display(Name = "Cooking Step")]
        [StringLength(StepDescriptionMaxLength, MinimumLength = StepDescriptionMinLength, ErrorMessage = StepDescriptionRangeErrorMessage)]
        public string Description { get; set; } = null!;
    }
}
