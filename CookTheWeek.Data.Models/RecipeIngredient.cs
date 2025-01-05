namespace CookTheWeek.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using Microsoft.EntityFrameworkCore;

    using static Common.EntityValidationConstants.RecipeIngredientValidation;
    using static Common.EntityValidationConstants.RecipeIngredientQtyValidation;

    [Comment("Recipe Ingredient")]
    public class RecipeIngredient
    {
        [Key]
        [Comment("Unique Recipe Ingredient Key identifier")]
        public int Id { get; set; } 

        [Comment("Key Identifier for Recipe")]
        [ForeignKey(nameof(Recipe))]
        public Guid RecipeId { get; set; } 

        [Comment("Recipe")]
        public Recipe Recipe { get; set; }

        [Comment("Key Identifier for Ingredient")]
        [Required]
        [ForeignKey(nameof(Ingredient) )]
        public int IngredientId { get; set; }

        [Comment("Ingredient")]
        [Required]
        public Ingredient Ingredient { get; set; } = null!;

        [Comment("Quantity of Ingredient in Recipe")]
        [Range(QtyMinDecimalValue, QtyMaxDecimalValue, ErrorMessage = InvalidDecimalRangeErrorMessage)]
        [Required]
        [Column(TypeName = "decimal(10,3)")]
        public decimal Qty { get; set; }

        [Comment("Measure Key Identifier")]
        [Required]
        public int MeasureId { get; set; }

        [Comment("Measure")]
        [Required]
        public Measure Measure { get; set; } = null!;

        [Comment("User Notes for the given RecipeIngredient")]
        public string? Note { get; set; } 

        [Comment("Soft Delete the RecipeIngredient when the Recipe is deleted")]
        [Required]
        public bool IsDeleted { get; set; }
    }
}
