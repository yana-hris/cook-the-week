namespace CookTheWeek.Web.ViewModels.Meal
{
    using CookTheWeek.Web.ViewModels.Interfaces;
    using CookTheWeek.Web.ViewModels.Step;
    using CookTheWeek.Web.ViewModels.SupplyItem;

    public class MealDetailsViewModel
    {
        public MealDetailsViewModel()
        {
            this.IngredientsByCategories = new List<SupplyItemListModel<IngredientItemViewModel>>();
            this.CookingSteps = new List<StepViewModel>();
        }

        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string ImageUrl { get; set; } = null!;
        public string? Description { get; set; }
        public int ServingSize { get; set; }
        public string CookingTime { get; set; } = null!;
        public string? DifficultyLevel { get; set; }
        public string CookingDate { get; set; } = null!;
        public string CategoryName { get; set; } = null!;           
        public bool IsCooked { get; set; }
        public bool IsMealPlanFinished { get; set; }
        public List<StepViewModel> CookingSteps { get; set; }
        public IEnumerable<ISupplyItemListModel<IngredientItemViewModel>> IngredientsByCategories { get; set; }

    }
}
