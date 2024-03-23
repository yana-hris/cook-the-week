namespace CookTheWeek.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class FavouriteRecipe
    {
        [Required]
        [ForeignKey(nameof(User))]
        public Guid UserId { get; set; } 
        public ApplicationUser User { get; set; } = null!;

        [Required]
        [ForeignKey(nameof(Recipe))]
        public Guid RecipeId { get; set; }
        public Recipe Recipe { get; set; } = null!;
    }
}
