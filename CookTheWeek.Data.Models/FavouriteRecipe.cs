namespace CookTheWeek.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using Microsoft.EntityFrameworkCore;

    [Comment("Users` Favourite Recipes (likes)")]
    public class FavouriteRecipe
    {
        [Comment("User Key Identifier")]
        [Required]
        [ForeignKey(nameof(User))]
        public Guid UserId { get; set; } 
        public ApplicationUser User { get; set; } = null!;

        [Comment("Recipe Key Identifier")]
        [Required]
        [ForeignKey(nameof(Recipe))]
        public Guid RecipeId { get; set; }
        public Recipe Recipe { get; set; } = null!;

        [Comment("Soft Delete the Recipe Like when the Recipe is deleted")]
        [Required]
        public bool IsDeleted { get; set; }
    }
}
