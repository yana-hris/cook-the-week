namespace CookTheWeek.Web.ViewModels.Recipe
{    
    public class RecipeAllViewModel
    {
        public string Id { get; set; }
        public string ImageUrl { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public string Category { get; set; }
        public int Servings { get; set; }
        public string CookingTime { get; set; }

    }
}
