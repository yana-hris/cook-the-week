namespace CookTheWeek.Web.ViewModels.Recipe
{    
    public class RecipeDeleteViewModel
    {
        public string Id { get; set; } = null!;

        public string Title { get; set; } = null!;

        public string ImageUrl { get; set; } = null!;

        public int Servings { get; set; }

        public int TotalTime { get; set; }

        public string CreatedOn { get; set; } = null!;

        public string CategoryName { get; set; } = null!;
    }
}
