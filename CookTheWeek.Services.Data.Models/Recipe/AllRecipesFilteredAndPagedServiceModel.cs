namespace CookTheWeek.Services.Data.Models.Recipe
{
    using CookTheWeek.Web.ViewModels.Recipe;

    public class AllRecipesFilteredAndPagedServiceModel
    {
        public AllRecipesFilteredAndPagedServiceModel()
        {
            this.Recipes = new HashSet<RecipeAllViewModel>();
        }

        public int TotalRecipesCount { get; set; }

        public ICollection<RecipeAllViewModel> Recipes { get; set; }
    }
}
