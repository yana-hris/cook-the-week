namespace CookTheWeek.Services.Interfaces
{
    using CookTheWeek.Services.Data.Models.RecipeIngredient;
    using CookTheWeek.Web.ViewModels.Ingredient;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IIngredientService
    {
        Task<bool> existsByNameAsync(string name);
        Task<int> AddIngredientAsync(IngredientFormViewModel model);
        Task<IEnumerable<RecipeIngredientServiceModel>> GetIngredientSuggestionsAsync(string input);
        Task<IEnumerable<RecipeIngredientServiceModel>> GetAllIngredientsAsync();
    }
}
