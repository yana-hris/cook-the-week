namespace CookTheWeek.Web.ViewModels.Admin.CategoryAdmin
{
    using System.ComponentModel.DataAnnotations;

    using CookTheWeek.Web.ViewModels.Interfaces;

    using static Common.EntityValidationConstants.Category;

    public class RecipeCategoryAddFormModel : ICategoryFormModel
    {
        [Required]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength, ErrorMessage = @"Recipe Category Name should be between {2} and {1} characters long")]
        public string Name { get; set; } = null!;
    }
}
