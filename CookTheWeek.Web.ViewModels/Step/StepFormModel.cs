﻿namespace CookTheWeek.Web.ViewModels.Step
{
    using System.ComponentModel.DataAnnotations;

    using static CookTheWeek.Common.EntityValidationConstants.Step;
    public class StepFormModel
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = StepRequiredErrorMessage)]
        [Display(Name = "Cooking Step")]
        [StringLength(DescriptionMaxLength, MinimumLength = DescriptionMinLength, ErrorMessage = StepDescriptionRangeErrorMessage)]
        public string Description { get; set; } = null!;
    }
}
