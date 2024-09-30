namespace CookTheWeek.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using Microsoft.EntityFrameworkCore;

    using static Common.EntityValidationConstants.RecipeValidation;

    [Comment("Recipe")]
    public class Recipe
    {
        public Recipe()
        {
            Id = Guid.NewGuid();

            this.RecipesIngredients = new List<RecipeIngredient>();
            this.FavouriteRecipes = new HashSet<FavouriteRecipe>();
            this.Steps = new List<Step>();
            this.Meals = new HashSet<Meal>();   
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


        [Comment("Soft Delete the Recipe")]
        [Required]
        public bool IsDeleted { get; set; }

        [Comment("Indicator for Recipe Ownership")]
        [Required]
        public bool IsSiteRecipe { get; set; }


        [Comment("Recipe Cooking Instructions")]
        public ICollection<Step> Steps { get; set; }


        [Comment("A collection of Recipe Ingredients to cook a Recipe")]
        public ICollection<RecipeIngredient> RecipesIngredients { get; set; }


        [Comment("A collection of Users who have added the Recipe to their Favourite-Recipes")]
        public ICollection<FavouriteRecipe> FavouriteRecipes { get; set; }


        [Comment("A collection of Meals cooked with the Recipe")]
        public ICollection<Meal> Meals { get; set; }

        
    }
}
