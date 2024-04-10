namespace CookTheWeek.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    
    using Microsoft.EntityFrameworkCore;

    using static Common.EntityValidationConstants.Ingredient;

    [Comment("Ingredient")]
    public class Ingredient
    {
        private string name;
        public Ingredient()
        {
            RecipesIngredients = new HashSet<RecipeIngredient>();
        }
        [Comment("Key Identifier")]
        [Key]
        public int Id { get; set; }

        [Comment("Ingredient Name")]
        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; } = null!;

        [Comment("Ingredient Category Key Identifier")]
        [Required]
        [ForeignKey(nameof(IngredientCategory))]
        public int IngredientCategoryId { get; set; }
        public IngredientCategory IngredientCategory { get; set; } = null!;

        [Comment("A collection of recipes, containing the ingredient")]
        public ICollection<RecipeIngredient> RecipesIngredients { get; set; }
    }
}
