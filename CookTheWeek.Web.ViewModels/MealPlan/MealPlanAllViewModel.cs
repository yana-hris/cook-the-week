namespace CookTheWeek.Web.ViewModels.MealPlan
{
    public class MealPlanAllViewModel
    {
        public string Id { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string OwnerUsername { get; set; } = null!;

        public string StartDate { get; set; } = null!;

        public string EndDate { get; set; } = null!;

        public int MealsCount { get; set; }
    }
}
