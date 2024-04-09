namespace CookTheWeek.Services.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using CookTheWeek.Services.Data.Models.Ingredient;
    using CookTheWeek.Services.Data.Models.RecipeIngredient;
    using CookTheWeek.Web.ViewModels.Ingredient;

    public interface IIngredientService
    {
        Task<AllIngredientsFilteredAndPagedServiceModel> AllAsync(AllIngredientsQueryModel queryModel);
        Task<int> AddAsync(IngredientFormViewModel model);
        Task EditAsync(IngredientEditViewModel model);
        Task<IEnumerable<RecipeIngredientSuggestionServiceModel>> suggestionsAsync(string input);
        Task<IngredientEditViewModel> GetForEditByIdAsync(int id);
        Task<bool> ExistsByIdAsync(int id);
        Task<bool> ExistsByNameAsync(string name);
        Task<int> AllCountAsync();
        Task DeleteById(int id);
        Task<int> GetIdByNameAsync(string name);
        
    }
}
