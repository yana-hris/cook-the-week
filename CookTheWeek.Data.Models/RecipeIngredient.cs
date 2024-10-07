namespace CookTheWeek.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using Microsoft.EntityFrameworkCore;

    [Comment("Recipe Ingredient")]
    public class RecipeIngredient
    {
        [Key]
        [Comment("Unique Recipe Ingredient Key identifier")]
        public int Id { get; set; } 

        [Comment("Key Identifier for Recipe")]
        [ForeignKey(nameof(Recipe))]
        [Required]
        public Guid RecipeId { get; set; } 

        [Comment("Recipe")]
        [Required]
        public Recipe Recipe { get; set; } = null!;

        [Comment("Key Identifier for Ingredient")]
        [Required]
        [ForeignKey(nameof(Ingredient) )]
        public int IngredientId { get; set; }

        [Comment("Ingredient")]
        [Required]
        public Ingredient Ingredient { get; set; } = null!;

        [Comment("Quantity of Ingredient in Recipe")]
        [Required]
        public decimal Qty { get; set; }

        [Comment("Measure Key Identifier")]
        [Required]
        public int MeasureId { get; set; }

        [Comment("Measure")]
        [Required]
        public Measure Measure { get; set; } = null!;
        
        [Comment("Key identifier for Specification")]
        [ForeignKey(nameof(Specification))]
        public int? SpecificationId { get; set; }

        [Comment("Specification")]       
        public Specification? Specification { get; set; }

        [Comment("Soft Delete the RecipeIngredient when the Recipe is deleted")]
        [Required]
        public bool IsDeleted { get; set; }
    }
}
