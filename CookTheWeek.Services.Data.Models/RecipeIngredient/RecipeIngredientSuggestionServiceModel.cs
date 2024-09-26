namespace CookTheWeek.Services.Data.Models.RecipeIngredient
{
    using System.ComponentModel.DataAnnotations;

    public class RecipeIngredientSuggestionServiceModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;
    }
}
