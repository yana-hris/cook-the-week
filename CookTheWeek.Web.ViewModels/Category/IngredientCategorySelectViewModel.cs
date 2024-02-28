namespace CookTheWeek.Web.ViewModels.Category
{
    using System.ComponentModel.DataAnnotations;

    public class IngredientCategorySelectViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;
    }
}
