namespace CookTheWeek.Web.ViewModels.Meal
{
    
    public class MealViewModel
    {
        public string Id { get; set; } = null!;
        public string RecipeId { get; set; } = null!;
        public string Title { get; set; } = null!;
        public int Servings { get; set; }
        public string CookingTime { get; set; } = null!;
        public string ImageUrl { get; set; } = null!;
        public string CategoryName { get; set; } = null!;
        public string DayOfTheWeek { get; set; } = null!;
        public string Date { get; set; } = null!;
        public bool IsCooked { get; set; }
    }
}
