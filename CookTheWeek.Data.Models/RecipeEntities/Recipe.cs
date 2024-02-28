namespace CookTheWeek.Data.Models.RecipeEntities
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using static Common.EntityValidationConstants.Recipe;

    public class Recipe
    {
        public Recipe()
        {
            this.Id = Guid.NewGuid();
            this.RecipesIngredients = new HashSet<RecipeIngredient>();
        }

        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(TitleMaxLength)]
        public string Title { get; set; } = null!;

        [MaxLength(DescriptionMaxLength)]
        public string? Description { get; set; }

        [Required]
        public string Instructions { get; set; } = null!;

        [Required]
        [Range(1, 20)]
        public int Servings { get; set; }

        [Required]
        public TimeSpan TotalTime { get; set; }

        [Required]
        [MaxLength(ImageUlrMaxLength)]
        public string ImageUrl { get; set; } = null!;

        [Required]
        [ForeignKey(nameof(RecipeCategory))]
        public int RecipeCategoryId { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }

        public RecipeCategory RecipeCategory { get; set; } = null!;

        public ICollection<RecipeIngredient> RecipesIngredients { get; set; }


    }
}
