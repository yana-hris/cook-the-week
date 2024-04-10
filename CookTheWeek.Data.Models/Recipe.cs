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
        }

        [Comment("Key Indetifier")]
        [Key]
        public Guid Id { get; set; }

        [Comment("Creator of the recipe")]
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
        public string Instructions { get; set; } = null!;

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
        [ForeignKey(nameof(RecipeCategory))]
        public int RecipeCategoryId { get; set; }
        public RecipeCategory RecipeCategory { get; set; } = null!;

        [Comment("Recipe Date and Time Creation")]
        [Required]
        public DateTime CreatedOn { get; set; }

        [Comment("Soft Delete for Recipe")]
        [Required]
        public bool IsDeleted { get; set; }

        [Comment("A collection of Ingredients for this Recipe")]
        public ICollection<RecipeIngredient> RecipesIngredients { get; set; }

        [Comment("A collection of users who have added this recipe in their Favourite Recipes Collection")]
        public ICollection<FavouriteRecipe> FavouriteRecipes { get; set; }

        [Comment("A collection of meals that will be cooked with this Recipe")]
        public ICollection<Meal> Meals { get; set; } 
    }
}
