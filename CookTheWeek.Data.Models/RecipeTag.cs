namespace CookTheWeek.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using Microsoft.EntityFrameworkCore;

    [Comment("Recipe`s Tags")]
    public class RecipeTag
    {
        [Comment("Recipe Key Identifier")]
        [Required]
        [ForeignKey(nameof(Recipe))]
        public Guid RecipeId { get; set; }
        public Recipe Recipe { get; set; } = null!;

        [Comment("Tag Key Identifier")]
        [Required]
        [ForeignKey(nameof(Tag))]
        public int TagId { get; set; }
        public Tag Tag { get; set; } = null!;
    }
}
