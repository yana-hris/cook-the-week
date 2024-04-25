namespace CookTheWeek.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using Microsoft.EntityFrameworkCore;

    using static Common.EntityValidationConstants.Meal;

    [Comment("Meal")]
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
        [Required]
        public DateTime CookDate { get; set; }

        [Comment("Meal completion Identifier")]
        [Required]
        public bool IsCooked { get; set; }

        [Comment("Meal Plan Key Identifier")]
        [Required]
        [ForeignKey(nameof(MealPlan))]
        public Guid MealPlanId { get; set; }
        public MealPlan MealPlan { get; set; } = null!;
    }
}
