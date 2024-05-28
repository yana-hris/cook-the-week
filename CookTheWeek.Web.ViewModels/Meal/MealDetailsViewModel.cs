namespace CookTheWeek.Web.ViewModels.Meal
{
    using CookTheWeek.Web.ViewModels.RecipeIngredient;
    using CookTheWeek.Web.ViewModels.ShoppingList;
    using CookTheWeek.Web.ViewModels.Step;

    public class MealDetailsViewModel
    {
        public MealDetailsViewModel()
        {
            this.IngredientsByCategories = new List<ProductListViewModel>();
            this.CookingSteps = new List<StepViewModel>();
        }

        public int Id { get; set; }

        public string Title { get; set; } = null!;

        public string ImageUrl { get; set; } = null!;

        public string? Description { get; set; }

        public int ServingSize { get; set; }

        public TimeSpan CookingTime { get; set; }

        public string CategoryName { get; set; } = null!;
        
        public List<StepViewModel> CookingSteps { get; set; }
        public ICollection<ProductListViewModel> IngredientsByCategories { get; set; }

    }
}
