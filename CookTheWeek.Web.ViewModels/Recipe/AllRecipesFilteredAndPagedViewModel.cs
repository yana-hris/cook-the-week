namespace CookTheWeek.Web.ViewModels.Recipe
{
    using System.ComponentModel.DataAnnotations;

    using CookTheWeek.Web.ViewModels.MealPlan;
    using CookTheWeek.Web.ViewModels.Recipe.Enums;

    public class AllRecipesFilteredAndPagedViewModel
    {
        public AllRecipesFilteredAndPagedViewModel()
        {
            this.Recipes = new HashSet<RecipeAllViewModel>();
            this.ActiveMealPlan = new MealPlanActiveModalViewModel();
        }

        [Display(Name = "Search for..")]
        public string? SearchString { get; set; }

        public int TotalRecipes { get; set; }

        [Display(Name = "Sort by")]
        public RecipeSorting RecipeSorting { get; set; }

        public string? Category { get; set; }

        public int RecipesPerPage { get; set; }

        public int CurrentPage { get; set; }

        public ICollection<RecipeAllViewModel> Recipes { get; set; }

        public IDictionary<int, string> RecipeSortings { get; set; } = null!;
        public ICollection<string> Categories { get; set; } = null!;
        public MealPlanActiveModalViewModel ActiveMealPlan { get; set; }
    }

    
}
