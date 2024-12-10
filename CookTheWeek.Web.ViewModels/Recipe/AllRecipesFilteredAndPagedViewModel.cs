namespace CookTheWeek.Web.ViewModels.Recipe
{
    using System.ComponentModel.DataAnnotations;

    using CookTheWeek.Web.ViewModels.MealPlan;

    public class AllRecipesFilteredAndPagedViewModel
    {
        public AllRecipesFilteredAndPagedViewModel()
        {
            this.Recipes = new HashSet<RecipeAllViewModel>();
            this.MealTypes = new List<SelectViewModel>();
            this.DifficultyLevels = new List<SelectViewModel>();
            this.AvailableTags = new List<SelectViewModel>();
            this.RecipeSortings = new List<SelectViewModel>();
            this.SelectedTagIds = new List<int>();
            this.ActiveMealPlan = new MealPlanActiveModalViewModel();
            this.PreparationTimes = new List<int>();
            this.RecipeSources = new List<SelectViewModel>();
        }

        // Filters

        [Display(Name = "Search for..")]
        public string? SearchString { get; set; }

        [Display(Name = "Meal Type")]
        public int? MealTypeId { get; set; }

        [Display(Name = "Ready In")]
        public int? MaxPreparationTime { get; set; }

        [Display(Name = "Level")]
        public int? DifficultyLevel { get; set; }

        [Display(Name = "Level")]
        public List<int>? SelectedTagIds { get; set; }

        [Display(Name = "Recipe Source")]
        public int? RecipeSource { get; set; }


        // Pagination
        public int CurrentPage { get; set; }
        public int RecipesPerPage { get; set; }
        public int TotalResults { get; set; }


        // Sorting

        [Display(Name = "Sort by")]
        public int? RecipeSorting { get; set; }


        // Result Set Collection
        public ICollection<RecipeAllViewModel> Recipes { get; set; }


        // Select Options
        public ICollection<SelectViewModel> MealTypes { get; set; } = null!;
        public ICollection<SelectViewModel> DifficultyLevels { get; set; } = null!;
        public ICollection<SelectViewModel> AvailableTags { get; set; } = null!;
        public ICollection<SelectViewModel> RecipeSortings { get; set; } = null!;
        public IReadOnlyList<int> PreparationTimes { get; set; } = null!;
        public ICollection<SelectViewModel> RecipeSources { get; set; } = null!;


        // Meal Plan Modal Needed Data (optional)
        public MealPlanActiveModalViewModel ActiveMealPlan { get; set; }
    }

    
}
