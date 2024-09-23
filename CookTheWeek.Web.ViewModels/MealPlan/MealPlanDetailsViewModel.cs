namespace CookTheWeek.Web.ViewModels.MealPlan
{
    using Meal;

    public class MealPlanDetailsViewModel
    {
        public MealPlanDetailsViewModel()
        {
            this.Meals = new List<MealViewModel>();
        }
        public string Id { get; set; } = null!;

        public string Name { get; set; } = null!;
        public string OwnerId { get; set; } = null!;

        public bool IsFinished { get; set; }

        public IList<MealViewModel> Meals { get; set; }

        public int? TotalServings { get; set; }

        public int? TotalCookingTimeMinutes { get; set; }

        public int? TotalIngredients { get; set; }

        public int? TotalCookingDays { get; set; }
    }
}
