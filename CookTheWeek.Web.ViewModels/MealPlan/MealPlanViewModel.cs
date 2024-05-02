namespace CookTheWeek.Web.ViewModels.MealPlan
{
    using Meal;

    public class MealPlanViewModel
    {
        public string Id { get; set; } = null!;

        public string Name { get; set; } = null!;

        public bool IsFinished { get; set; }

        public IList<MealViewModel> Meals { get; set; } = new List<MealViewModel>();

        public int? TotalServings { get; set; }

        public int? TotalCookingTimeMinutes { get; set; }

        public int? TotalIngredients { get; set; }

        public int? TotalCookingDays { get; set; }
    }
}
