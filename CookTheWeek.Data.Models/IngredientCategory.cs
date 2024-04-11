namespace CookTheWeek.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using Microsoft.EntityFrameworkCore;

    using static CookTheWeek.Common.EntityValidationConstants.IngredientCategory;

    [Comment("Ingredient Category")]
    public class IngredientCategory
    {
        public IngredientCategory()
        {
            Ingredients = new HashSet<Ingredient>();
        }

        [Comment("Key identifier")]
        [Key]
        public int Id { get; set; }

        [Comment("Ingredient Category Name")]
        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; } = null!;

        [Comment("A collection of Ingridients in a Category")]
        public ICollection<Ingredient> Ingredients { get; set; }
    }
}
