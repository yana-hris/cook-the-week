namespace CookTheWeek.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using CookTheWeek.Data.Models.IgredientEntities;
    using CookTheWeek.Data.Models.RecipeEntities;
    using Microsoft.EntityFrameworkCore;

    [Comment("Съставки към рецепти")]
    public class RecipeIngredient
    {
        [Comment("Ключ рецепта")]
        [ForeignKey(nameof(Recipe))]
        [Required]
        public Guid RecipeId { get; set; } 

        [Comment("Рецепта")]
        [Required]
        public Recipe Recipe { get; set; } = null!;

        [Comment("Ключ съставка")]
        [Required]
        [ForeignKey(nameof(Ingredient) )]
        public int IngredientId { get; set; }

        [Comment("Съставка")]
        [Required]
        public Ingredient Ingredient { get; set; } = null!;

        [Comment("Количество на съставка в рецепта")]
        [Required]
        public decimal Qty { get; set; }

        [Comment("Мерна единица за съставка в рецепта")]
        [Required]
        public int MeasureId { get; set; }

        [Comment("Мерна единица")]
        [Required]
        public Measure Measure { get; set; } = null!;

        [Comment("Пояснение към съставката (замразен, от консерва, нарязан на парченца и т.н.")]
        //[Comment("Special condition for this ingredient in this recipe, e.g. frozen, canned, sliced, etc.")]
        public Specification? Specification { get; set; }
    }
}
