namespace CookTheWeek.Data.Models.RecipeEntities
{    
    using System.ComponentModel.DataAnnotations;

    using static Common.EntityValidationConstants.RecipeCategory;
    public class RecipeCategory
    {
        public RecipeCategory()
        {
            this.Recipes = new HashSet<Recipe>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; }

        public ICollection<Recipe> Recipes { get; set; }
    }
}
