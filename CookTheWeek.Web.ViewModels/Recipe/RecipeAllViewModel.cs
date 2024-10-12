namespace CookTheWeek.Web.ViewModels.Recipe
{
    using CookTheWeek.Web.ViewModels.Category;

    public class RecipeAllViewModel
    {
        public Guid Id { get; set; } 
        public Guid OwnerId { get; set; }
        public string ImageUrl { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public string CookingTime { get; set; } = null!;
        public RecipeCategorySelectViewModel Category { get; set; } = null!;
        public int Servings { get; set; }

    }
}
