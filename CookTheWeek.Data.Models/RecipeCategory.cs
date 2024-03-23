namespace CookTheWeek.Data.Models
{
    using Microsoft.EntityFrameworkCore;
    using System.ComponentModel.DataAnnotations;

    using static Common.EntityValidationConstants.RecipeCategory;

    [Comment("Recipes Category")]
    public class RecipeCategory
    {
        public RecipeCategory()
        {
            Recipes = new HashSet<Recipe>();
        }

        [Comment("Key Identifier")]
        [Key]
        public int Id { get; set; }

        [Comment("Recipe Category Name")]
        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; }

        [Comment("A collection in recipes in this category")]
        public ICollection<Recipe> Recipes { get; set; }
    }
}
