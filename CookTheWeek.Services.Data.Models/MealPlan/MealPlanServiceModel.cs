namespace CookTheWeek.Services.Data.Models.MealPlan
{
    
    public class MealPlanServiceModel
    {
        public MealPlanServiceModel()
        {
            this.Meals = new List<MealServiceModel>();
        }

        public string UserId { get; set; }
        public ICollection<MealServiceModel> Meals { get; set; }
    }
}
