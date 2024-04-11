namespace CookTheWeek.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using Microsoft.EntityFrameworkCore;

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

        [Comment("A collection in Recipes in Category")]
        public ICollection<Recipe> Recipes { get; set; }
    }
}
