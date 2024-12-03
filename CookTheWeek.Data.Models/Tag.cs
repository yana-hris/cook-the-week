namespace CookTheWeek.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using Microsoft.EntityFrameworkCore;

    using static Common.EntityValidationConstants.TagValidation;

    [Comment("Recipe Tag")]
    public class Tag
    {
        public Tag()
        {
            this.RecipeTags = new HashSet<RecipeTag>();    
        }
        [Comment("Key identifier")]
        [Key]
        public int Id { get; set; }

        [Comment("Meal Plan Name")]
        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; } = null!;

        [Comment("A collection of Recipe Tags")]
        public ICollection<RecipeTag> RecipeTags { get; set; }
    }
}
