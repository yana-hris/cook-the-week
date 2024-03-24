namespace CookTheWeek.Data.Models
{
    using Microsoft.EntityFrameworkCore;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using static Common.EntityValidationConstants.Meal;

    [Comment("A Meal is a Recipe with user-defined Serving Size and Cook Date")]
    public class Meal
    {
        [Comment("Meal Key Identifier")]
        [Key]
        public int Id { get; set; }

        [Comment("Recipe Key Identifier")]
        [Required]
        [ForeignKey(nameof(Recipe))]
        public Guid RecipeId { get; set; } 

        public Recipe Recipe { get; set; } = null!;

        [Comment("Meal Serving Size")]
        [Required]
        [Range(MinServingSize,MaxServingSize)]
        public int ServingSize { get; set; }

        [Comment("Meal Cook Date")]
        public DateTime? CookDate { get; set; }

        [Comment("Recipe is cooked or not")]
        [Required]
        public bool IsCooked { get; set; }
    }
}
