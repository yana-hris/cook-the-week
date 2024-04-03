namespace CookTheWeek.Services.Interfaces
{
    using CookTheWeek.Services.Data.Models.Ingredient;
    using CookTheWeek.Services.Data.Models.RecipeIngredient;
    using CookTheWeek.Web.ViewModels.Ingredient;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IIngredientService
    {
        Task<bool> ExistsByNameAsync(string name);

        Task<bool> ExistsByIdAsync(int id);
        Task<int> AddIngredientAsync(IngredientFormViewModel model);
        Task<IEnumerable<RecipeIngredientSuggestionServiceModel>> GetIngredientSuggestionsAsync(string input);
        Task<IEnumerable<IngredientServiceModel>> GetAllIngredientsAsync();
        Task<IEnumerable<IngredientServiceModel>> GetAllByCategoryId(int categoryId);
        Task<IngredientEditViewModel?> GetByIdAsync(int id);
        Task EditAsync(IngredientEditViewModel model);
        Task<AllIngredientsFilteredAndPagedServiceModel> AllAsync(AllIngredientsQueryModel queryModel);
        Task<int> AllCountAsync();
    }
}
