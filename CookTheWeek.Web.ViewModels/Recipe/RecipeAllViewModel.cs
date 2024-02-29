namespace CookTheWeek.Web.ViewModels.Recipe
{
    using CookTheWeek.Web.ViewModels.Category;

    public class RecipeAllViewModel
    {
        public string Id { get; set; }
        public string ImageUrl { get; set; }
        public string Title { get; set; }
        public string CookingTime { get; set; }
        public RecipeCategorySelectViewModel Category { get; set; }
        public int Servings { get; set; }

    }
}
