namespace CookTheWeek.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    
    using Microsoft.EntityFrameworkCore;

    using static Common.EntityValidationConstants.RecipeIngredient;

    [Comment("Ingredient")]
    public class Ingredient
    {
        public Ingredient()
        {
            this.RecipesIngredients = new HashSet<RecipeIngredient>();
        }
        [Comment("Key Identifier")]
        [Key]
        public int Id { get; set; }

        [Comment("Ingredient Name")]
        [Required]
        [MaxLength(RecipeIngredientNameMaxLength)]
        public string Name { get; set; } = null!;

        [Comment("Ingredient Category Key Identifier")]
        [Required]
        [ForeignKey(nameof(Category))]
        public int CategoryId { get; set; }
        public IngredientCategory Category { get; set; } = null!;

        [Comment("A collection Recipe-Ingredients with an Ingredient")]
        public ICollection<RecipeIngredient> RecipesIngredients { get; set; } = null!;
    }
}
