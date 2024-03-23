namespace CookTheWeek.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Microsoft.EntityFrameworkCore;

    [Comment("The Ingredients in Recipe")]
    public class RecipeIngredient
    {
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

        [Comment("Quantity of the Ingredient in this Recipe")]
        [Required]
        public decimal Qty { get; set; }

        [Comment("Measure Key Identifier")]
        [Required]
        public int MeasureId { get; set; }

        [Comment("Measure")]
        [Required]
        public Measure Measure { get; set; } = null!;

        /// <summary>
        /// "Special condition for this ingredient in this recipe, e.g. frozen, canned, sliced, etc.")
        /// </summary>
        [Comment("Specification Key Identifier")]
        [ForeignKey(nameof(Specification))]
        public int? SpecificationId { get; set; }

        [Comment("Specification")]       
        public Specification? Specification { get; set; }
    }
}
