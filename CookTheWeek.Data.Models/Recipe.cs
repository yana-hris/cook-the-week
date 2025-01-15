namespace CookTheWeek.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using Microsoft.EntityFrameworkCore;

    using CookTheWeek.Common.Enums;

    using static Common.EntityValidationConstants.RecipeValidation;

    [Comment("Recipe")]
    public class Recipe
    {
        public Recipe()
        {
            Id = Guid.NewGuid();

            this.RecipesIngredients = new HashSet<RecipeIngredient>();
            this.FavouriteRecipes = new HashSet<FavouriteRecipe>();
            this.Steps = new HashSet<Step>();
            this.Meals = new HashSet<Meal>();   
            this.RecipeTags = new HashSet<RecipeTag>();
            this.Ratings = new HashSet<RecipeRating>();

        }

        [Comment("Key Indetifier")]
        [Key]
        public Guid Id { get; set; }


        [Comment("Recipe Creator")]
        [Required]
        [ForeignKey(nameof(Owner))]
        public Guid OwnerId { get; set; }
        public ApplicationUser Owner { get; set; } = null!;


        [Comment("Recipe Title")]
        [Required]
        [MaxLength(TitleMaxLength)]
        public string Title { get; set; } = null!;


        [Comment("Recipe Description")]
        [MaxLength(DescriptionMaxLength)]
        public string? Description { get; set; }     
        

        [Comment("Recipe Serving Size")]
        [Required]
        [Range(ServingsMinValue, ServingsMaxValue)]
        public int Servings { get; set; }


        [Comment("Recipe Cooking Time")]
        [Required]
        public int TotalTimeMinutes { get; set; }


        [Comment("Recipe Total Views")]
        public int Views { get; set; } = 0;


        [Comment("Recipe Average Rating calculated")]
        public double AverageRating { get; set; } = 0.0;


        [Comment("Recipe Image Link")]
        [Required]
        [MaxLength(ImageUlrMaxLength)]
        public string ExternalImageUrl { get; set; } = null!;


        [Comment("Cloudinary Image Link")]
        [MaxLength(ImageUlrMaxLength)]
        public string? InternalImageUrl { get; set; }


        [Comment("Recipe Category Key Identifier")]
        [Required]
        [ForeignKey(nameof(Category))]
        public int CategoryId { get; set; }
        public RecipeCategory Category { get; set; } = null!;


        [Comment("Date and Time of a Recipe Creation")]
        [Required]
        public DateTime CreatedOn { get; set; }


        [Comment("Soft Delete the Recipe")]
        [Required]
        public bool IsDeleted { get; set; }


        [Comment("Indicator for Recipe Ownership")]
        [Required]
        public bool IsSiteRecipe { get; set; }


        [Comment("Level of difficulty for the Recipe")]
        public DifficultyLevel? DifficultyLevel { get; set; }


        [Comment("Recipe Cooking Instructions")]
        public ICollection<Step> Steps { get; set; }


        [Comment("A collection of Recipe Ingredients to cook a Recipe")]
        public ICollection<RecipeIngredient> RecipesIngredients { get; set; }


        [Comment("A collection of Users who have added the Recipe to their Favourite-Recipes")]
        public ICollection<FavouriteRecipe> FavouriteRecipes { get; set; }


        [Comment("A collection of Meals cooked with the Recipe")]
        public ICollection<Meal> Meals { get; set; }


        [Comment("A collection of Recipe Tags")]
        public ICollection<RecipeTag> RecipeTags { get; set; }


        [Comment("A collection of Recipe-User Ratings")]
        public ICollection<RecipeRating> Ratings { get; set; }

    }
}
