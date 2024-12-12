namespace CookTheWeek.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using Microsoft.EntityFrameworkCore;

    [Comment("Recipe Rating by a given User")]
    public class RecipeRating
    {
        [Comment("Recipe Key Identifier")]
        [Required]
        [ForeignKey(nameof(Recipe))]
        public Guid RecipeId { get; set; }

        public Recipe Recipe { get; set; } = null!;

        [Comment("User Key Identifier")]
        [Required]
        [ForeignKey(nameof(User))]
        public Guid UserId { get; set; }
        public ApplicationUser User { get; set; } = null!;

        [Comment("User Rating for a given Recipe from 1 to 5")]
        [Required]
        [Range(1, 5)]
        public int RatingValue { get; set; }

        [Comment("User Comment for a given Recipe explaining Rativng Value")]
        public string? RatingText { get; set; }

        [Comment("Time of Rating Creation")]
        [Required]
        public DateTime CreatedOn { get; set; }

        [Comment("Soft Delete the Rating when the Recipe is deleted")]
        [Required]
        public bool IsDeleted { get; set; }
    }
}
