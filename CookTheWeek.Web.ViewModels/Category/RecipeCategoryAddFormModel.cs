﻿namespace CookTheWeek.Web.ViewModels.Category
{
    using System.ComponentModel.DataAnnotations;

    using static Common.EntityValidationConstants.RecipeCategory;

    public class RecipeCategoryAddFormModel
    {
        [Required]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength, ErrorMessage = @"Recipe Category Name should be between {2} and {1} characters long")]
        public string Name { get; set; } = null!;
    }
}
