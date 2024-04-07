namespace CookTheWeek.Web.ViewModels.Recipe
{    
    public class RecipeMineViewModel
    {
        public RecipeMineViewModel()
        {
            this.FavouriteRecipes = new HashSet<RecipeAllViewModel>();
            this.OwnedRecipes = new HashSet<RecipeAllViewModel>();
        }

        public ICollection<RecipeAllViewModel> FavouriteRecipes { get; set; }

        public ICollection<RecipeAllViewModel> OwnedRecipes { get; set; }
    }
}
