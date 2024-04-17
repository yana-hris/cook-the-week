namespace CookTheWeek.Services.Data.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using CookTheWeek.Web.ViewModels.Meal;
    using Data.Models.Recipe;
    using Web.ViewModels.Recipe;

    public interface IRecipeService
    {
        Task<AllRecipesFilteredAndPagedServiceModel> AllAsync(AllRecipesQueryModel queryModel);
        Task<string> AddAsync(RecipeAddFormModel model, string ownerId);
        Task<RecipeDetailsViewModel> DetailsByIdAsync(string id);
        Task<bool> ExistsByIdAsync(string id);
        Task<RecipeEditFormModel> GetForEditByIdAsync(string id);
        Task EditAsync(RecipeEditFormModel model);
        Task<RecipeDeleteViewModel> GetForDeleteByIdAsync(string id);
        Task DeleteByIdAsync(string id);
        Task<ICollection<RecipeAllViewModel>> AllAddedByUserAsync(string userId);
        Task<int> MineCountAsync(string userId);         
        Task<int> AllCountAsync();
        Task<bool> IsIncludedInMealPlans(string id);
        Task<MealAddFormModel> GetForMealByIdAsync(string recipeId);
    }
}
