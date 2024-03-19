namespace CookTheWeek.Web.ViewModels.Recipe
{    
    public class RecipeDeleteViewModel
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string ImageUrl { get; set; }

        public int Servings { get; set; }

        public int TotalTime { get; set; }

        public string CreatedOn { get; set; }

        public string CategoryName { get; set; }
    }
}
