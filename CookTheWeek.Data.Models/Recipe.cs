namespace CookTheWeek.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using Microsoft.EntityFrameworkCore;

    using static Common.EntityValidationConstants.Recipe;

    [Comment("Recipe")]
    public class Recipe
    {
        private string title;
        public Recipe()
        {
            Id = Guid.NewGuid();
            this.RecipesIngredients = new List<RecipeIngredient>();
            this.FavouriteRecipes = new HashSet<FavouriteRecipe>();
            this.Meals = new HashSet<Meal>();   
            this.Instructions = new List<Step>();
        }

        [Comment("Key Indetifier")]
        [Key]
        public Guid Id { get; set; }

        [Comment("Recipe Creator")]
        [Required]
        public string OwnerId { get; set; } = null!;

        [Comment("Recipe Title")]
        [Required]
        [MaxLength(TitleMaxLength)]
        public string Title { get; set; } = null!;

        [Comment("Recipe Description")]
        [MaxLength(DescriptionMaxLength)]
        public string? Description { get; set; }

        [Comment("Recipe Instructions")]
        [Required]
        public ICollection<Step> Instructions { get; set; }

        [Comment("Recipe Serving Size")]
        [Required]
        [Range(1, 20)]
        public int Servings { get; set; }

        [Comment("Recipe Cooking Time")]
        [Required]
        public TimeSpan TotalTime { get; set; }

        [Comment("Recipe Image Link")]
        [Required]
        [MaxLength(ImageUlrMaxLength)]
        public string ImageUrl { get; set; } = null!;

        [Comment("Recipe Category Key Identifier")]
        [Required]
        [ForeignKey(nameof(Category))]
        public int CategoryId { get; set; }
        public RecipeCategory Category { get; set; } = null!;

        [Comment("Date and Time of a Recipe Creation")]
        [Required]
        public DateTime CreatedOn { get; set; }

        [Comment("Soft Delete for a Recipe")]
        [Required]
        public bool IsDeleted { get; set; }

        [Comment("A collection Recipe-Ingredients with a Recipe")]
        public ICollection<RecipeIngredient> RecipesIngredients { get; set; }

        [Comment("A collection of Users who have added a Recipe to their Favourite-Recipes")]
        public ICollection<FavouriteRecipe> FavouriteRecipes { get; set; }

        [Comment("A collection of Meals with a Recipe")]
        public ICollection<Meal> Meals { get; set; }

        
    }
}
