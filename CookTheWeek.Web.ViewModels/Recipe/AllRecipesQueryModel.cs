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
            this.RecipesPerPage = EntitiesPerPage;
            
            this.Categories = new HashSet<string>();
            this.Recipes = new HashSet<RecipeAllViewModel>();
        }
        public string? Category { get; set; }

        [Display(Name = "Search by word")]
        public string? SearchString { get; set; }

        [Display(Name = "Sort Recipes by")]
        public RecipeSorting RecipeSorting { get; set; }

        public int CurrentPage { get; set; }

        public int RecipesPerPage { get; set; }

        public int TotalRecipes { get; set; }

        public ICollection<string> Categories { get; set; }

        public IDictionary<int, string> RecipeSortings { get; set; }

        public ICollection<RecipeAllViewModel> Recipes { get; set; }
    }
}
