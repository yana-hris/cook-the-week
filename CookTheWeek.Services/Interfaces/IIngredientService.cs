namespace CookTheWeek.Services.Data.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using CookTheWeek.Services.Data.Models.Ingredient;
    using CookTheWeek.Services.Data.Models.RecipeIngredient;
    using CookTheWeek.Web.ViewModels.Admin.IngredientAdmin;

    public interface IIngredientService
    {
        Task<AllIngredientsFilteredAndPagedServiceModel> AllAsync(AllIngredientsQueryModel queryModel);
        Task<int> AddAsync(IngredientAddFormModel model);
        Task EditAsync(IngredientEditFormModel model);
        Task<IEnumerable<RecipeIngredientSuggestionServiceModel>> GenerateIngredientSuggestionsAsync(string input);
        Task<IngredientEditFormModel> GetForEditByIdAsync(int id);
        Task<int?> GetIdByNameAsync(string name);
        Task<bool> ExistsByIdAsync(int id);
        Task<bool> ExistsByNameAsync(string name);
        Task<int> AllCountAsync();
        Task DeleteById(int id);
        
    }
}
