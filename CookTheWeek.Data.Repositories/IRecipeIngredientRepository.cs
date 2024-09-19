namespace CookTheWeek.Data.Repositories
{
    using CookTheWeek.Data.Models;

    public interface IRecipeIngredientRepository
    {
        Task AddAllAsync(ICollection<RecipeIngredient> recipeIngredients);
        
        Task UpdateAllAsync(string recipeId, ICollection<RecipeIngredient> recipeIngredients);

        Task DeleteAllAsync(string recipeId);
    }
}
