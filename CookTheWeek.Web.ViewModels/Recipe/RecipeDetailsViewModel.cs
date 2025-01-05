namespace CookTheWeek.Web.ViewModels.Recipe
{
    using CookTheWeek.Web.ViewModels.Interfaces;
    using CookTheWeek.Web.ViewModels.RecipeIngredient;
    using CookTheWeek.Web.ViewModels.Step;
    using CookTheWeek.Web.ViewModels.SupplyItem;

    public class RecipeDetailsViewModel
    {
        public RecipeDetailsViewModel()
        {
            this.Steps = new List<StepViewModel>();
        }
        public string Id { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string? Description { get; set; } 
        public int Servings { get; set; }
        public string TotalTime { get; set; } = null!;
        public string ImageUrl { get; set; } = null!;
        public string CategoryName { get; set; } = null!;
        public string DifficultyLevel { get; set; } = null!;
        public string CreatedOn { get; set; } = null!;
        public string CreatedBy { get; set; } = null!;
        public bool IsLikedByUser { get; set; }
        public int? LikesCount { get; set; }
        public int? CookedCount { get; set; }
        public bool IsSiteRecipe { get; set; }
        public bool IsInActiveMealPlan { get; set; }
        public List<StepViewModel> Steps { get; set; }

        public IEnumerable<ISupplyItemListModel<RecipeIngredientDetailsViewModel>> RecipeIngredientsByCategories =
                                                new List<SupplyItemListModel<RecipeIngredientDetailsViewModel>>();
    }
}
