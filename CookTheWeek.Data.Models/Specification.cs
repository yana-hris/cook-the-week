namespace CookTheWeek.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using Microsoft.EntityFrameworkCore;

    using static Common.EntityValidationConstants.Specification;

    [Comment("Specification of Recipe-Ingredient")]
    public class Specification
    {
        public Specification()
        {
            this.RecipesIngredients = new List<RecipeIngredient>();
        }

        [Comment("Key Identifier")]
        [Key]
        public int Id { get; set; }

        [Comment("Specification Description")]
        [Required]
        [MaxLength(DescriptionMaxLength)]
        public string Description { get; set; } = null!;

        [Comment("A collection of Recipe Ingredients with Specification")]
        public ICollection<RecipeIngredient> RecipesIngredients { get; set; }
    }
}
