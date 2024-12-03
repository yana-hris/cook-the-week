namespace CookTheWeek.Web.ViewModels.Interfaces
{
    using CookTheWeek.Web.ViewModels.RecipeIngredient;
    using CookTheWeek.Web.ViewModels.Step;

    public interface IRecipeFormModel
    {
        string Title { get; set; }
        string? Description { get; set; }
        int? Servings { get; set; }
        int? CookingTimeMinutes { get; set; }
        string ImageUrl { get; set; }
        int? DifficultyLevelId { get; set; }
        int? RecipeCategoryId { get; set; }
        List<int> SelectedTagIds { get; set; }
        List<StepFormModel> Steps { get; set; }
        List<RecipeIngredientFormModel> RecipeIngredients { get; set; }        
        ICollection<int>? ServingsOptions { get; set; }
        ICollection<SelectViewModel>? Categories { get; set; }
        ICollection<SelectViewModel>? DifficultyLevels { get; set; }
        ICollection<SelectViewModel>? AvailableTags { get; set; }
    }
}
