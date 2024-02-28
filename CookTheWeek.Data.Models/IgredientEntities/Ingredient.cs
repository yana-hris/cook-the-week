namespace CookTheWeek.Data.Models.IgredientEntities
{    
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using static Common.EntityValidationConstants.Ingredient;
    public class Ingredient
    {
        public Ingredient()
        {
            this.RecipesIngredients = new HashSet<RecipeIngredient>();
        }
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; } = null!;

        [Required]
        [ForeignKey(nameof(IngredientCategory))]
        public int IngredientCategoryId { get; set; }

        public IngredientCategory IngredientCategory { get; set; } = null!;

        public ICollection<RecipeIngredient> RecipesIngredients { get; set; }
    }
}
