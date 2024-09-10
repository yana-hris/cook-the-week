namespace CookTheWeek.Web.ViewModels.Recipe
{
    public class RecipeMineAdminViewModel
    {
        public RecipeMineAdminViewModel()
        {
            this.SiteRecipes = new List<RecipeAllViewModel>();
            this.UserRecipes = new List<RecipeAllViewModel>();
        }

        public ICollection<RecipeAllViewModel> SiteRecipes { get; set; }

        public ICollection<RecipeAllViewModel> UserRecipes { get; set; }
    }

}
