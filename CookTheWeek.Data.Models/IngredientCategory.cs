namespace CookTheWeek.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using Microsoft.EntityFrameworkCore;

    using CookTheWeek.Data.Models.Interfaces;

    using static CookTheWeek.Common.EntityValidationConstants.CategoryValidation;

    [Comment("Ingredient Category")]
    public class IngredientCategory : ICategory
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
