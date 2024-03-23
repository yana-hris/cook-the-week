namespace CookTheWeek.Data.Models
{
    using Microsoft.EntityFrameworkCore;
    using System.ComponentModel.DataAnnotations;

    using static CookTheWeek.Common.EntityValidationConstants.IngredientCategory;

    [Comment("Ingredients Category")]
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

        [Comment("Ingridients in this category")]
        public ICollection<Ingredient> Ingredients { get; set; }
    }
}
