namespace CookTheWeek.Web.ViewModels.MealPlan
{
    public class MealPlanActiveModalViewModel
    {       
        public string Id { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string StartDate { get; set; } = null!;

        public string EndDate { get; set; } = null!;

        public int TotalServings { get; set; }

        public int TotalMealsCooked { get; set; }

        public int TotalMealsUncooked { get; set; }

        public int TotalMealsCount { get; set; }

        public int TotalCookingTimeMinutes { get; set; }

        public int TotalIngredients { get; set; }

        public int DaysRemaining { get; set; }

        // For the modal rendering
        public Guid UserId = Guid.Empty;

        public bool JustLoggedIn = false;

        public bool HasActiveMealPlan = false;
    }
}
