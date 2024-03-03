namespace CookTheWeek.Services.Interfaces
{
    using CookTheWeek.Web.ViewModels.RecipeIngredient;

    public interface IRecipeIngredientService
    {
        Task<bool> IngredientMeasureExistsAsync(int measureId);
        Task<bool> IngredientSpecificationExistsAsunc(int specificationId);
        Task<bool> IngredientIsAlreadyAddedAsync(string ingredientName, string recipeId);
        //Task<int> AddAsync(RecipeIngredientFormViewModel model, int recipeId);

        Task<ICollection<RecipeIngredientMeasureViewModel>> GetRecipeIngredientMeasuresAsync();
        Task<ICollection<RecipeIngredientSpecificationViewModel>> GetRecipeIngredientSpecificationsAsync();
    }
}
