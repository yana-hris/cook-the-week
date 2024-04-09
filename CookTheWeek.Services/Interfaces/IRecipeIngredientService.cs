namespace CookTheWeek.Services.Interfaces
{
    using CookTheWeek.Web.ViewModels.RecipeIngredient;

    public interface IRecipeIngredientService
    {
        Task<bool> IngredientMeasureExistsAsync(int measureId);
        Task<bool> IngredientSpecificationExistsAsync(int specificationId);
        Task<bool> IsAlreadyAddedAsync(string ingredientName, string recipeId);
        Task<int> AddAsync(RecipeIngredientFormViewModel model, string recipeId);
        Task<ICollection<RecipeIngredientSelectMeasureViewModel>> GetRecipeIngredientMeasuresAsync();
        Task<ICollection<RecipeIngredientSelectSpecificationViewModel>> GetRecipeIngredientSpecificationsAsync();
        Task RemoveAsync(int ingredientId, string recipeId);
    }
}
