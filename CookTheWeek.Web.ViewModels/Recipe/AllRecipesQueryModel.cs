namespace CookTheWeek.Web.ViewModels.Recipe
{
    using System.ComponentModel.DataAnnotations;

    using Enums;

    using static Common.GeneralApplicationConstants;

    public class AllRecipesQueryModel
    {
        public AllRecipesQueryModel()
        {
            this.CurrentPage = DefaultPage;
            this.RecipesPerPage = DefaultRecipesPerPage;
            
            this.Categories = new HashSet<string>();
            this.Recipes = new HashSet<RecipeAllViewModel>();
        }
        public string? Category { get; set; }

        [Display(Name = "Search for..")]
        public string? SearchString { get; set; }

        [Display(Name = "Sort by")]
        public RecipeSorting RecipeSorting { get; set; }

        public int CurrentPage { get; set; }

        public int RecipesPerPage { get; set; }

        public int TotalRecipes { get; set; }

        public ICollection<string> Categories { get; set; } = null!;

        public IDictionary<int, string> RecipeSortings { get; set; } = null!;

        public ICollection<RecipeAllViewModel> Recipes { get; set; }
    }
}
